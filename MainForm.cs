namespace Comparatist
{
    public partial class MainForm : Form
    {
        private InMemoryDatabase _db = new();
        private string _filePath = string.Empty;
        private RepoTypes _currentRepo = RepoTypes.Roots;

        public MainForm()
        {
            InitializeComponent();
            SetupLanguageGrid();
        }

        #region FILE_OPERATIONS
        private void fileToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void Open(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "DataBase files (*.db)|*.db";
                dialog.Title = "Select DataBase file";

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
                dialog.Filter = "DataBase files (*.db)|*.db";
                dialog.Title = "Save DataBase as...";

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
            if (_db != null && !string.IsNullOrEmpty(_filePath))
                _db.Save(_filePath);
            else
                MessageBox.Show("Nothing to save!");
        }

        private void Exit(object sender, EventArgs e)
        {
            if (_db != null)
            {
                if (string.IsNullOrEmpty(_filePath))
                    SaveAs(sender, EventArgs.Empty);
                else
                    Save(sender, EventArgs.Empty);
            }

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
                gridView.CurrentCell = dataGridViewLanguages.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        private void RefreshContent()
        {
            ShowActiveGrid();
            SetCheckedRepositoryMenu();

            switch (_currentRepo)
            {
                case RepoTypes.Roots: RefreshRoots(); break;
                case RepoTypes.SemanticGroups: RefreshSemanticGroups(); break;
                case RepoTypes.Languages: RefreshLanguages(); break;
                case RepoTypes.Sources: RefreshSources(); break;
            }
        }

        private void SetCheckedRepositoryMenu()
        {
            rootsToolStripMenuItem.Checked = _currentRepo == RepoTypes.Roots;
            semanticGroupsToolStripMenuItem.Checked = _currentRepo == RepoTypes.SemanticGroups;
            languagesToolStripMenuItem.Checked = _currentRepo == RepoTypes.Languages;
            sourcesToolStripMenuItem.Checked = _currentRepo == RepoTypes.Sources;
        }

        private void ShowActiveGrid()
        {
            dataGridViewLanguages.Visible = _currentRepo == RepoTypes.Languages;
            //dataGridViewWords.Visible = repo == "Words";
            //dataGridViewStems.Visible = repo == "Stems";
            //dataGridViewRoots.Visible = repo == "Roots";
            //dataGridViewGroups.Visible = repo == "SemanticGroups";
            //dataGridViewSources.Visible = repo == "Sources";
        }

        private void SelectWordsRepo(object sender, EventArgs e)
        {
            _currentRepo = RepoTypes.Roots;
            RefreshContent();
        }

        private void SelectSemanticGroupsRepo(object sender, EventArgs e)
        {
            _currentRepo = RepoTypes.SemanticGroups;
            RefreshContent();
        }

        private void SelectLanguagesRepo(object sender, EventArgs e)
        {
            _currentRepo = RepoTypes.Languages;
            RefreshContent();
        }

        private void SelectSourcesRepo(object sender, EventArgs e)
        {
            _currentRepo = RepoTypes.Sources;
            RefreshContent();
        }

        #endregion COMMON_CONTENT

        #region SOURCES
        private void RefreshSources()
        {

        }
        #endregion SOURCES

        #region LANGUAGES
        private void SetupLanguageGrid()
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

            if (row.Tag == null)
                return;

            Guid id = (Guid)row.Tag;

            var language = row.Cells[0].Value.ToString() ?? string.Empty;
            var input = InputDialog.Show("Edit language", $"Enter new name for {language}");
            if (!string.IsNullOrWhiteSpace(input))
            {
                var item = new Language() { Value = input };
                _db.Languages.Update(id, item);
                RefreshContent();
            }
        }

        private void RemoveLanguage(object sender, EventArgs e)
        {
            if (dataGridViewLanguages.SelectedRows.Count == 0)
                return;

            var row = dataGridViewLanguages.SelectedRows[0];

            if (row.Tag == null)
                return;

            Guid id = (Guid)row.Tag;
            var language = row.Cells[0].Value.ToString() ?? string.Empty;

            var result = MessageBox.Show($"Remove language {language}?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _db.Languages.Delete(id);
                RefreshContent();
            }
        }
        #endregion LANGUAGES

        #region SEMANTIC_GROUPS
        private void RefreshSemanticGroups()
        {

        }
        #endregion SEMANTIC_GROUPS

        #region ROOTS
        private void RefreshRoots()
        {

        }
        #endregion ROOTS





        



        
    }

    public enum RepoTypes
    {
        Roots = 0,
        SemanticGroups = 1,
        Languages = 2,
        Sources = 3,
    }
}
