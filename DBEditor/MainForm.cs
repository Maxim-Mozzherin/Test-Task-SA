using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DBEditor
{
    public partial class MainForm : Form
    {
        private string connectionString;
        private static readonly string[] VALID_TYPES = { "INTEGER", "REAL", "TEXT", "TIMESTAMP" };
        private const string DEFAULT_PK_NAME = "id";

        // Текущие данные для работы с таблицей
        private NpgsqlDataAdapter currentAdapter;
        private DataTable currentTable;
        private string currentTableName;

        public MainForm()
        {
            // Запрашиваем пароль у пользователя
            string password = Microsoft.VisualBasic.Interaction.InputBox(
                "Введите пароль для подключения к базе PostgreSQL:",
                "Подключение к БД",
                "");

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пароль не введён. Программа будет закрыта.");
                Environment.Exit(0);
            }

            // Формируем строку подключения с введённым паролем
            connectionString = $"Host=localhost;Port=5432;Username=postgres;Password={password};Database=Test_DB;";
            InitializeComponent();
            Load += MainForm_Load;
            dataGridView1.DataError += dataGridView1_DataError;
        }

        // Загрузка списка таблиц при запуске формы
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadTables();
        }

        #region Работа со списком таблиц

        // Получение списка всех таблиц из БД и отображение в listBox
        private void LoadTables()
        {
            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();

                const string query = @"
                    SELECT table_name 
                    FROM information_schema.tables 
                    WHERE table_schema = 'public'
                    ORDER BY table_name;";

                using var cmd = new NpgsqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                listBoxTables.Items.Clear();
                while (reader.Read())
                    listBoxTables.Items.Add(reader.GetString(0));
            }
            catch (Exception ex)
            {
                ShowError("загрузке таблиц", ex);
            }
        }

        // Обновление списка таблиц
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTables();
            MessageBox.Show("Список таблиц обновлён.");
        }

        // Обработка выбора таблицы из списка
        private void listBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTables.SelectedItem is string tableName)
            {
                currentTableName = tableName;
                LoadTableData(tableName);
            }
        }

        #endregion

        #region Загрузка и сохранение данных

        // Загрузка данных выбранной таблицы в DataGridView
        private void LoadTableData(string tableName)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                string query = $@"SELECT * FROM public.""{tableName}"";";

                currentAdapter?.Dispose();
                currentAdapter = new NpgsqlDataAdapter(query, conn);
                _ = new NpgsqlCommandBuilder(currentAdapter);

                currentTable = new DataTable();
                currentAdapter.Fill(currentTable);

                dataGridView1.DataSource = currentTable;

                HideIdColumn();
            }
            catch (Exception ex)
            {
                ShowError("загрузке данных", ex);
            }
        }

        // Скрытие колонки id в DataGridView
        private void HideIdColumn()
        {
            if (currentTable?.Columns.Contains(DEFAULT_PK_NAME) == true)
                dataGridView1.Columns[DEFAULT_PK_NAME].Visible = false;
        }

        // Сохранение изменений в БД
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateTableSelected()) return;
            if (currentAdapter == null || currentTable == null)
            {
                MessageBox.Show("Нет данных для сохранения.");
                return;
            }

            try
            {
                currentAdapter.Update(currentTable);
                MessageBox.Show("Данные успешно сохранены!");
                LoadTableData(currentTableName);
            }
            catch (Exception ex)
            {
                ShowError("сохранении данных", ex);
            }
        }

        #endregion

        #region Управление таблицами

        // Создание новой таблицы с пк и полями
        private void btnCreateTable_Click(object sender, EventArgs e)
        {
            string tableName = PromptInput("Введите имя новой таблицы:", "Создание таблицы", "new_table");
            if (string.IsNullOrWhiteSpace(tableName)) return;

            string pkColumn = PromptInput("Введите имя колонки для первичного ключа:",
                "Первичный ключ", DEFAULT_PK_NAME);
            if (string.IsNullOrWhiteSpace(pkColumn))
                pkColumn = DEFAULT_PK_NAME;

            var columns = CollectColumns();

            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();

                var columnDefs = new List<string> { $@"""{pkColumn}"" SERIAL PRIMARY KEY" };
                columnDefs.AddRange(columns.Select(col => $@"""{col.name}"" {col.type}"));

                string createQuery = $@"CREATE TABLE IF NOT EXISTS public.""{tableName}"" ({string.Join(", ", columnDefs)});";

                using var cmd = new NpgsqlCommand(createQuery, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show($"Таблица '{tableName}' создана с PK '{pkColumn}' и {columns.Count} полями.");
                btnRefresh_Click(null, null);
            }
            catch (Exception ex)
            {
                ShowError("создании таблицы", ex);
            }
        }

        // Удаление выбранной таблицы из БД
        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedTable(out string tableName)) return;

            if (!ConfirmAction($"Удалить таблицу '{tableName}'?")) return;

            try
            {
                ExecuteNonQuery($@"DROP TABLE IF EXISTS public.""{tableName}"" CASCADE;");

                MessageBox.Show($"Таблица '{tableName}' удалена.");
                ClearCurrentData();
                btnRefresh_Click(null, null);
            }
            catch (Exception ex)
            {
                ShowError("удалении таблицы", ex);
            }
        }

        #endregion

        #region Управление полями

        // Добавление нового поля в таблицу
        private void btnAddField_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedTable(out string tableName)) return;

            string fieldName = PromptInput("Введите имя нового поля:", "Добавить поле", "new_column");
            if (string.IsNullOrWhiteSpace(fieldName)) return;

            string dataType = PromptDataType(fieldName);
            if (dataType == null) return;

            try
            {
                ExecuteNonQuery($@"ALTER TABLE public.""{tableName}"" ADD COLUMN ""{fieldName}"" {dataType};");

                MessageBox.Show($"Поле '{fieldName}' добавлено в таблицу '{tableName}'.");
                LoadTableData(tableName);
            }
            catch (Exception ex)
            {
                ShowError("добавлении поля", ex);
            }
        }

        // Удаление поля из таблицы
        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedTable(out string tableName)) return;

            string columnName = dataGridView1.CurrentCell?.OwningColumn?.Name;
            if (string.IsNullOrEmpty(columnName))
            {
                MessageBox.Show("Выберите колонку для удаления, кликнув на её ячейку.");
                return;
            }

            if (columnName.Equals(DEFAULT_PK_NAME, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Нельзя удалить первичный ключ.");
                return;
            }

            if (!ConfirmAction($"Удалить поле '{columnName}' из таблицы '{tableName}'?")) return;

            try
            {
                ExecuteNonQuery($@"ALTER TABLE public.""{tableName}"" DROP COLUMN ""{columnName}"";");

                MessageBox.Show($"Поле '{columnName}' удалено.");
                LoadTableData(tableName);
            }
            catch (Exception ex)
            {
                ShowError("удалении поля", ex);
            }
        }

        #endregion

        #region Управление строками

        // Удаление выбранной строки из таблицы
        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку для удаления.");
                return;
            }

            if (!ConfirmAction("Удалить выбранную строку?")) return;

            try
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                currentAdapter.Update(currentTable);
                MessageBox.Show("Строка удалена!");
            }
            catch (Exception ex)
            {
                ShowError("удалении строки", ex);
            }
        }

        #endregion

        #region Вспомогательные методы

        // Обработка ошибок ввода данных в DataGridView
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Ошибка ввода: неверный формат данных для этой колонки.");
            e.ThrowException = false;
        }

        // Проверка, выбрана ли таблица
        private bool ValidateTableSelected()
        {
            if (string.IsNullOrEmpty(currentTableName))
            {
                MessageBox.Show("Сначала выберите таблицу.");
                return false;
            }
            return true;
        }

        // Попытка получить имя выбранной таблицы
        private bool TryGetSelectedTable(out string tableName)
        {
            tableName = listBoxTables.SelectedItem as string;
            if (tableName == null)
            {
                MessageBox.Show("Выберите таблицу.");
                return false;
            }
            return true;
        }

        // Выполнение SQL-запроса без возврата данных
        private void ExecuteNonQuery(string query)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.ExecuteNonQuery();
        }

        // Очистка текущих данных таблицы
        private void ClearCurrentData()
        {
            dataGridView1.DataSource = null;
            currentAdapter?.Dispose();
            currentAdapter = null;
            currentTable = null;
            currentTableName = null;
        }

        // Сбор информации о колонках для новой таблицы
        private List<(string name, string type)> CollectColumns()
        {
            var columns = new List<(string name, string type)>();
            string typeList = string.Join(", ", VALID_TYPES);

            while (true)
            {
                string colName = PromptInput("Введите имя нового поля (оставьте пустым, чтобы закончить):",
                    "Добавить поле", "");
                if (string.IsNullOrWhiteSpace(colName)) break;

                string colType = PromptInput($"Введите тип данных для '{colName}' ({typeList}):",
                    "Тип данных", "TEXT").ToUpper();

                if (!VALID_TYPES.Contains(colType))
                {
                    MessageBox.Show($"Некорректный тип данных для '{colName}'. Поле пропущено.");
                    continue;
                }

                columns.Add((colName, colType));
            }

            return columns;
        }

        // Запрос типа данных для поля
        private string PromptDataType(string fieldName)
        {
            string typeList = string.Join(", ", VALID_TYPES);
            string dataType = PromptInput($"Введите тип данных ({typeList}):",
                "Тип данных", "TEXT").ToUpper();

            if (!VALID_TYPES.Contains(dataType))
            {
                MessageBox.Show("Некорректный тип данных.");
                return null;
            }

            return dataType;
        }

        // Отображение диалога для ввода текста
        private static string PromptInput(string prompt, string title, string defaultValue)
        {
            return Microsoft.VisualBasic.Interaction.InputBox(prompt, title, defaultValue);
        }

        // Запрос подтверждения действия
        private static bool ConfirmAction(string message)
        {
            return MessageBox.Show(message, "Подтверждение", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        private static void ShowError(string operation, Exception ex)
        {
            MessageBox.Show($"Ошибка при {operation}:\n{ex.Message}");
        }

        #endregion


    }
}