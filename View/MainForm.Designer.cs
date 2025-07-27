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
            _showAlphaRootsMenuItem = new ToolStripMenuItem();
            _showSemanticMenuItem = new ToolStripMenuItem();
            _showLanguageMenuItem = new ToolStripMenuItem();
            _showSourcesMenuItem = new ToolStripMenuItem();
            _languagesGridView = new DataGridView();
            contextMenuStripLanguages = new ContextMenuStrip(components);
            addLanguageToolStripMenuItem = new ToolStripMenuItem();
            editLanguageToolStripMenuItem = new ToolStripMenuItem();
            removeLanguageToolStripMenuItem = new ToolStripMenuItem();
            _sourcesGridView = new DataGridView();
            contextMenuStripSources = new ContextMenuStrip(components);
            addSourceToolStripMenuItem = new ToolStripMenuItem();
            editSourceToolStripMenuItem = new ToolStripMenuItem();
            removeSourceToolStripMenuItem = new ToolStripMenuItem();
            _semanticTreeView = new TreeView();
            _semanticMenu = new ContextMenuStrip(components);
            addRootGroupToolStripMenuItem = new ToolStripMenuItem();
            _semanticNodeMenu = new ContextMenuStrip(components);
            addChildGroupToolStripMenuItem = new ToolStripMenuItem();
            editGroupToolStripMenuItem = new ToolStripMenuItem();
            moveGroupToolStripMenuItem = new ToolStripMenuItem();
            removeGroupToolStripMenuItem = new ToolStripMenuItem();
            _alphaRootsGridView = new DataGridView();
            _rootGridMenu = new ContextMenuStrip(components);
            _addRootMenuItem = new ToolStripMenuItem();
            _rootRowMenu = new ContextMenuStrip(components);
            _addRootRowMenuItem = new ToolStripMenuItem();
            _editRootRowMenuItem = new ToolStripMenuItem();
            _deleteRootRowMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)inMemoryDatabaseBindingSource).BeginInit();
            mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_languagesGridView).BeginInit();
            contextMenuStripLanguages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_sourcesGridView).BeginInit();
            contextMenuStripSources.SuspendLayout();
            _semanticMenu.SuspendLayout();
            _semanticNodeMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_alphaRootsGridView).BeginInit();
            _rootGridMenu.SuspendLayout();
            _rootRowMenu.SuspendLayout();
            SuspendLayout();
            // 
            // inMemoryDatabaseBindingSource
            // 
            inMemoryDatabaseBindingSource.DataSource = typeof(Database);
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
            repositoriesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _showAlphaRootsMenuItem, _showSemanticMenuItem, _showLanguageMenuItem, _showSourcesMenuItem });
            repositoriesToolStripMenuItem.Name = "repositoriesToolStripMenuItem";
            repositoriesToolStripMenuItem.Size = new Size(83, 20);
            repositoriesToolStripMenuItem.Text = "Repositories";
            // 
            // _showAlphaRootsMenuItem
            // 
            _showAlphaRootsMenuItem.Name = "_showAlphaRootsMenuItem";
            _showAlphaRootsMenuItem.Size = new Size(169, 22);
            _showAlphaRootsMenuItem.Text = "Roots by alphabet";
            _showAlphaRootsMenuItem.Click += SelectAlphaRoots;
            // 
            // _showSemanticMenuItem
            // 
            _showSemanticMenuItem.Name = "_showSemanticMenuItem";
            _showSemanticMenuItem.Size = new Size(169, 22);
            _showSemanticMenuItem.Text = "Semantic Groups";
            _showSemanticMenuItem.Click += SelectSemanticGroups;
            // 
            // _showLanguageMenuItem
            // 
            _showLanguageMenuItem.Name = "_showLanguageMenuItem";
            _showLanguageMenuItem.Size = new Size(169, 22);
            _showLanguageMenuItem.Text = "Languages";
            _showLanguageMenuItem.Click += SelectLanguages;
            // 
            // _showSourcesMenuItem
            // 
            _showSourcesMenuItem.Name = "_showSourcesMenuItem";
            _showSourcesMenuItem.Size = new Size(169, 22);
            _showSourcesMenuItem.Text = "Sources";
            _showSourcesMenuItem.Click += SelectSources;
            // 
            // _languagesGridView
            // 
            _languagesGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _languagesGridView.ContextMenuStrip = contextMenuStripLanguages;
            _languagesGridView.Dock = DockStyle.Fill;
            _languagesGridView.Location = new Point(0, 24);
            _languagesGridView.MultiSelect = false;
            _languagesGridView.Name = "_languagesGridView";
            _languagesGridView.ReadOnly = true;
            _languagesGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _languagesGridView.Size = new Size(800, 426);
            _languagesGridView.TabIndex = 7;
            _languagesGridView.Visible = false;
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
            removeLanguageToolStripMenuItem.Click += DeleteLanguage;
            // 
            // _sourcesGridView
            // 
            _sourcesGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _sourcesGridView.ContextMenuStrip = contextMenuStripSources;
            _sourcesGridView.Dock = DockStyle.Fill;
            _sourcesGridView.Location = new Point(0, 24);
            _sourcesGridView.Name = "_sourcesGridView";
            _sourcesGridView.Size = new Size(800, 426);
            _sourcesGridView.TabIndex = 8;
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
            removeSourceToolStripMenuItem.Click += DeleteSource;
            // 
            // _semanticTreeView
            // 
            _semanticTreeView.ContextMenuStrip = _semanticMenu;
            _semanticTreeView.Dock = DockStyle.Fill;
            _semanticTreeView.Location = new Point(0, 24);
            _semanticTreeView.Name = "_semanticTreeView";
            _semanticTreeView.Size = new Size(800, 426);
            _semanticTreeView.TabIndex = 9;
            // 
            // _semanticMenu
            // 
            _semanticMenu.Items.AddRange(new ToolStripItem[] { addRootGroupToolStripMenuItem });
            _semanticMenu.Name = "contextMenuStripSemanticGroups";
            _semanticMenu.Size = new Size(161, 26);
            // 
            // addRootGroupToolStripMenuItem
            // 
            addRootGroupToolStripMenuItem.Name = "addRootGroupToolStripMenuItem";
            addRootGroupToolStripMenuItem.Size = new Size(160, 22);
            addRootGroupToolStripMenuItem.Text = "Add Root Group";
            addRootGroupToolStripMenuItem.Click += AddHeadGroup;
            // 
            // _semanticNodeMenu
            // 
            _semanticNodeMenu.Items.AddRange(new ToolStripItem[] { addChildGroupToolStripMenuItem, editGroupToolStripMenuItem, moveGroupToolStripMenuItem, removeGroupToolStripMenuItem });
            _semanticNodeMenu.Name = "contextMenuStripSemanticGroups";
            _semanticNodeMenu.Size = new Size(164, 92);
            // 
            // addChildGroupToolStripMenuItem
            // 
            addChildGroupToolStripMenuItem.Name = "addChildGroupToolStripMenuItem";
            addChildGroupToolStripMenuItem.Size = new Size(163, 22);
            addChildGroupToolStripMenuItem.Text = "Add Child Group";
            addChildGroupToolStripMenuItem.Click += AddChildGroup;
            // 
            // editGroupToolStripMenuItem
            // 
            editGroupToolStripMenuItem.Name = "editGroupToolStripMenuItem";
            editGroupToolStripMenuItem.Size = new Size(163, 22);
            editGroupToolStripMenuItem.Text = "Edit Group";
            editGroupToolStripMenuItem.Click += EditGroup;
            // 
            // moveGroupToolStripMenuItem
            // 
            moveGroupToolStripMenuItem.Name = "moveGroupToolStripMenuItem";
            moveGroupToolStripMenuItem.Size = new Size(163, 22);
            moveGroupToolStripMenuItem.Text = "Move Group";
            moveGroupToolStripMenuItem.Click += MoveGroup;
            // 
            // removeGroupToolStripMenuItem
            // 
            removeGroupToolStripMenuItem.Name = "removeGroupToolStripMenuItem";
            removeGroupToolStripMenuItem.Size = new Size(163, 22);
            removeGroupToolStripMenuItem.Text = "Remove Group";
            removeGroupToolStripMenuItem.Click += DeleteGroup;
            // 
            // _alphaRootsGridView
            // 
            _alphaRootsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _alphaRootsGridView.Dock = DockStyle.Fill;
            _alphaRootsGridView.Location = new Point(0, 24);
            _alphaRootsGridView.Name = "_alphaRootsGridView";
            _alphaRootsGridView.Size = new Size(800, 426);
            _alphaRootsGridView.TabIndex = 10;
            // 
            // _rootGridMenu
            // 
            _rootGridMenu.Items.AddRange(new ToolStripItem[] { _addRootMenuItem });
            _rootGridMenu.Name = "_rootGridMenu";
            _rootGridMenu.Size = new Size(125, 26);
            // 
            // addRootToolStripMenuItem
            // 
            _addRootMenuItem.Name = "addRootToolStripMenuItem";
            _addRootMenuItem.Size = new Size(124, 22);
            _addRootMenuItem.Text = "Add Root";
            _addRootMenuItem.Click += AddRoot;
            // 
            // _rootRowMenu
            // 
            _rootRowMenu.Items.AddRange(new ToolStripItem[] { _addRootRowMenuItem, _editRootRowMenuItem, _deleteRootRowMenuItem });
            _rootRowMenu.Name = "_rootRowMenu";
            _rootRowMenu.Size = new Size(181, 92);
            // 
            // addRootToolStripMenuItem1
            // 
            _addRootRowMenuItem.Name = "addRootToolStripMenuItem1";
            _addRootRowMenuItem.Size = new Size(180, 22);
            _addRootRowMenuItem.Text = "Add Root";
            _addRootRowMenuItem.Click += AddRoot;
            // 
            // editRootToolStripMenuItem
            // 
            _editRootRowMenuItem.Name = "editRootToolStripMenuItem";
            _editRootRowMenuItem.Size = new Size(180, 22);
            _editRootRowMenuItem.Text = "Edit Root";
            _editRootRowMenuItem.Click += EditRoot;
            // 
            // deleteRootToolStripMenuItem
            // 
            _deleteRootRowMenuItem.Name = "deleteRootToolStripMenuItem";
            _deleteRootRowMenuItem.Size = new Size(180, 22);
            _deleteRootRowMenuItem.Text = "Delete Root";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(_alphaRootsGridView);
            Controls.Add(_semanticTreeView);
            Controls.Add(_sourcesGridView);
            Controls.Add(_languagesGridView);
            Controls.Add(mainMenu);
            MainMenuStrip = mainMenu;
            Name = "MainForm";
            Text = "Comparatist";
            ((System.ComponentModel.ISupportInitialize)inMemoryDatabaseBindingSource).EndInit();
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_languagesGridView).EndInit();
            contextMenuStripLanguages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_sourcesGridView).EndInit();
            contextMenuStripSources.ResumeLayout(false);
            _semanticMenu.ResumeLayout(false);
            _semanticNodeMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_alphaRootsGridView).EndInit();
            _rootGridMenu.ResumeLayout(false);
            _rootRowMenu.ResumeLayout(false);
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
        private ToolStripMenuItem _showAlphaRootsMenuItem;
        private ToolStripMenuItem _showSemanticMenuItem;
        private ToolStripMenuItem _showLanguageMenuItem;
        private ToolStripMenuItem _showSourcesMenuItem;
        private DataGridView _languagesGridView;
        private ContextMenuStrip contextMenuStripLanguages;
        private ToolStripMenuItem addLanguageToolStripMenuItem;
        private ToolStripMenuItem editLanguageToolStripMenuItem;
        private ToolStripMenuItem removeLanguageToolStripMenuItem;
        private DataGridView _sourcesGridView;
        private ContextMenuStrip contextMenuStripSources;
        private ToolStripMenuItem addSourceToolStripMenuItem;
        private ToolStripMenuItem editSourceToolStripMenuItem;
        private ToolStripMenuItem removeSourceToolStripMenuItem;
        private TreeView _semanticTreeView;
        private ContextMenuStrip _semanticNodeMenu;
        private ToolStripMenuItem addChildGroupToolStripMenuItem;
        private ToolStripMenuItem editGroupToolStripMenuItem;
        private ToolStripMenuItem moveGroupToolStripMenuItem;
        private ToolStripMenuItem removeGroupToolStripMenuItem;
        private ContextMenuStrip _semanticMenu;
        private ToolStripMenuItem addRootGroupToolStripMenuItem;
        private DataGridView _alphaRootsGridView;
        private ContextMenuStrip _rootGridMenu;
        private ToolStripMenuItem _addRootMenuItem;
        private ContextMenuStrip _rootRowMenu;
        private ToolStripMenuItem _addRootRowMenuItem;
        private ToolStripMenuItem _editRootRowMenuItem;
        private ToolStripMenuItem _deleteRootRowMenuItem;
    }
}
