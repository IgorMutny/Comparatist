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
            mainMenu = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem1 = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)dataGridViewWords).BeginInit();
            ((System.ComponentModel.ISupportInitialize)inMemoryDatabaseBindingSource).BeginInit();
            mainMenu.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewWords
            // 
            dataGridViewWords.AutoGenerateColumns = false;
            dataGridViewWords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewWords.Columns.AddRange(new DataGridViewColumn[] { semanticGroupsDataGridViewTextBoxColumn, sourcesDataGridViewTextBoxColumn, languagesDataGridViewTextBoxColumn, rootsDataGridViewTextBoxColumn, stemsDataGridViewTextBoxColumn, wordsDataGridViewTextBoxColumn });
            dataGridViewWords.DataSource = inMemoryDatabaseBindingSource;
            dataGridViewWords.Dock = DockStyle.Fill;
            dataGridViewWords.Location = new Point(0, 24);
            dataGridViewWords.Name = "dataGridViewWords";
            dataGridViewWords.Size = new Size(800, 426);
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
            // mainMenu
            // 
            mainMenu.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            mainMenu.Location = new Point(0, 0);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new Size(800, 24);
            mainMenu.TabIndex = 6;
            mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveAsToolStripMenuItem1, saveToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            fileToolStripMenuItem.Click += fileToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(180, 22);
            openToolStripMenuItem.Text = "Open...";
            openToolStripMenuItem.Click += Open;
            // 
            // saveAsToolStripMenuItem1
            // 
            saveAsToolStripMenuItem1.Name = "saveAsToolStripMenuItem1";
            saveAsToolStripMenuItem1.Size = new Size(180, 22);
            saveAsToolStripMenuItem1.Text = "Save As...";
            saveAsToolStripMenuItem1.Click += SaveAs;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(180, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += Save;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(180, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += Exit;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridViewWords);
            Controls.Add(mainMenu);
            MainMenuStrip = mainMenu;
            Name = "MainForm";
            Text = "Comparatist";
            ((System.ComponentModel.ISupportInitialize)dataGridViewWords).EndInit();
            ((System.ComponentModel.ISupportInitialize)inMemoryDatabaseBindingSource).EndInit();
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private MenuStrip mainMenu;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}
