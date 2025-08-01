using Comparatist.Core.Records;
using Comparatist.Services.TableCache;
using Comparatist.View.Infrastructure;
using Comparatist.View.Utities;

namespace Comparatist.View.WordGrid
{
    internal class WordGridViewAdapter : ViewAdapter
    {
        private DataGridView _grid;
        private DisposableMenu _gridMenu;
        private DisposableMenu _rootMenu;
        private DisposableMenu _stemMenu;
        private DisposableMenu _wordMenu;
        private WordGridRenderHelper _renderHelper;

        public WordGridViewAdapter(DataGridView grid)
        {
            _grid = grid;
            _renderHelper = new WordGridRenderHelper(grid);

            _gridMenu = new DisposableMenu(
                ("Add root", FakeAction));

            _rootMenu = new DisposableMenu(
                ("Add root", FakeAction),
                ("Edit root", FakeAction),
                ("Delete root", FakeAction),
                ("Add stem", FakeAction));

            _stemMenu = new DisposableMenu(
                ("Add stem", FakeAction),
                ("Edit stem", FakeAction),
                ("Delete stem", FakeAction));

            _wordMenu = new DisposableMenu(
                ("Add or edit word", FakeAction),
                ("Delete word", FakeAction));

            SetupGrid();
        }

        private void SetupGrid()
        {
            throw new NotImplementedException();
        }

        public void Render(IEnumerable<CachedBlock> blocks, IEnumerable<Language> languages) =>
            _renderHelper.Render(blocks, languages);

        private void FakeAction()
        {

        }

        protected override void Unsubscribe()
        {
            throw new NotImplementedException();
        }
    }
}
