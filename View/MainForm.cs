using Comparatist.View.Services;

namespace Comparatist
{
    public partial class MainForm : Form
    {
        private Database _db = new();
        private ContentHolderTypes _currentContentHolder = ContentHolderTypes.AlphaRoots;
        private FileService _fileService;
        private LanguagesService _languagesService;
        private SourcesService _sourcesService;
        private SemanticTreeService _semanticTreeService;
        private AlphaRootsService _alphaRootsService;

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
            _alphaRootsService = new(_db, _alphaRootsGridView, _rootGridMenu, _rootRowMenu);
        }

        private void Open(object sender, EventArgs e) => _fileService.Open();
        private void SaveAs(object sender, EventArgs e) => _fileService.SaveAs();
        private void Save(object sender, EventArgs e) => _fileService.Save();
        private void Exit(object sender, EventArgs e) => Close();

        private void SelectAlphaRoots(object sender, EventArgs e) => SelectContent(ContentHolderTypes.AlphaRoots);
        private void SelectSemanticGroups(object sender, EventArgs e) => SelectContent(ContentHolderTypes.SemanticGroups);
        private void SelectLanguages(object sender, EventArgs e) => SelectContent(ContentHolderTypes.Languages);
        private void SelectSources(object sender, EventArgs e) => SelectContent(ContentHolderTypes.Sources);

        private void AddSource(object sender, EventArgs e) => _sourcesService.Add();
        private void EditSource(object sender, EventArgs e) => _sourcesService.Edit();
        private void DeleteSource(object sender, EventArgs e) => _sourcesService.Delete();


        private void AddLanguage(object sender, EventArgs e) => _languagesService.Add();
        private void EditLanguage(object sender, EventArgs e) => _languagesService.Edit();
        private void DeleteLanguage(object sender, EventArgs e) => _languagesService.Delete();

        private void AddHeadGroup(object sender, EventArgs e) => _semanticTreeService.AddHead();
        private void AddChildGroup(object sender, EventArgs e) => _semanticTreeService.AddChild();
        private void EditGroup(object sender, EventArgs e) => _semanticTreeService.Edit();
        private void MoveGroup(object sender, EventArgs e) => _semanticTreeService.Move();
        private void DeleteGroup(object sender, EventArgs e) => _semanticTreeService.Delete();

        private void AddRoot(object sender, EventArgs e) => _alphaRootsService.AddRoot();
        private void EditRoot(object sender, EventArgs e) => _alphaRootsService.EditRoot();
        private void DeleteRoot(object sender, EventArgs e) => _alphaRootsService.DeleteRoot();

        private void RefreshAllContent()
        {
            ShowActiveContentHolder();
            SetCheckedRepositoryMenu();
            _semanticTreeService.Refresh();
            _languagesService.Refresh();
            _sourcesService.Refresh();
            _alphaRootsService.Refresh();
        }

        private void SetCheckedRepositoryMenu()
        {
            _showAlphaRootsMenuItem.Checked = _currentContentHolder == ContentHolderTypes.AlphaRoots;
            _showSemanticMenuItem.Checked = _currentContentHolder == ContentHolderTypes.SemanticGroups;
            _showLanguageMenuItem.Checked = _currentContentHolder == ContentHolderTypes.Languages;
            _showSourcesMenuItem.Checked = _currentContentHolder == ContentHolderTypes.Sources;
        }

        private void ShowActiveContentHolder()
        {
            _alphaRootsGridView.Visible = _currentContentHolder == ContentHolderTypes.AlphaRoots;
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
