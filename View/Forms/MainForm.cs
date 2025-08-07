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
            _languageGridPresenter = new LanguageGridPresenter(_service, languageRenderer, languageInputHandler);

            _categoryTreeViewAdapter = new(_categoryTreeView);
            _categoryTreePresenter = new(_service, _categoryTreeViewAdapter);

            var wordRenderer = new WordGridRenderer(_wordGridView);
            var wordInputHandler = new WordGridInputHandler(_wordGridView);
            _wordGridPresenter = new WordGridPresenter(_service, wordRenderer, wordInputHandler);

            _mainMenuViewAdapter.RegisterPresenter(
                ContentTypes.Languages,
                _languageGridPresenter,
                "Languages");

            _mainMenuViewAdapter.RegisterViewAdapter(
                ContentTypes.Categories,
                _categoryTreeViewAdapter,
                "Categories");

            _mainMenuViewAdapter.RegisterPresenter(
                ContentTypes.Words,
                _wordGridPresenter,
                "Word table");

            _mainMenuViewAdapter.Initialize();
        }
    }
}
