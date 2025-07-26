namespace Comparatist
{
    public partial class MainForm : Form
    {
        private const string Extension = ".comparatist";

        private Database _db = new();
        private string _filePath = string.Empty;
        private ContentHolderTypes _currentContentHolder = ContentHolderTypes.Roots;
        private bool _isMoveMode = false;
        private TreeNode? _nodeBeingMoved = null;

        public MainForm()
        {
            _db = new();
            InitializeComponent();
            SetupLanguagesGridView();
            SetupSourcesGridView();
            SetupSemanticTreeView();
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
            _languagesGridView.Visible = _currentContentHolder == ContentHolderTypes.Languages;
            //dataGridViewWords.Visible = repo == "Words";
            //dataGridViewStems.Visible = repo == "Stems";
            //dataGridViewRoots.Visible = repo == "Roots";
            _semanticTreeView.Visible = _currentContentHolder == ContentHolderTypes.SemanticGroups;
            _sourcesGridView.Visible = _currentContentHolder == ContentHolderTypes.Sources;
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
        private void SetupSourcesGridView()
        {
            _sourcesGridView.Columns.Clear();

            var valueColumn = new DataGridViewTextBoxColumn
            {
                Name = "Value",
                HeaderText = "Source",
                DataPropertyName = "Value",
                Width = 200
            };

            _sourcesGridView.Columns.AddRange(valueColumn);
            _sourcesGridView.AutoGenerateColumns = false;
            _sourcesGridView.CellMouseDown += OnDataGridViewRightClick;
            _sourcesGridView.ReadOnly = true;
            _sourcesGridView.Visible = false;
        }

        private void RefreshSources()
        {
            _sourcesGridView.Rows.Clear();

            foreach (var e in _db.Sources.GetAll())
            {
                int rowIndex = _sourcesGridView.Rows.Add(e.Value);
                _sourcesGridView.Rows[rowIndex].Tag = e.Id;
            }
        }

        private void AddSource(object sender, EventArgs e)
        {
            string? input = InputBox.Show("Add source", string.Empty);

            if (!string.IsNullOrWhiteSpace(input))
            {
                _db.Sources.Add(new Source { Value = input });
                RefreshContent();
            }
        }

        private void EditSource(object sender, EventArgs e)
        {
            if (_sourcesGridView.SelectedRows.Count == 0)
                return;

            var row = _sourcesGridView.SelectedRows[0];

            if (row.Tag is not Guid id)
                return;

            if (!_db.Sources.TryGet(id, out var source))
                return;

            string? input = InputBox.Show("Edit source", $"New name for {source.Value}");

            if (string.IsNullOrWhiteSpace(input))
                return;

            source.Value = input;
            RefreshContent();
        }

        private void DeleteSource(object sender, EventArgs e)
        {
            if (_sourcesGridView.SelectedRows.Count == 0)
                return;

            var row = _sourcesGridView.SelectedRows[0];

            if (row.Tag is not Guid id)
                return;

            if (!_db.Sources.TryGet(id, out var source))
                return;

            var result = MessageBox.Show(
                $"Remove {source.Value}",
                $"Remove source?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _db.Sources.Delete(id);
                RefreshContent();
            }
        }
        #endregion SOURCES

        #region LANGUAGES
        private void SetupLanguagesGridView()
        {
            _languagesGridView.Columns.Clear();

            var valueColumn = new DataGridViewTextBoxColumn
            {
                Name = "Value",
                HeaderText = "Language",
                DataPropertyName = "Value",
                Width = 200
            };

            _languagesGridView.Columns.AddRange(valueColumn);
            _languagesGridView.AutoGenerateColumns = false;
            _languagesGridView.CellMouseDown += OnDataGridViewRightClick;
            _languagesGridView.ReadOnly = true;
            _languagesGridView.Visible = false;
        }

        private void RefreshLanguages()
        {
            _languagesGridView.Rows.Clear();

            foreach (var e in _db.Languages.GetAll())
            {
                int rowIndex = _languagesGridView.Rows.Add(e.Value);
                _languagesGridView.Rows[rowIndex].Tag = e.Id;
            }
        }

        private void AddLanguage(object sender, EventArgs e)
        {
            string? input = InputBox.Show("Add language", string.Empty);

            if (!string.IsNullOrEmpty(input))
            {
                _db.Languages.Add(new Language { Value = input });
                RefreshContent();
            }
        }

        private void EditLanguage(object sender, EventArgs e)
        {
            if (_languagesGridView.SelectedRows.Count == 0)
                return;

            var row = _languagesGridView.SelectedRows[0];

            if (row.Tag is not Guid id)
                return;

            if (!_db.Languages.TryGet(id, out var language))
                return;

            var input = InputBox.Show("Edit language", $"New name for {language.Value}");

            if (string.IsNullOrWhiteSpace(input))
                return;

            language.Value = input;
            RefreshContent();
        }

        private void DeleteLanguage(object sender, EventArgs e)
        {
            if (_languagesGridView.SelectedRows.Count == 0)
                return;

            var row = _languagesGridView.SelectedRows[0];

            if (row.Tag is not Guid id)
                return;

            if (!_db.Languages.TryGet(id, out var language))
                return;

            var result = MessageBox.Show(
                $"Remove {language.Value}",
                $"Remove language?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _db.Languages.Delete(id);
                RefreshContent();
            }
        }
        #endregion LANGUAGES

        #region SEMANTIC_GROUPS
        private void SetupSemanticTreeView()
        {
            _semanticTreeView.AllowDrop = true;
        }

        private void RefreshSemanticGroups()
        {
            _semanticTreeView.BeginUpdate();
            _semanticTreeView.Nodes.Clear();

            var nodeMap = new Dictionary<SemanticGroup, TreeNode>();

            foreach (var record in _db.SemanticGroups.GetAll())
            {
                var node = new TreeNode(record.Value) { Tag = record.Id };
                nodeMap[record] = node;
            }

            foreach (var pair in nodeMap)
            {
                var record = pair.Key;
                var parentId = record.ParentId;

                if (parentId == null)
                {
                    _semanticTreeView.Nodes.Add(pair.Value);
                }
                else if (_db.SemanticGroups.TryGet(parentId.Value, out var parentGroup)
                    && nodeMap.TryGetValue(parentGroup, out var parentNode))
                {
                    parentNode.Nodes.Add(pair.Value);
                }
            }

            _semanticTreeView.EndUpdate();
            _semanticTreeView.ExpandAll();
        }
        private void AddRootGroup(object sender, EventArgs e)
        {
            var input = InputBox.Show("New semantic group", "Will be a root group");

            if (string.IsNullOrWhiteSpace(input))
                return;

            var newGroup = new SemanticGroup
            {
                Value = input,
                ParentId = null
            };

            _db.SemanticGroups.Add(newGroup);
            RefreshSemanticGroups();
        }

        private void AddChildGroup(object sender, EventArgs e)
        {
            if (_semanticTreeView.SelectedNode?.Tag is not Guid parentId ||
                !_db.SemanticGroups.TryGet(parentId, out var parent))
                return;

            var input = InputBox.Show("New semantic group", $"As child of {parent.Value}");

            if (string.IsNullOrWhiteSpace(input))
                return;

            var newGroup = new SemanticGroup
            {
                Value = input,
                ParentId = parentId
            };

            _db.SemanticGroups.Add(newGroup);
            RefreshSemanticGroups();
        }

        private void EditGroup(object sender, EventArgs e)
        {
            if (_semanticTreeView.SelectedNode?.Tag is not Guid id ||
                !_db.SemanticGroups.TryGet(id, out var group))
                return;

            var input = InputBox.Show("Edit group", $"New name for {group.Value}");

            if (string.IsNullOrWhiteSpace(input))
                return;

            group.Value = input;
            RefreshSemanticGroups();
        }

        private void MoveGroup(object sender, EventArgs e)
        {
            if (_semanticTreeView.SelectedNode == null)
                return;

            _isMoveMode = true;
            _nodeBeingMoved = _semanticTreeView.SelectedNode;
        }

        private void DeleteGroup(object sender, EventArgs e)
        {
            if (_semanticTreeView.SelectedNode?.Tag is not Guid id ||
                !_db.SemanticGroups.TryGet(id, out var group))
                return;

            var result = MessageBox.Show(
                $"Remove {group.Value}",
                "Remove semantic group?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _db.SemanticGroups.Delete(id);
                RefreshSemanticGroups();
            }
        }
        private void OnTreeViewMouseUp(object sender, EventArgs e)
        {
            var mouseEvent = (MouseEventArgs)e;

            if (mouseEvent.Button != MouseButtons.Right)
                return;

            TreeNode clickedNode = _semanticTreeView.GetNodeAt(mouseEvent.X, mouseEvent.Y);

            if (clickedNode == null)
            {
                _semanticTreeView.SelectedNode = null;
                _semanticMenu.Show(_semanticTreeView, mouseEvent.Location);
            }
            else
            {
                _semanticTreeView.SelectedNode = clickedNode;
                _semanticNodeMenu.Show(_semanticTreeView, mouseEvent.Location);
            }
        }

        private void DragGroup(object sender, ItemDragEventArgs e)
        {
            if (_isMoveMode && e.Item is TreeNode node)
                DoDragDrop(node, DragDropEffects.Move);
        }

        private void EnterDragGroup(object sender, DragEventArgs e)
        {
            if (e.Data != null && _isMoveMode && e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
        }

        private void DragOverGroup(object sender, DragEventArgs e)
        {
            if (!_isMoveMode)
                return;

            Point pt = _semanticTreeView.PointToClient(new Point(e.X, e.Y));
            _semanticTreeView.SelectedNode = _semanticTreeView.GetNodeAt(pt);
        }

        private void DragDropGroup(object sender, DragEventArgs e)
        {
            if (!_isMoveMode || _nodeBeingMoved?.Tag is not Guid sourceId)
                return;

            Point point = _semanticTreeView.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = _semanticTreeView.GetNodeAt(point);

            if (targetNode == _nodeBeingMoved)
                return;

            if (targetNode != null && targetNode.Bounds.Contains(point))
                MoveToParentNode(sourceId, targetNode);
            else
                MoveToRoot(sourceId);

            _isMoveMode = false;
            _nodeBeingMoved = null;
        }

        private void MoveToRoot(Guid sourceId)
        {
            if (_nodeBeingMoved == null)
                return;

            var confirm = MessageBox.Show(
                $"Move \"{_nodeBeingMoved.Text}\" to the root?",
                "Move semantic group?",
                MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes && _db.SemanticGroups.TryGet(sourceId, out var group))
            {
                group.ParentId = null;
                RefreshSemanticGroups();
            }
        }

        private void MoveToParentNode(Guid sourceId, TreeNode targetNode)
        {
            if (_nodeBeingMoved == null)
                return;

            var confirm = MessageBox.Show(
                $"Move \"{_nodeBeingMoved.Text}\" into \"{targetNode.Text}\"?",
                "Move semantic group?",
                MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes
                && targetNode.Tag is Guid targetId
                && _db.SemanticGroups.TryGet(sourceId, out var group))
            {
                if (IsDescendant(targetId, sourceId))
                {
                    MessageBox.Show("Moving forbidden", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _isMoveMode = false;
                    _nodeBeingMoved = null;
                    return;
                }

                group.ParentId = targetId;
                RefreshSemanticGroups();
            }
        }

        private bool IsDescendant(Guid childId, Guid parentId)
        {
            Guid? currentId = childId;

            while (currentId.HasValue)
            {
                if (currentId == parentId)
                    return true;

                if (_db.SemanticGroups.TryGet(currentId.Value, out var currentGroup))
                    currentId = currentGroup.ParentId;
                else
                    break;
            }

            return false;
        }
        #endregion SEMANTIC_GROUPS

        #region ROOTS
        private void RefreshRoots()
        {

        }
        #endregion ROOTS
    }
}
