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

        }

        protected override void Unsubscribe()
        {

        }

        protected override void OnShow()
        {
            RedrawAll();
        }

        protected override void OnHide()
        {
            _sectionBinders.Clear();
            _orderedBinderIds.Clear();
        }

        private void RedrawAll()
        {
            _sectionBinders.Clear();
            _orderedBinderIds.Clear();
            Renderer.Clear();

            var languages = Execute(Service.GetAllLanguages);
            var state = Execute(() => Service.GetWordTable(SortingType));

            if (state == null || languages == null)
            {
                Renderer.ShowError("No cached data received");
                return;
            }

            var orderedLanguages = languages.Values.OrderBy(e => e.Record.Order).ToList();
            Renderer.CreateColumns(orderedLanguages);

            CollectBinders(state);

            foreach(var id in _orderedBinderIds)
            {
                var binder = _sectionBinders[id];
                binder.Initialize();
            }
        }

        private void CollectBinders(Dictionary<Guid, CachedCategory> state)
        {
            var orderedCategories = state.Values.OrderBy(e => e.Record.Order).ToList();

            foreach (var category in orderedCategories)
                CollectBindersRecursively(category);
        }

        private void CollectBindersRecursively(CachedCategory category)
        {
            _sectionBinders.Add(category.Record.Id, new SectionBinder(category, Renderer));
            _orderedBinderIds.Add(category.Record.Id);

            var orderedChildren = category.Children.Values.OrderBy(e => e.Record.Order).ToList();

            foreach (var child in orderedChildren)
                CollectBindersRecursively(child);
        }
    }
}
