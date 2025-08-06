using Comparatist.Services.Infrastructure;
using Comparatist.View.CategoryTree;
using Comparatist.View.Infrastructure;
using Comparatist.View.LanguageGrid;
using Comparatist.View.MainMenu;
using Comparatist.View.WordGrid;

namespace Comparatist
{
    public partial class MainForm : Form
    {
        private MainMenuViewAdapter _mainMenuViewAdapter;
        private MainMenuPresenter _mainMenuPresenter;

        private LanguageGridPresenter _languageGridPresenter;

        private CategoryTreeViewAdapter _categoryTreeViewAdapter;
        private CategoryTreePresenter _categoryTreePresenter;

        private WordGridViewAdapter _wordGridViewAdapter;
        private WordGridPresenter _wordGridPresenter;
        
        private IProjectService _service;

        public MainForm()
        {
            InitializeComponent();

            _service = new ProjectService();

            _mainMenuViewAdapter = new(_mainMenuStrip);
            _mainMenuPresenter = new(_service, _mainMenuViewAdapter);
            _mainMenuViewAdapter.ExitRequest += Close;

            var languageRenderer = new LanguageGridRenderer(_languageGridView);
            var languageInputHandler = new LanguageGridInputHandler(_languageGridView);
            _languageGridPresenter = new(_service, languageRenderer, languageInputHandler);

            _categoryTreeViewAdapter = new(_categoryTreeView);
            _categoryTreePresenter = new(_service, _categoryTreeViewAdapter);

            _wordGridViewAdapter = new(_wordGridView);
            _wordGridPresenter = new(_service, _wordGridViewAdapter);

            _mainMenuViewAdapter.RegisterPresenter(
                ContentTypes.Languages,
                _languageGridPresenter,
                "Languages");

            _mainMenuViewAdapter.RegisterViewAdapter(
                ContentTypes.Categories,
                _categoryTreeViewAdapter,
                "Categories");

            _mainMenuViewAdapter.RegisterViewAdapter(
                ContentTypes.Words,
                _wordGridViewAdapter,
                "Word table");
        }
    }
}
