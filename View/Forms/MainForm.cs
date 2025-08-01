using Comparatist.Core.Infrastructure;
using Comparatist.Services.Infrastructure;
using Comparatist.View.CategoryTree;
using Comparatist.View.LanguageGrid;
using Comparatist.View.Utities;
using Comparatist.View.WordGrid;

namespace Comparatist
{
    public partial class MainForm : Form
    {
        private Database _db = new();
        private ContentHolderTypes _currentContentHolder = ContentHolderTypes.AlphaRoots;
        private FileService _fileService;
        
        private LanguageGridViewAdapter _languageGridViewAdapter;
        private LanguageGridPresenter _languageGridPresenter;

        private CategoryTreeViewAdapter _categoryTreeViewAdapter;
        private CategoryTreePresenter _categoryTreePresenter;

        private WordGridViewAdapter _wordGridViewAdapter;
        private WordGridPresenter _wordGridPresenter;
        
        private IProjectService _service;

        public MainForm()
        {
            _db = new();
            InitializeComponent();

            _service = new ProjectService();
            _fileService = new FileService(_db, RefreshAllContent);

            _languageGridViewAdapter = new(_languagesGridView);
            _languageGridPresenter = new(_service, _languageGridViewAdapter);

            _categoryTreeViewAdapter = new(_semanticTreeView);
            _categoryTreePresenter = new(_service, _categoryTreeViewAdapter);

            _wordGridViewAdapter = new(_alphaRootsGridView);
            _wordGridPresenter = new(_service, _wordGridViewAdapter);
        }

        private void Open(object sender, EventArgs e) => _fileService.Open();
        private void SaveAs(object sender, EventArgs e) => _fileService.SaveAs();
        private void Save(object sender, EventArgs e) => _fileService.Save();
        private void Exit(object sender, EventArgs e) => Close();

        private void SelectAlphaRoots(object sender, EventArgs e) => SelectContent(ContentHolderTypes.AlphaRoots);
        private void SelectSemanticGroups(object sender, EventArgs e) => SelectContent(ContentHolderTypes.SemanticGroups);
        private void SelectLanguages(object sender, EventArgs e) => SelectContent(ContentHolderTypes.Languages);

 

        private void RefreshAllContent()
        {
            ShowActiveContentHolder();
            SetCheckedRepositoryMenu();
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
