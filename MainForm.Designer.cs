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
            inMemoryDatabaseBindingSource = new BindingSource(components);
            mainMenu = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem1 = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            repositoriesToolStripMenuItem = new ToolStripMenuItem();
            rootsToolStripMenuItem = new ToolStripMenuItem();
            semanticGroupsToolStripMenuItem = new ToolStripMenuItem();
            languagesToolStripMenuItem = new ToolStripMenuItem();
            sourcesToolStripMenuItem = new ToolStripMenuItem();
            dataGridViewLanguages = new DataGridView();
            contextMenuStripLanguages = new ContextMenuStrip(components);
            addLanguageToolStripMenuItem = new ToolStripMenuItem();
            editLanguageToolStripMenuItem = new ToolStripMenuItem();
            removeLanguageToolStripMenuItem = new ToolStripMenuItem();
            dataGridViewSources = new DataGridView();
            contextMenuStripSources = new ContextMenuStrip(components);
            addSourceToolStripMenuItem = new ToolStripMenuItem();
            editSourceToolStripMenuItem = new ToolStripMenuItem();
            removeSourceToolStripMenuItem = new ToolStripMenuItem();
            treeViewSemanticGroups = new TreeView();
            contextMenuStripSemanticGroups = new ContextMenuStrip(components);
            addGroupToolStripMenuItem = new ToolStripMenuItem();
            editGroupToolStripMenuItem = new ToolStripMenuItem();
            moveGroupToolStripMenuItem = new ToolStripMenuItem();
            removeGroupToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)inMemoryDatabaseBindingSource).BeginInit();
            mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewLanguages).BeginInit();
            contextMenuStripLanguages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSources).BeginInit();
            contextMenuStripSources.SuspendLayout();
            contextMenuStripSemanticGroups.SuspendLayout();
            SuspendLayout();
            // 
            // inMemoryDatabaseBindingSource
            // 
            inMemoryDatabaseBindingSource.DataSource = typeof(InMemoryDatabase);
            // 
            // mainMenu
            // 
            mainMenu.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, repositoriesToolStripMenuItem });
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
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(123, 22);
            openToolStripMenuItem.Text = "Open...";
            openToolStripMenuItem.Click += Open;
            // 
            // saveAsToolStripMenuItem1
            // 
            saveAsToolStripMenuItem1.Name = "saveAsToolStripMenuItem1";
            saveAsToolStripMenuItem1.Size = new Size(123, 22);
            saveAsToolStripMenuItem1.Text = "Save As...";
            saveAsToolStripMenuItem1.Click += SaveAs;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(123, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += Save;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(123, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += Exit;
            // 
            // repositoriesToolStripMenuItem
            // 
            repositoriesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { rootsToolStripMenuItem, semanticGroupsToolStripMenuItem, languagesToolStripMenuItem, sourcesToolStripMenuItem });
            repositoriesToolStripMenuItem.Name = "repositoriesToolStripMenuItem";
            repositoriesToolStripMenuItem.Size = new Size(83, 20);
            repositoriesToolStripMenuItem.Text = "Repositories";
            // 
            // rootsToolStripMenuItem
            // 
            rootsToolStripMenuItem.Name = "rootsToolStripMenuItem";
            rootsToolStripMenuItem.Size = new Size(164, 22);
            rootsToolStripMenuItem.Text = "Roots";
            rootsToolStripMenuItem.Click += SelectRootsRepo;
            // 
            // semanticGroupsToolStripMenuItem
            // 
            semanticGroupsToolStripMenuItem.Name = "semanticGroupsToolStripMenuItem";
            semanticGroupsToolStripMenuItem.Size = new Size(164, 22);
            semanticGroupsToolStripMenuItem.Text = "Semantic Groups";
            semanticGroupsToolStripMenuItem.Click += SelectSemanticGroupsRepo;
            // 
            // languagesToolStripMenuItem
            // 
            languagesToolStripMenuItem.Name = "languagesToolStripMenuItem";
            languagesToolStripMenuItem.Size = new Size(164, 22);
            languagesToolStripMenuItem.Text = "Languages";
            languagesToolStripMenuItem.Click += SelectLanguagesRepo;
            // 
            // sourcesToolStripMenuItem
            // 
            sourcesToolStripMenuItem.Name = "sourcesToolStripMenuItem";
            sourcesToolStripMenuItem.Size = new Size(164, 22);
            sourcesToolStripMenuItem.Text = "Sources";
            sourcesToolStripMenuItem.Click += SelectSourcesRepo;
            // 
            // dataGridViewLanguages
            // 
            dataGridViewLanguages.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewLanguages.ContextMenuStrip = contextMenuStripLanguages;
            dataGridViewLanguages.Dock = DockStyle.Fill;
            dataGridViewLanguages.Location = new Point(0, 24);
            dataGridViewLanguages.MultiSelect = false;
            dataGridViewLanguages.Name = "dataGridViewLanguages";
            dataGridViewLanguages.ReadOnly = true;
            dataGridViewLanguages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewLanguages.Size = new Size(800, 426);
            dataGridViewLanguages.TabIndex = 7;
            dataGridViewLanguages.Visible = false;
            // 
            // contextMenuStripLanguages
            // 
            contextMenuStripLanguages.Items.AddRange(new ToolStripItem[] { addLanguageToolStripMenuItem, editLanguageToolStripMenuItem, removeLanguageToolStripMenuItem });
            contextMenuStripLanguages.Name = "Languages Menu";
            contextMenuStripLanguages.Size = new Size(173, 70);
            // 
            // addLanguageToolStripMenuItem
            // 
            addLanguageToolStripMenuItem.Name = "addLanguageToolStripMenuItem";
            addLanguageToolStripMenuItem.Size = new Size(172, 22);
            addLanguageToolStripMenuItem.Text = "Add Language";
            addLanguageToolStripMenuItem.Click += AddLanguage;
            // 
            // editLanguageToolStripMenuItem
            // 
            editLanguageToolStripMenuItem.Name = "editLanguageToolStripMenuItem";
            editLanguageToolStripMenuItem.Size = new Size(172, 22);
            editLanguageToolStripMenuItem.Text = "Edit Language";
            editLanguageToolStripMenuItem.Click += EditLanguage;
            // 
            // removeLanguageToolStripMenuItem
            // 
            removeLanguageToolStripMenuItem.Name = "removeLanguageToolStripMenuItem";
            removeLanguageToolStripMenuItem.Size = new Size(172, 22);
            removeLanguageToolStripMenuItem.Text = "Remove Language";
            removeLanguageToolStripMenuItem.Click += RemoveLanguage;
            // 
            // dataGridViewSources
            // 
            dataGridViewSources.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSources.ContextMenuStrip = contextMenuStripSources;
            dataGridViewSources.Dock = DockStyle.Fill;
            dataGridViewSources.Location = new Point(0, 24);
            dataGridViewSources.Name = "dataGridViewSources";
            dataGridViewSources.Size = new Size(800, 426);
            dataGridViewSources.TabIndex = 8;
            // 
            // contextMenuStripSources
            // 
            contextMenuStripSources.Items.AddRange(new ToolStripItem[] { addSourceToolStripMenuItem, editSourceToolStripMenuItem, removeSourceToolStripMenuItem });
            contextMenuStripSources.Name = "contextMenuStripSources";
            contextMenuStripSources.Size = new Size(157, 70);
            // 
            // addSourceToolStripMenuItem
            // 
            addSourceToolStripMenuItem.Name = "addSourceToolStripMenuItem";
            addSourceToolStripMenuItem.Size = new Size(156, 22);
            addSourceToolStripMenuItem.Text = "Add Source";
            addSourceToolStripMenuItem.Click += AddSource;
            // 
            // editSourceToolStripMenuItem
            // 
            editSourceToolStripMenuItem.Name = "editSourceToolStripMenuItem";
            editSourceToolStripMenuItem.Size = new Size(156, 22);
            editSourceToolStripMenuItem.Text = "Edit Source";
            editSourceToolStripMenuItem.Click += EditSource;
            // 
            // removeSourceToolStripMenuItem
            // 
            removeSourceToolStripMenuItem.Name = "removeSourceToolStripMenuItem";
            removeSourceToolStripMenuItem.Size = new Size(156, 22);
            removeSourceToolStripMenuItem.Text = "Remove Source";
            removeSourceToolStripMenuItem.Click += RemoveSource;
            // 
            // treeViewSemanticGroups
            // 
            treeViewSemanticGroups.ContextMenuStrip = contextMenuStripSemanticGroups;
            treeViewSemanticGroups.Dock = DockStyle.Fill;
            treeViewSemanticGroups.Location = new Point(0, 24);
            treeViewSemanticGroups.Name = "treeViewSemanticGroups";
            treeViewSemanticGroups.Size = new Size(800, 426);
            treeViewSemanticGroups.TabIndex = 9;
            // 
            // contextMenuStripSemanticGroups
            // 
            contextMenuStripSemanticGroups.Items.AddRange(new ToolStripItem[] { addGroupToolStripMenuItem, editGroupToolStripMenuItem, moveGroupToolStripMenuItem, removeGroupToolStripMenuItem });
            contextMenuStripSemanticGroups.Name = "contextMenuStripSemanticGroups";
            contextMenuStripSemanticGroups.Size = new Size(154, 92);
            // 
            // addGroupToolStripMenuItem
            // 
            addGroupToolStripMenuItem.Name = "addGroupToolStripMenuItem";
            addGroupToolStripMenuItem.Size = new Size(153, 22);
            addGroupToolStripMenuItem.Text = "Add Group";
            addGroupToolStripMenuItem.Click += AddGroup;
            // 
            // editGroupToolStripMenuItem
            // 
            editGroupToolStripMenuItem.Name = "editGroupToolStripMenuItem";
            editGroupToolStripMenuItem.Size = new Size(153, 22);
            editGroupToolStripMenuItem.Text = "Edit Group";
            editGroupToolStripMenuItem.Click += EditGroup;
            // 
            // moveGroupToolStripMenuItem
            // 
            moveGroupToolStripMenuItem.Name = "moveGroupToolStripMenuItem";
            moveGroupToolStripMenuItem.Size = new Size(153, 22);
            moveGroupToolStripMenuItem.Text = "Move Group";
            moveGroupToolStripMenuItem.Click += moveGroupToolStripMenuItem_Click;
            // 
            // removeGroupToolStripMenuItem
            // 
            removeGroupToolStripMenuItem.Name = "removeGroupToolStripMenuItem";
            removeGroupToolStripMenuItem.Size = new Size(153, 22);
            removeGroupToolStripMenuItem.Text = "Remove Group";
            removeGroupToolStripMenuItem.Click += RemoveGroup;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(treeViewSemanticGroups);
            Controls.Add(dataGridViewSources);
            Controls.Add(dataGridViewLanguages);
            Controls.Add(mainMenu);
            MainMenuStrip = mainMenu;
            Name = "MainForm";
            Text = "Comparatist";
            ((System.ComponentModel.ISupportInitialize)inMemoryDatabaseBindingSource).EndInit();
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewLanguages).EndInit();
            contextMenuStripLanguages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewSources).EndInit();
            contextMenuStripSources.ResumeLayout(false);
            contextMenuStripSemanticGroups.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private BindingSource inMemoryDatabaseBindingSource;
        private MenuStrip mainMenu;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem repositoriesToolStripMenuItem;
        private ToolStripMenuItem rootsToolStripMenuItem;
        private ToolStripMenuItem semanticGroupsToolStripMenuItem;
        private ToolStripMenuItem languagesToolStripMenuItem;
        private ToolStripMenuItem sourcesToolStripMenuItem;
        private DataGridView dataGridViewLanguages;
        private ContextMenuStrip contextMenuStripLanguages;
        private ToolStripMenuItem addLanguageToolStripMenuItem;
        private ToolStripMenuItem editLanguageToolStripMenuItem;
        private ToolStripMenuItem removeLanguageToolStripMenuItem;
        private DataGridView dataGridViewSources;
        private ContextMenuStrip contextMenuStripSources;
        private ToolStripMenuItem addSourceToolStripMenuItem;
        private ToolStripMenuItem editSourceToolStripMenuItem;
        private ToolStripMenuItem removeSourceToolStripMenuItem;
        private TreeView treeViewSemanticGroups;
        private ContextMenuStrip contextMenuStripSemanticGroups;
        private ToolStripMenuItem addGroupToolStripMenuItem;
        private ToolStripMenuItem editGroupToolStripMenuItem;
        private ToolStripMenuItem moveGroupToolStripMenuItem;
        private ToolStripMenuItem removeGroupToolStripMenuItem;
    }
}
