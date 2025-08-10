using Comparatist.Application.Management;
using Comparatist.View.CategoryTree;
using Comparatist.View.Common;
using Comparatist.View.LanguageGrid;
using Comparatist.View.MainMenu;
using Comparatist.View.WordGrid;

namespace Comparatist
{
    public partial class MainForm : Form
    {
        private MainMenuHandler _mainMenuHandler;

        private LanguageGridPresenter _languageGridPresenter;
        private CategoryTreePresenter _categoryTreePresenter;
        private WordGridPresenter _wordGridPresenter;
    
        private IProjectService _service;

        public MainForm()
        {
            InitializeComponent();

            _service = new ProjectService();

            _mainMenuHandler = new(_service, _mainMenuStrip);
            _mainMenuHandler.ExitRequest += Close;

            var languageRenderer = new LanguageGridRenderer(_languageGridView);
            var languageInputHandler = new LanguageGridInputHandler(_languageGridView);
            _languageGridPresenter = new LanguageGridPresenter(_service, languageRenderer, languageInputHandler);

            var categoryTreeRenderer = new CategoryTreeRenderer(_categoryTreeView);
            var categoryTreeInputHandler = new CategoryTreeInputHandler(_categoryTreeView);
            _categoryTreePresenter = new(_service, categoryTreeRenderer, categoryTreeInputHandler);

            var wordRenderer = new WordGridRenderer(_wordGridView);
            var wordInputHandler = new WordGridInputHandler(_wordGridView);
            _wordGridPresenter = new WordGridPresenter(_service, wordRenderer, wordInputHandler);

            _mainMenuHandler.RegisterPresenter(
                ContentTypes.Languages,
                _languageGridPresenter,
                "Languages");

            _mainMenuHandler.RegisterPresenter(
                ContentTypes.Categories,
                _categoryTreePresenter,
                "Categories");

            _mainMenuHandler.RegisterPresenter(
                ContentTypes.Words,
                _wordGridPresenter,
                "Word table");

            _mainMenuHandler.Initialize();
        }
    }
}
