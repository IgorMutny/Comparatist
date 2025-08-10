using Comparatist.Data.Entities;
using Comparatist.Application.Cache;
using Comparatist.Application.Management;
using Comparatist.View.Common;
using Comparatist.View.WordGrid.Binders;

namespace Comparatist.View.WordGrid
{
    internal class WordGridPresenter :
        Presenter<WordGridRenderer, WordGridInputHandler>
    {
        private SortingTypes _sortingType = SortingTypes.Alphabet;
        private Dictionary<Guid, CategoryBinder> _binders = new();
        private bool _isShown;

        public WordGridPresenter(
            IProjectService service,
            WordGridRenderer renderer,
            WordGridInputHandler inputHandler) :
            base(service, renderer, inputHandler)
        { }

        public SortingTypes SortingType
        {
            get { return _sortingType; }
            set { _sortingType = value; if (_isShown) { RedrawAll(); } }
        }

        protected override void Subscribe()
        {
            InputHandler.AddRootRequest += OnAddRootRequest;
            InputHandler.UpdateRootRequest += OnUpdateRootRequest;
            InputHandler.DeleteRootRequest += OnDeleteRootRequest;
            InputHandler.AddStemRequest += OnAddStemRequest;
            InputHandler.UpdateStemRequest += OnUpdateStemRequest;
            InputHandler.DeleteStemRequest += OnDeleteStemRequest;
            InputHandler.AddWordRequest += OnAddWordRequest;
            InputHandler.UpdateWordRequest += OnUpdateWordRequest;
            InputHandler.DeleteWordRequest += OnDeleteWordRequest;
            InputHandler.ExpandOrCollapseRequested += OnExpandOrCollapseRequested;
        }

        protected override void Unsubscribe()
        {
            InputHandler.AddRootRequest -= OnAddRootRequest;
            InputHandler.UpdateRootRequest -= OnUpdateRootRequest;
            InputHandler.DeleteRootRequest -= OnDeleteRootRequest;
            InputHandler.AddStemRequest -= OnAddStemRequest;
            InputHandler.UpdateStemRequest -= OnUpdateStemRequest;
            InputHandler.DeleteStemRequest -= OnDeleteStemRequest;
            InputHandler.AddWordRequest -= OnAddWordRequest;
            InputHandler.UpdateWordRequest -= OnUpdateWordRequest;
            InputHandler.DeleteWordRequest -= OnDeleteWordRequest;
            InputHandler.ExpandOrCollapseRequested -= OnExpandOrCollapseRequested;
        }

        protected override void OnShow()
        {
            RedrawAll();
            _isShown = true;
        }

        protected override void OnHide()
        {
            Reset();
            _isShown = false;
        }

        private void Reset()
        {
            _binders.Clear();
            Renderer.Reset();
        }

        private void OnAddRootRequest(Root root)
        {
            Execute(() => Service.Add(root));
            DrawDiff();
        }

        private void OnUpdateRootRequest(Root root)
        {
            Execute(() => Service.Update(root));
            DrawDiff();
        }

        private void OnDeleteRootRequest(Root root)
        {
            Execute(() => Service.Delete(root));
            DrawDiff();
        }

        private void OnAddStemRequest(Stem stem)
        {
            Execute(() => Service.Add(stem));
            DrawDiff();
        }

        private void OnUpdateStemRequest(Stem stem)
        {
            Execute(() => Service.Update(stem));
            DrawDiff();
        }

        private void OnDeleteStemRequest(Stem stem)
        {
            Execute(() => Service.Delete(stem));
            DrawDiff();
        }

        private void OnAddWordRequest(Word word)
        {
            Execute(() => Service.Add(word));
            DrawDiff();
        }

        private void OnUpdateWordRequest(Word word)
        {
            Execute(() => Service.Update(word));
            DrawDiff();
        }

        private void OnDeleteWordRequest(Word word)
        {
            Execute(() => Service.Delete(word));
            DrawDiff();
        }

        private void RedrawAll()
        {
            Reset();

            var languages = Execute(Service.GetAllLanguages);
            var categories = Execute(Service.GetAllCategories);
            var roots = Execute(Service.GetAllRoots);
            var state = SortingType == SortingTypes.Categories
                ? Execute(Service.GetWordTableByCategory)
                : Execute(Service.GetWordTableByAlphabet);

            if (languages == null || categories == null || roots == null || state == null)
            {
                Renderer.ShowError("Not enough cached data received");
                return;
            }

            InputHandler.AllCategories = categories.Values.Select(e => e.Record).ToList();
            InputHandler.AllRoots = roots.Values.Select(e => e.Record).OrderBy(e => e.Value).ToList();

            var orderedLanguages = languages.Values.OrderBy(e => e.Record.Order).ToList();
            Renderer.CreateColumns(orderedLanguages);

            var orderedCategories = state.Values.OrderBy(e => e.Record.Order).ToList();

            foreach (var category in orderedCategories)
                AddBinder(category);
        }

        private void DrawDiff()
        {
            var categories = Execute(Service.GetAllCategories);
            var roots = Execute(Service.GetAllRoots);
            var state = SortingType == SortingTypes.Categories
                ? Execute(Service.GetWordTableByCategory)
                : Execute(Service.GetWordTableByAlphabet);

            if (state == null || roots == null || categories == null)
            {
                Renderer.ShowError("No cached data received");
                return;
            }

            InputHandler.AllCategories = categories.Values.Select(e => e.Record).ToList();
            InputHandler.AllRoots = roots.Values.Select(e => e.Record).OrderBy(e => e.Value).ToList();

            foreach (var category in state.Values)
                if (_binders.TryGetValue(category.Record.Id, out var binder))
                    binder.Update(category);
        }

        private void AddBinder(CachedCategory category)
        {
            var binder = new CategoryBinder(category, Renderer);
            _binders.Add(category.Record.Id, binder);
            Renderer.Add(binder);
            binder.OnCreate();
        }

        private void OnExpandOrCollapseRequested(Root root)
        {
            var categoryIds = root.CategoryIds;

            foreach (var categoryId in categoryIds)
                if (_binders.TryGetValue(categoryId, out var binder))
                    binder.ExpandOrCollapse(root);

            if (_binders.TryGetValue(Guid.Empty, out var specialBinder))
                specialBinder.ExpandOrCollapse(root);
        }
    }
}
