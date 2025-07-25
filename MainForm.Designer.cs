namespace Comparatist
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dataGridViewWords = new DataGridView();
            semanticGroupsDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            sourcesDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            languagesDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            rootsDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            stemsDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            wordsDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            inMemoryDatabaseBindingSource = new BindingSource(components);
            buttonAdd = new Button();
            buttonEdit = new Button();
            buttonDelete = new Button();
            buttonSave = new Button();
            buttonOpen = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewWords).BeginInit();
            ((System.ComponentModel.ISupportInitialize)inMemoryDatabaseBindingSource).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewWords
            // 
            dataGridViewWords.AutoGenerateColumns = true;
            dataGridViewWords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewWords.Columns.AddRange(new DataGridViewColumn[] { semanticGroupsDataGridViewTextBoxColumn, sourcesDataGridViewTextBoxColumn, languagesDataGridViewTextBoxColumn, rootsDataGridViewTextBoxColumn, stemsDataGridViewTextBoxColumn, wordsDataGridViewTextBoxColumn });
            dataGridViewWords.DataSource = inMemoryDatabaseBindingSource;
            dataGridViewWords.Location = new Point(5, 50);
            dataGridViewWords.Name = "dataGridViewWords";
            dataGridViewWords.Size = new Size(783, 388);
            dataGridViewWords.TabIndex = 0;
            dataGridViewWords.CellContentClick += dataGridViewWords_CellContentClick;
            // 
            // semanticGroupsDataGridViewTextBoxColumn
            // 
            semanticGroupsDataGridViewTextBoxColumn.DataPropertyName = "SemanticGroups";
            semanticGroupsDataGridViewTextBoxColumn.HeaderText = "SemanticGroups";
            semanticGroupsDataGridViewTextBoxColumn.Name = "semanticGroupsDataGridViewTextBoxColumn";
            semanticGroupsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sourcesDataGridViewTextBoxColumn
            // 
            sourcesDataGridViewTextBoxColumn.DataPropertyName = "Sources";
            sourcesDataGridViewTextBoxColumn.HeaderText = "Sources";
            sourcesDataGridViewTextBoxColumn.Name = "sourcesDataGridViewTextBoxColumn";
            sourcesDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // languagesDataGridViewTextBoxColumn
            // 
            languagesDataGridViewTextBoxColumn.DataPropertyName = "Languages";
            languagesDataGridViewTextBoxColumn.HeaderText = "Languages";
            languagesDataGridViewTextBoxColumn.Name = "languagesDataGridViewTextBoxColumn";
            languagesDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // rootsDataGridViewTextBoxColumn
            // 
            rootsDataGridViewTextBoxColumn.DataPropertyName = "Roots";
            rootsDataGridViewTextBoxColumn.HeaderText = "Roots";
            rootsDataGridViewTextBoxColumn.Name = "rootsDataGridViewTextBoxColumn";
            rootsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // stemsDataGridViewTextBoxColumn
            // 
            stemsDataGridViewTextBoxColumn.DataPropertyName = "Stems";
            stemsDataGridViewTextBoxColumn.HeaderText = "Stems";
            stemsDataGridViewTextBoxColumn.Name = "stemsDataGridViewTextBoxColumn";
            stemsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // wordsDataGridViewTextBoxColumn
            // 
            wordsDataGridViewTextBoxColumn.DataPropertyName = "Words";
            wordsDataGridViewTextBoxColumn.HeaderText = "Words";
            wordsDataGridViewTextBoxColumn.Name = "wordsDataGridViewTextBoxColumn";
            wordsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // inMemoryDatabaseBindingSource
            // 
            inMemoryDatabaseBindingSource.DataSource = typeof(InMemoryDatabase);
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(11, 17);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(75, 23);
            buttonAdd.TabIndex = 1;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            // 
            // buttonEdit
            // 
            buttonEdit.Location = new Point(92, 17);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new Size(75, 23);
            buttonEdit.TabIndex = 2;
            buttonEdit.Text = "Edit";
            buttonEdit.UseVisualStyleBackColor = true;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(173, 17);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(75, 23);
            buttonDelete.TabIndex = 3;
            buttonDelete.Text = "Delete";
            buttonDelete.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(254, 17);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 4;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonOpen
            // 
            buttonOpen.Location = new Point(335, 17);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(75, 23);
            buttonOpen.TabIndex = 5;
            buttonOpen.Text = "Open";
            buttonOpen.UseVisualStyleBackColor = true;
            buttonOpen.Click += buttonOpen_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonOpen);
            Controls.Add(buttonSave);
            Controls.Add(buttonDelete);
            Controls.Add(buttonEdit);
            Controls.Add(buttonAdd);
            Controls.Add(dataGridViewWords);
            Name = "MainForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridViewWords).EndInit();
            ((System.ComponentModel.ISupportInitialize)inMemoryDatabaseBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewWords;
        private DataGridViewTextBoxColumn semanticGroupsDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn sourcesDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn languagesDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn rootsDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn stemsDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn wordsDataGridViewTextBoxColumn;
        private BindingSource inMemoryDatabaseBindingSource;
        private Button buttonAdd;
        private Button buttonEdit;
        private Button buttonDelete;
        private Button buttonSave;
        private Button buttonOpen;
    }
}
