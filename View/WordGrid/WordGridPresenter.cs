using Comparatist.Core.Records;
using Comparatist.Services.Cache;
using Comparatist.Services.Infrastructure;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.WordGrid
{
    internal class WordGridPresenter :
        Presenter<WordGridRenderer, WordGridInputHandler, DataGridView>
    {
        private SortingTypes _sortingType = SortingTypes.Alphabet;
        private Dictionary<Guid, SectionBinder> _sectionBinders = new();
        private List<Guid> _orderedBinderIds = new();

        public WordGridPresenter(
            IProjectService service,
            WordGridRenderer renderer,
            WordGridInputHandler inputHandler) :
            base(service, renderer, inputHandler)
        { }
        public SortingTypes SortingType
        {
            get { return _sortingType; }
            set { _sortingType = value; RedrawAll(); }
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
        }

        protected override void OnShow()
        {
            RedrawAll();
        }

        protected override void OnHide()
        {
            Reset();
        }

        private void Reset()
        {
            _sectionBinders.Clear();
            _orderedBinderIds.Clear();
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

            InputHandler.AllCategories = categories.Values.Select(e => e.Record);
            InputHandler.AllRoots = roots.Values.Select(e => e.Record);

            var orderedLanguages = languages.Values.OrderBy(e => e.Record.Order).ToList();
            Renderer.CreateColumns(orderedLanguages);

            var orderedCategories = state.Values.OrderBy(e => e.Record.Order).ToList();

            foreach (var category in orderedCategories)
                CreateBinder(category);
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

            InputHandler.AllCategories = categories.Values.Select(e => e.Record);
            InputHandler.AllRoots = roots.Values.Select(e => e.Record);

            foreach (var category in state.Values)
                if (_sectionBinders.TryGetValue(category.Record.Id, out var binder))
                    binder.Update(category);
        }

        private void CreateBinder(CachedCategory category)
        {
            var binder = new SectionBinder(category, Renderer);
            _sectionBinders.Add(category.Record.Id, binder);
            _orderedBinderIds.Add(category.Record.Id);
            binder.Initialize();
        }
    }
}
