using Comparatist.Core.Infrastructure;
using Comparatist.Services.Infrastructure;
using Comparatist.Services.TableCache;
using Comparatist.View.CategoryTree;

namespace Comparatist
{
    public partial class MainForm : Form
    {
        private Database _db = new();
        private ContentHolderTypes _currentContentHolder = ContentHolderTypes.AlphaRoots;
        private FileService _fileService;
        private LanguagesService _languagesService;
        private CategoryTreePresenter _categoryTreePresenter;
        private CategoryTreeViewAdapter _categoryTreeViewAdapter;
        private AlphaRootsService _alphaRootsService;
        private IProjectService _service;

        public MainForm()
        {
            _db = new();
            InitializeComponent();

            _service = new ProjectService();
            _fileService = new FileService(_db, RefreshAllContent);
            _languagesService = new(_languagesGridView, _db.Languages);

            _categoryTreeViewAdapter = new(_semanticTreeView);
            _categoryTreePresenter = new(_service, _categoryTreeViewAdapter);

            _alphaRootsService = new(
                _alphaRootsGridView,
                _rootGridMenu,
                _rootRowMenu,
                _stemRowMenu,
                _wordMenu);
        }

        private void Open(object sender, EventArgs e) => _fileService.Open();
        private void SaveAs(object sender, EventArgs e) => _fileService.SaveAs();
        private void Save(object sender, EventArgs e) => _fileService.Save();
        private void Exit(object sender, EventArgs e) => Close();

        private void SelectAlphaRoots(object sender, EventArgs e) => SelectContent(ContentHolderTypes.AlphaRoots);
        private void SelectSemanticGroups(object sender, EventArgs e) => SelectContent(ContentHolderTypes.SemanticGroups);
        private void SelectLanguages(object sender, EventArgs e) => SelectContent(ContentHolderTypes.Languages);

        private void AddLanguage(object sender, EventArgs e) => _languagesService.Add();
        private void EditLanguage(object sender, EventArgs e) => _languagesService.Edit();
        private void DeleteLanguage(object sender, EventArgs e) => _languagesService.Delete();

        //private void AddHeadGroup(object sender, EventArgs e) => _semanticTreeService.AddHead();
        //private void AddChildGroup(object sender, EventArgs e) => _semanticTreeService.AddChild();
        //private void EditGroup(object sender, EventArgs e) => _semanticTreeService.Edit();
        //private void MoveGroup(object sender, EventArgs e) => _semanticTreeService.Move();
        //private void DeleteGroup(object sender, EventArgs e) => _semanticTreeService.Delete();

        private void AddRoot(object sender, EventArgs e) => _alphaRootsService.AddRoot();
        private void EditRoot(object sender, EventArgs e) => _alphaRootsService.EditRoot();
        private void DeleteRoot(object sender, EventArgs e) => _alphaRootsService.DeleteRoot();

        private void AddStem(object sender, EventArgs e) => _alphaRootsService.AddStem();
        private void AddStemWithRoot(object sender, EventArgs e) => _alphaRootsService.AddStem();
        private void EditStem(object sender, EventArgs e) => _alphaRootsService.EditStem();
        private void DeleteStem(object sender, EventArgs e) => _alphaRootsService.DeleteStem();

        private void AddOrEditWord(object sender, EventArgs e) => _alphaRootsService.AddOrEditWord();
        private void DeleteWord(object sender, EventArgs e) => _alphaRootsService.DeleteWord();

        private void RefreshAllContent()
        {
            ShowActiveContentHolder();
            SetCheckedRepositoryMenu();
            //_semanticTreeService.Refresh();
            _languagesService.Refresh();
            _alphaRootsService.Refresh();
        }

        private void SetCheckedRepositoryMenu()
        {
            _showAlphaRootsMenuItem.Checked = _currentContentHolder == ContentHolderTypes.AlphaRoots;
            _showSemanticMenuItem.Checked = _currentContentHolder == ContentHolderTypes.SemanticGroups;
            _showLanguageMenuItem.Checked = _currentContentHolder == ContentHolderTypes.Languages;
        }

        private void ShowActiveContentHolder()
        {
            _alphaRootsGridView.Visible = _currentContentHolder == ContentHolderTypes.AlphaRoots;
            _languagesGridView.Visible = _currentContentHolder == ContentHolderTypes.Languages;
            _semanticTreeView.Visible = _currentContentHolder == ContentHolderTypes.SemanticGroups;
        }

        private void SelectContent(ContentHolderTypes type)
        {
            _currentContentHolder = type;
            RefreshAllContent();
        }
    }
}
