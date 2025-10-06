namespace DBEditor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            stringsBindingSource = new BindingSource(components);
            splitContainer1 = new SplitContainer();
            btnRefresh = new Button();
            btnCreateTable = new Button();
            btnDeleteTable = new Button();
            listBoxTables = new ListBox();
            btnDeleteField = new Button();
            dataGridView1 = new DataGridView();
            btnAddField = new Button();
            btnSave = new Button();
            btnDeleteRow = new Button();
            ((System.ComponentModel.ISupportInitialize)stringsBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // stringsBindingSource
            // 
            stringsBindingSource.DataSource = typeof(Microsoft.VisualBasic.Strings);
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new Point(12, 12);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(btnRefresh);
            splitContainer1.Panel1.Controls.Add(btnCreateTable);
            splitContainer1.Panel1.Controls.Add(btnDeleteTable);
            splitContainer1.Panel1.Controls.Add(listBoxTables);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(btnDeleteField);
            splitContainer1.Panel2.Controls.Add(dataGridView1);
            splitContainer1.Panel2.Controls.Add(btnAddField);
            splitContainer1.Panel2.Controls.Add(btnSave);
            splitContainer1.Panel2.Controls.Add(btnDeleteRow);
            splitContainer1.Size = new Size(701, 245);
            splitContainer1.SplitterDistance = 233;
            splitContainer1.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(31, 120);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(90, 23);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnCreateTable
            // 
            btnCreateTable.Location = new Point(31, 149);
            btnCreateTable.Name = "btnCreateTable";
            btnCreateTable.Size = new Size(90, 23);
            btnCreateTable.TabIndex = 1;
            btnCreateTable.Text = "Create Table";
            btnCreateTable.UseVisualStyleBackColor = true;
            btnCreateTable.Click += btnCreateTable_Click;
            // 
            // btnDeleteTable
            // 
            btnDeleteTable.Location = new Point(34, 179);
            btnDeleteTable.Name = "btnDeleteTable";
            btnDeleteTable.Size = new Size(87, 23);
            btnDeleteTable.TabIndex = 2;
            btnDeleteTable.Text = "Delete Table";
            btnDeleteTable.UseVisualStyleBackColor = true;
            btnDeleteTable.Click += btnDeleteTable_Click;
            // 
            // listBoxTables
            // 
            listBoxTables.ItemHeight = 15;
            listBoxTables.Location = new Point(3, 3);
            listBoxTables.Name = "listBoxTables";
            listBoxTables.Size = new Size(227, 94);
            listBoxTables.TabIndex = 3;
            listBoxTables.SelectedIndexChanged += listBoxTables_SelectedIndexChanged;
            // 
            // btnDeleteField
            // 
            btnDeleteField.Location = new Point(15, 194);
            btnDeleteField.Name = "btnDeleteField";
            btnDeleteField.Size = new Size(93, 23);
            btnDeleteField.TabIndex = 4;
            btnDeleteField.Text = "DELETE FIELD";
            btnDeleteField.UseVisualStyleBackColor = true;
            btnDeleteField.Click += btnDeleteField_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(458, 150);
            dataGridView1.TabIndex = 0;
            // 
            // btnAddField
            // 
            btnAddField.Location = new Point(15, 165);
            btnAddField.Name = "btnAddField";
            btnAddField.Size = new Size(91, 23);
            btnAddField.TabIndex = 1;
            btnAddField.Text = "ADD FIELD";
            btnAddField.UseVisualStyleBackColor = true;
            btnAddField.Click += btnAddField_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(112, 165);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(93, 23);
            btnSave.TabIndex = 2;
            btnSave.Text = "SAVE";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnDeleteRow
            // 
            btnDeleteRow.Location = new Point(114, 194);
            btnDeleteRow.Name = "btnDeleteRow";
            btnDeleteRow.Size = new Size(91, 23);
            btnDeleteRow.TabIndex = 3;
            btnDeleteRow.Text = "DELETE ROW";
            btnDeleteRow.UseVisualStyleBackColor = true;
            btnDeleteRow.Click += btnDeleteRow_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Name = "MainForm";
            Text = "DBEditor";
            ((System.ComponentModel.ISupportInitialize)stringsBindingSource).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private BindingSource stringsBindingSource;
        private SplitContainer splitContainer1;
        private ListBox listBoxTables;
        private DataGridView dataGridView1;
        private Button btnDeleteTable;
        private Button btnCreateTable;
        private Button btnRefresh;
        private Button btnAddField;
        private Button btnSave;
        private Button btnDeleteRow;
        private Button btnDeleteField;
    }
}
