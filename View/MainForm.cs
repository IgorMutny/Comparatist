namespace Comparatist
{
    public partial class MainForm : Form
    {
        private const string Extension = ".comparatist";

        private Database _db = new();
        private string _filePath = string.Empty;
        private ContentHolderTypes _currentContentHolder = ContentHolderTypes.Roots;
        private LanguagesService _languagesService;
        private SourcesService _sourcesService;
        private SemanticTreeService _semanticTreeService;

        public MainForm()
        {
            _db = new();
            InitializeComponent();
            _languagesService = new(_languagesGridView, _db.Languages);
            _sourcesService = new(_sourcesGridView, _db.Sources);
            _semanticTreeService = new(
                _semanticTreeView,
                _db.SemanticGroups,
                _semanticMenu,
                _semanticNodeMenu,
                (obj, e) => DoDragDrop(obj, e));
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
                    RefreshAllContent();
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

        private void RefreshAllContent()
        {
            ShowActiveContentHolder();
            SetCheckedRepositoryMenu();
            RefreshRoots();
            _semanticTreeService.Refresh();
            _languagesService.Refresh();
            _sourcesService.Refresh();
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

        private void SelectRoots(object sender, EventArgs e)
        {
            _currentContentHolder = ContentHolderTypes.Roots;
            RefreshAllContent();
        }

        private void SelectSemanticGroups(object sender, EventArgs e)
        {
            _currentContentHolder = ContentHolderTypes.SemanticGroups;
            RefreshAllContent();
        }

        private void SelectLanguages(object sender, EventArgs e)
        {
            _currentContentHolder = ContentHolderTypes.Languages;
            RefreshAllContent();
        }

        private void SelectSources(object sender, EventArgs e)
        {
            _currentContentHolder = ContentHolderTypes.Sources;
            RefreshAllContent();
        }

        #endregion COMMON_CONTENT

        private void AddSource(object sender, EventArgs e) => _sourcesService.Add();
        private void EditSource(object sender, EventArgs e) => _sourcesService.Edit();
        private void DeleteSource(object sender, EventArgs e) => _sourcesService.Delete();


        private void AddLanguage(object sender, EventArgs e) => _languagesService.Add();
        private void EditLanguage(object sender, EventArgs e) => _languagesService.Edit();
        private void DeleteLanguage(object sender, EventArgs e) => _languagesService.Delete();

        private void AddRootGroup(object sender, EventArgs e) => _semanticTreeService.AddRoot();
        private void AddChildGroup(object sender, EventArgs e) => _semanticTreeService.AddChild();
        private void EditGroup(object sender, EventArgs e) => _semanticTreeService.Edit();
        private void MoveGroup(object sender, EventArgs e) => _semanticTreeService.Move();
        private void DeleteGroup(object sender, EventArgs e) => _semanticTreeService.Delete();

        #region ROOTS
        private void RefreshRoots()
        {

        }
        #endregion ROOTS
    }
}
