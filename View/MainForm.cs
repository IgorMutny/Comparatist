namespace Comparatist
{
    public partial class MainForm : Form
    {
        private Database _db = new();
        private ContentHolderTypes _currentContentHolder = ContentHolderTypes.Roots;
        private FileService _fileService;
        private LanguagesService _languagesService;
        private SourcesService _sourcesService;
        private SemanticTreeService _semanticTreeService;

        public MainForm()
        {
            _db = new();
            InitializeComponent();
            _fileService = new FileService(_db, RefreshAllContent);
            _languagesService = new(_languagesGridView, _db.Languages);
            _sourcesService = new(_sourcesGridView, _db.Sources);
            _semanticTreeService = new(
                _semanticTreeView,
                _db.SemanticGroups,
                _semanticMenu,
                _semanticNodeMenu,
                (obj, e) => DoDragDrop(obj, e));
        }

        private void Open(object sender, EventArgs e) => _fileService.Open();
        private void SaveAs(object sender, EventArgs e) => _fileService.SaveAs();
        private void Save(object sender, EventArgs e) => _fileService.Save();
        private void Exit(object sender, EventArgs e) => Close();

        private void SelectRoots(object sender, EventArgs e) => SelectContent(ContentHolderTypes.Roots);
        private void SelectSemanticGroups(object sender, EventArgs e) => SelectContent(ContentHolderTypes.SemanticGroups);
        private void SelectLanguages(object sender, EventArgs e) => SelectContent(ContentHolderTypes.Languages);
        private void SelectSources(object sender, EventArgs e) => SelectContent(ContentHolderTypes.Sources);

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

        private void RefreshAllContent()
        {
            ShowActiveContentHolder();
            SetCheckedRepositoryMenu();
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
            _semanticTreeView.Visible = _currentContentHolder == ContentHolderTypes.SemanticGroups;
            _sourcesGridView.Visible = _currentContentHolder == ContentHolderTypes.Sources;
        }

        private void SelectContent(ContentHolderTypes type)
        {
            _currentContentHolder = type;
            RefreshAllContent();
        }
    }
}
