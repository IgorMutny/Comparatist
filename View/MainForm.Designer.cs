using Comparatist.Core.Infrastructure;

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
            _languagesGridView = new DataGridView();
            contextMenuStripLanguages = new ContextMenuStrip(components);
            addLanguageToolStripMenuItem = new ToolStripMenuItem();
            editLanguageToolStripMenuItem = new ToolStripMenuItem();
            removeLanguageToolStripMenuItem = new ToolStripMenuItem();
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
            addStemToolStripMenuItem = new ToolStripMenuItem();
            _stemRowMenu = new ContextMenuStrip(components);
            addStemToolStripMenuItem1 = new ToolStripMenuItem();
            editStemToolStripMenuItem = new ToolStripMenuItem();
            deleteStemToolStripMenuItem = new ToolStripMenuItem();
            _wordMenu = new ContextMenuStrip(components);
            addWordToolStripMenuItem = new ToolStripMenuItem();
            deleteWordToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)inMemoryDatabaseBindingSource).BeginInit();
            mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_languagesGridView).BeginInit();
            contextMenuStripLanguages.SuspendLayout();
            _semanticMenu.SuspendLayout();
            _semanticNodeMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_alphaRootsGridView).BeginInit();
            _rootGridMenu.SuspendLayout();
            _rootRowMenu.SuspendLayout();
            _stemRowMenu.SuspendLayout();
            _wordMenu.SuspendLayout();
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
            repositoriesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _showAlphaRootsMenuItem, _showSemanticMenuItem, _showLanguageMenuItem });
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
            // _addRootMenuItem
            // 
            _addRootMenuItem.Name = "_addRootMenuItem";
            _addRootMenuItem.Size = new Size(124, 22);
            _addRootMenuItem.Text = "Add Root";
            _addRootMenuItem.Click += AddRoot;
            // 
            // _rootRowMenu
            // 
            _rootRowMenu.Items.AddRange(new ToolStripItem[] { _addRootRowMenuItem, _editRootRowMenuItem, _deleteRootRowMenuItem, addStemToolStripMenuItem });
            _rootRowMenu.Name = "_rootRowMenu";
            _rootRowMenu.Size = new Size(136, 92);
            // 
            // _addRootRowMenuItem
            // 
            _addRootRowMenuItem.Name = "_addRootRowMenuItem";
            _addRootRowMenuItem.Size = new Size(135, 22);
            _addRootRowMenuItem.Text = "Add Root";
            _addRootRowMenuItem.Click += AddRoot;
            // 
            // _editRootRowMenuItem
            // 
            _editRootRowMenuItem.Name = "_editRootRowMenuItem";
            _editRootRowMenuItem.Size = new Size(135, 22);
            _editRootRowMenuItem.Text = "Edit Root";
            _editRootRowMenuItem.Click += EditRoot;
            // 
            // _deleteRootRowMenuItem
            // 
            _deleteRootRowMenuItem.Name = "_deleteRootRowMenuItem";
            _deleteRootRowMenuItem.Size = new Size(135, 22);
            _deleteRootRowMenuItem.Text = "Delete Root";
            _deleteRootRowMenuItem.Click += DeleteRoot;
            // 
            // addStemToolStripMenuItem
            // 
            addStemToolStripMenuItem.Name = "addStemToolStripMenuItem";
            addStemToolStripMenuItem.Size = new Size(135, 22);
            addStemToolStripMenuItem.Text = "Add Stem";
            addStemToolStripMenuItem.Click += AddStem;
            // 
            // _stemRowMenu
            // 
            _stemRowMenu.Items.AddRange(new ToolStripItem[] { addStemToolStripMenuItem1, editStemToolStripMenuItem, deleteStemToolStripMenuItem });
            _stemRowMenu.Name = "_stemRowMenu";
            _stemRowMenu.Size = new Size(138, 70);
            // 
            // addStemToolStripMenuItem1
            // 
            addStemToolStripMenuItem1.Name = "addStemToolStripMenuItem1";
            addStemToolStripMenuItem1.Size = new Size(137, 22);
            addStemToolStripMenuItem1.Text = "Add Stem";
            addStemToolStripMenuItem1.Click += AddStemWithRoot;
            // 
            // editStemToolStripMenuItem
            // 
            editStemToolStripMenuItem.Name = "editStemToolStripMenuItem";
            editStemToolStripMenuItem.Size = new Size(137, 22);
            editStemToolStripMenuItem.Text = "Edit Stem";
            editStemToolStripMenuItem.Click += EditStem;
            // 
            // deleteStemToolStripMenuItem
            // 
            deleteStemToolStripMenuItem.Name = "deleteStemToolStripMenuItem";
            deleteStemToolStripMenuItem.Size = new Size(137, 22);
            deleteStemToolStripMenuItem.Text = "Delete Stem";
            deleteStemToolStripMenuItem.Click += DeleteStem;
            // 
            // _wordMenu
            // 
            _wordMenu.Items.AddRange(new ToolStripItem[] { addWordToolStripMenuItem, deleteWordToolStripMenuItem });
            _wordMenu.Name = "_wordMenu";
            _wordMenu.Size = new Size(181, 70);
            // 
            // addWordToolStripMenuItem
            // 
            addWordToolStripMenuItem.Name = "addWordToolStripMenuItem";
            addWordToolStripMenuItem.Size = new Size(180, 22);
            addWordToolStripMenuItem.Text = "Add / Edit Word";
            addWordToolStripMenuItem.Click += AddOrEditWord;
            // 
            // deleteWordToolStripMenuItem
            // 
            deleteWordToolStripMenuItem.Name = "deleteWordToolStripMenuItem";
            deleteWordToolStripMenuItem.Size = new Size(180, 22);
            deleteWordToolStripMenuItem.Text = "Delete Word";
            deleteWordToolStripMenuItem.Click += DeleteWord;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(_alphaRootsGridView);
            Controls.Add(_semanticTreeView);
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
            _semanticMenu.ResumeLayout(false);
            _semanticNodeMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_alphaRootsGridView).EndInit();
            _rootGridMenu.ResumeLayout(false);
            _rootRowMenu.ResumeLayout(false);
            _stemRowMenu.ResumeLayout(false);
            _wordMenu.ResumeLayout(false);
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
        private DataGridView _languagesGridView;
        private ContextMenuStrip contextMenuStripLanguages;
        private ToolStripMenuItem addLanguageToolStripMenuItem;
        private ToolStripMenuItem editLanguageToolStripMenuItem;
        private ToolStripMenuItem removeLanguageToolStripMenuItem;
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
        private ToolStripMenuItem addStemToolStripMenuItem;
        private ContextMenuStrip _stemRowMenu;
        private ToolStripMenuItem addStemToolStripMenuItem1;
        private ToolStripMenuItem editStemToolStripMenuItem;
        private ToolStripMenuItem deleteStemToolStripMenuItem;
        private ContextMenuStrip _wordMenu;
        private ToolStripMenuItem addWordToolStripMenuItem;
        private ToolStripMenuItem deleteWordToolStripMenuItem;
    }
}
