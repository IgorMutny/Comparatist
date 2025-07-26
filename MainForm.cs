namespace Comparatist
{
    public partial class MainForm : Form
    {
        private const string Extension = ".comparatist";

        private InMemoryDatabase _db = new();
        private string _filePath = string.Empty;
        private ContentHolderTypes _currentContentHolder = ContentHolderTypes.Roots;

        public MainForm()
        {
            _db = new();
            InitializeComponent();
            SetupLanguagesGrid();
            SetupSourcesGrid();
        }

        #region FILE_OPERATIONS
        private void Open(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = $"Comparatist DataBase files (*{Extension})|*{Extension}";
                dialog.Title = "Select Comparatist DataBase file";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _filePath = dialog.FileName;
                    _db.Load(_filePath);
                    RefreshContent();
                }
            }
        }

        private void SaveAs(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = $"Comparatist DataBase files (*{Extension})|*{Extension}";
                dialog.Title = "Save Comparatist DataBase as...";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string path = dialog.FileName;
                    _db.Save(path);
                    _filePath = path;
                }
            }
        }

        private void Save(object sender, EventArgs e)
        {
            if (_db == null)
                MessageBox.Show("Nothing to save!");
            else if (string.IsNullOrEmpty(_filePath))
                SaveAs(sender, e);
            else
                _db.Save(_filePath);

        }

        private void Exit(object sender, EventArgs e)
        {
            if (_db != null)
                Save(sender, EventArgs.Empty);

            Close();
        }
        #endregion FILE_OPERATIONS

        #region COMMON_CONTENT
        private void OnDataGridViewRightClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (sender is not DataGridView gridView)
                return;

            if (e.RowIndex >= 0 && e.Button == MouseButtons.Right)
            {
                gridView.ClearSelection();
                gridView.Rows[e.RowIndex].Selected = true;
                gridView.CurrentCell = gridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        private void RefreshContent()
        {
            ShowActiveContentHolder();
            SetCheckedRepositoryMenu();

            switch (_currentContentHolder)
            {
                case ContentHolderTypes.Roots: RefreshRoots(); break;
                case ContentHolderTypes.SemanticGroups: RefreshSemanticGroups(); break;
                case ContentHolderTypes.Languages: RefreshLanguages(); break;
                case ContentHolderTypes.Sources: RefreshSources(); break;
            }
        }

        private void SetCheckedRepositoryMenu()
        {
            rootsToolStripMenuItem.Checked = _currentContentHolder == ContentHolderTypes.Roots;
            semanticGroupsToolStripMenuItem.Checked = _currentContentHolder == ContentHolderTypes.SemanticGroups;
            languagesToolStripMenuItem.Checked = _currentContentHolder == ContentHolderTypes.Languages;
            sourcesToolStripMenuItem.Checked = _currentContentHolder == ContentHolderTypes.Sources;
        }

        private void ShowActiveContentHolder()
        {
            dataGridViewLanguages.Visible = _currentContentHolder == ContentHolderTypes.Languages;
            //dataGridViewWords.Visible = repo == "Words";
            //dataGridViewStems.Visible = repo == "Stems";
            //dataGridViewRoots.Visible = repo == "Roots";
            treeViewSemanticGroups.Visible = _currentContentHolder == ContentHolderTypes.SemanticGroups;
            dataGridViewSources.Visible = _currentContentHolder == ContentHolderTypes.Sources;
        }

        private void SelectRootsRepo(object sender, EventArgs e)
        {
            _currentContentHolder = ContentHolderTypes.Roots;
            RefreshContent();
        }

        private void SelectSemanticGroupsRepo(object sender, EventArgs e)
        {
            _currentContentHolder = ContentHolderTypes.SemanticGroups;
            RefreshContent();
        }

        private void SelectLanguagesRepo(object sender, EventArgs e)
        {
            _currentContentHolder = ContentHolderTypes.Languages;
            RefreshContent();
        }

        private void SelectSourcesRepo(object sender, EventArgs e)
        {
            _currentContentHolder = ContentHolderTypes.Sources;
            RefreshContent();
        }

        #endregion COMMON_CONTENT

        #region SOURCES
        private void SetupSourcesGrid()
        {
            dataGridViewSources.Columns.Clear();

            var valueColumn = new DataGridViewTextBoxColumn
            {
                Name = "Value",
                HeaderText = "Source",
                DataPropertyName = "Value",
                Width = 200
            };

            dataGridViewSources.Columns.AddRange(valueColumn);
            dataGridViewSources.AutoGenerateColumns = false;
            dataGridViewSources.CellMouseDown += OnDataGridViewRightClick;
            dataGridViewSources.ReadOnly = true;
            dataGridViewSources.Visible = false;
        }

        private void RefreshSources()
        {
            dataGridViewSources.Rows.Clear();

            foreach (var e in _db.Sources.GetAll())
            {
                int rowIndex = dataGridViewSources.Rows.Add(e.Record.Value);
                dataGridViewSources.Rows[rowIndex].Tag = e.Id;
            }
        }

        private void AddSource(object sender, EventArgs e)
        {
            string? input = InputDialog.Show("Add source", string.Empty);
            if (!string.IsNullOrEmpty(input))
            {
                _db.Sources.Add(new Source { Value = input });
                RefreshContent();
            }
        }

        private void EditSource(object sender, EventArgs e)
        {
            if (dataGridViewSources.SelectedRows.Count == 0)
                return;

            var row = dataGridViewSources.SelectedRows[0];

            if (row.Tag != null && row.Tag is Guid id)
            {
                var source = _db.Sources.GetById(id);

                if (source != null)
                {
                    var input = InputDialog.Show("Edit source", $"New name for {source.Value}");

                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        var item = new Source() { Value = input };
                        _db.Sources.Update(id, item);
                        RefreshContent();
                    }
                }
            }
        }

        private void RemoveSource(object sender, EventArgs e)
        {
            if (dataGridViewSources.SelectedRows.Count == 0)
                return;

            var row = dataGridViewSources.SelectedRows[0];

            if (row.Tag != null && row.Tag is Guid id)
            {
                var source = _db.Sources.GetById(id);

                if (source != null)
                {
                    var result = MessageBox.Show($"Remove {source.Value}", $"Remove source?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        _db.Sources.Delete(id);
                        RefreshContent();
                    }
                }
            }
        }
        #endregion SOURCES

        #region LANGUAGES
        private void SetupLanguagesGrid()
        {
            dataGridViewLanguages.Columns.Clear();

            var valueColumn = new DataGridViewTextBoxColumn
            {
                Name = "Value",
                HeaderText = "Language",
                DataPropertyName = "Value",
                Width = 200
            };

            dataGridViewLanguages.Columns.AddRange(valueColumn);
            dataGridViewLanguages.AutoGenerateColumns = false;
            dataGridViewLanguages.CellMouseDown += OnDataGridViewRightClick;
            dataGridViewLanguages.ReadOnly = true;
            dataGridViewLanguages.Visible = false;
        }

        private void RefreshLanguages()
        {
            dataGridViewLanguages.Rows.Clear();

            foreach (var e in _db.Languages.GetAll())
            {
                int rowIndex = dataGridViewLanguages.Rows.Add(e.Record.Value);
                dataGridViewLanguages.Rows[rowIndex].Tag = e.Id;
            }
        }

        private void AddLanguage(object sender, EventArgs e)
        {
            string? input = InputDialog.Show("Add language", string.Empty);
            if (!string.IsNullOrEmpty(input))
            {
                _db.Languages.Add(new Language { Value = input });
                RefreshContent();
            }
        }

        private void EditLanguage(object sender, EventArgs e)
        {
            if (dataGridViewLanguages.SelectedRows.Count == 0)
                return;

            var row = dataGridViewLanguages.SelectedRows[0];

            if (row.Tag != null && row.Tag is Guid id)
            {
                var language = _db.Languages.GetById(id);

                if (language != null)
                {
                    var input = InputDialog.Show("Edit language", $"New name for {language.Value}");
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        var item = new Language() { Value = input };
                        _db.Languages.Update(id, item);
                        RefreshContent();
                    }
                }
            }
        }

        private void RemoveLanguage(object sender, EventArgs e)
        {
            if (dataGridViewLanguages.SelectedRows.Count == 0)
                return;

            var row = dataGridViewLanguages.SelectedRows[0];

            if (row.Tag != null && row.Tag is Guid id)
            {
                var language = _db.Languages.GetById(id);

                if (language != null)
                {
                    var result = MessageBox.Show($"Remove {language.Value}", $"Remove language?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        _db.Languages.Delete(id);
                        RefreshContent();
                    }
                }
            }
        }
        #endregion LANGUAGES

        #region SEMANTIC_GROUPS
        private void RefreshSemanticGroups()
        {
            treeViewSemanticGroups.BeginUpdate();
            treeViewSemanticGroups.Nodes.Clear();

            var groupsToNodeMap = new Dictionary<SemanticGroup, TreeNode>();

            foreach (var guidedRecord in _db.SemanticGroups.GetAll())
            {
                var node = new TreeNode(guidedRecord.Record.Value) { Tag = guidedRecord.Id };
                groupsToNodeMap[guidedRecord.Record] = node;
            }

            foreach (var pair in groupsToNodeMap)
            {
                var node = pair.Key;

                if (node.ParentSemanticGroup == null)
                {
                    treeViewSemanticGroups.Nodes.Add(pair.Value);
                }
                else if (groupsToNodeMap.ContainsKey(node.ParentSemanticGroup))
                {
                    var parentNode = groupsToNodeMap[node.ParentSemanticGroup];
                    parentNode.Nodes.Add(node.Value);
                }
            }

            treeViewSemanticGroups.EndUpdate();
            treeViewSemanticGroups.ExpandAll();
        }

        private void AddGroup(object sender, EventArgs e)
        {
            SemanticGroup? parent = null;
            string prompt = string.Empty;

            if (treeViewSemanticGroups.SelectedNode?.Tag is Guid parentId)
            {
                parent = _db.SemanticGroups.GetById(parentId);

                if (parent != null)
                    prompt = $"As descendant of {parent.Value}";
            }

            var input = InputDialog.Show("New semantic group", prompt);
            if (!string.IsNullOrWhiteSpace(input))
            {
                var newGroup = new SemanticGroup
                {
                    Value = input,
                    ParentSemanticGroup = parent
                };
                _db.SemanticGroups.Add(newGroup);
                RefreshSemanticGroups();
            }
        }

        private void EditGroup(object sender, EventArgs e)
        {
            if (treeViewSemanticGroups.SelectedNode?.Tag is Guid id)
            {
                var group = _db.SemanticGroups.GetById(id);

                if (group != null)
                {
                    var input = InputDialog.Show("Edit group", $"New name for {group.Value}");

                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        group.Value = input;
                        _db.SemanticGroups.Update(id, group);
                        RefreshSemanticGroups();
                    }
                }
            }
        }

        private void moveGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void RemoveGroup(object sender, EventArgs e)
        {
            if (treeViewSemanticGroups.SelectedNode?.Tag is Guid id)
            {
                var group = _db.SemanticGroups.GetById(id);
                if (group != null)
                {
                    var result = MessageBox.Show($"Remove {group.Value}", "Remove semantic group?", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        _db.SemanticGroups.Delete(id);
                        RefreshSemanticGroups();
                    }
                }
            }
        }
        #endregion SEMANTIC_GROUPS

        #region ROOTS
        private void RefreshRoots()
        {

        }
        #endregion ROOTS
    }

    public enum ContentHolderTypes
    {
        Roots = 0,
        SemanticGroups = 1,
        Languages = 2,
        Sources = 3,
    }
}
