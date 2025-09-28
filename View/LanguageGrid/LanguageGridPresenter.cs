using Comparatist.Data.Entities;
using Comparatist.Application.Cache;
using Comparatist.Application.Management;
using Comparatist.View.Common;

namespace Comparatist.View.LanguageGrid
{
    internal class LanguageGridPresenter :
        Presenter<LanguageGridRenderer, LanguageGridInputHandler>
    {
        private List<CachedLanguage> _previousState = new();
        private List<CachedLanguage> _currentState = new();
        private Dictionary<Guid, LanguageBinder> _binders = new();

        public LanguageGridPresenter(
            IProjectService service,
            LanguageGridRenderer renderer,
            LanguageGridInputHandler inputHandler) :
            base(service, renderer, inputHandler)
        { }

        protected override void Subscribe()
        {
            InputHandler.AddRequest += OnAddRequest;
            InputHandler.UpdateRequest += OnUpdateRequest;
            InputHandler.UpdateManyRequest += OnUpdateManyRequest;
            InputHandler.DeleteRequest += OnDeleteRequest;
        }

        protected override void Unsubscribe()
        {
            InputHandler.AddRequest -= OnAddRequest;
            InputHandler.UpdateRequest -= OnUpdateRequest;
            InputHandler.UpdateManyRequest -= OnUpdateManyRequest;
            InputHandler.DeleteRequest -= OnDeleteRequest;
        }

        protected override void OnShow()
        {
            IsActive = true;
            RedrawAll();
        }

        protected override void OnHide()
        {
            IsActive = false;
            Reset();
        }

        private void Reset()
        {
            _previousState.Clear();
            _currentState.Clear();
            _binders.Clear();
            Renderer.Reset();
        }

        private void OnAddRequest(Language language)
        {
            Execute(() => Service.Add(language));
            DrawDiff();
        }

        private void OnUpdateRequest(Language language)
        {
            Execute(() => Service.Update(language));
            DrawDiff();
        }

        private void OnUpdateManyRequest(IEnumerable<Language> languages)
        {
            Execute(() => Service.UpdateMany(languages));
            DrawDiff();
        }

        private void OnDeleteRequest(Language language)
        {
            Execute(() => Service.Delete(language));
            DrawDiff();
        }

        public override void RedrawAll()
        {
            if(!IsActive)
                return;

            Reset();

            var languages = Execute(Service.GetAllLanguages);

            if (languages == null)
            {
                Renderer.ShowError("No language cache received");
                return;
            }

            _currentState = languages.ToList();
            _previousState = languages.Select(e => (CachedLanguage)e.Clone()).ToList();

            foreach (var language in _currentState)
                AddBinder(language);
        }

        private void DrawDiff()
        {
            Renderer.OnBeginUpdate();
            var state = Execute(Service.GetAllLanguages);

            if (state == null)
            {
                Renderer.ShowError("No language cache received");
                return;
            }

            _currentState = state.ToList();
            var currentIds = _currentState.Select(e => e.Record.Id).ToList();
            var previousIds = _previousState.Select(e => e.Record.Id).ToList();
            UpdateBinderSet(currentIds, previousIds);
            UpdateBindersContent(currentIds, previousIds);
            _previousState = state.Select(e => (CachedLanguage)e.Clone()).ToList();
            Renderer.OnEndUpdate();
        }

        private void UpdateBindersContent(List<Guid> currentIds, List<Guid> previousIds)
        {
            var updatedIds = currentIds.Intersect(previousIds);
            bool needsReorder = false;

            foreach (var id in updatedIds)
            {
                var binder = _binders[id];
                var language = _currentState.First(e => e.Record.Id == id);
                binder.Update(language);

                if (binder.NeedsReorder)
                    needsReorder = true;
            }

            if (needsReorder)
                UpdateChildrenOrder();
        }

        private void UpdateBinderSet(List<Guid> currentIds, List<Guid> previousIds)
        {
            var addedIds = currentIds.Except(previousIds);
            var removedIds = previousIds.Except(currentIds);

            foreach (var addedId in addedIds)
            {
                var language = _currentState.First(e => e.Record.Id == addedId);
                AddBinder(language);
            }

            foreach (var removedId in removedIds)
                RemoveBinder(removedId);

            if (addedIds.Count() > 0 || removedIds.Count() > 0)
                UpdateChildrenOrder();
        }

        private void UpdateChildrenOrder()
        {
            var orderedChildren = _binders.Values
                .OrderBy(b => b.Order)
                .ToList();

            for (int i = 0; i < orderedChildren.Count; i++)
            {
                var currentBinder = orderedChildren[i];

                if (!currentBinder.NeedsReorder)
                    continue;

                var previousBinder = i > 0
                    ? orderedChildren[i - 1]
                    : null;

                Renderer.Move(currentBinder, previousBinder);
                currentBinder.NeedsReorder = false;
            }
        }

        private void AddBinder(CachedLanguage language)
        {
            var binder = new LanguageBinder(language, Renderer);
            _binders.Add(binder.Id, binder);
            Renderer.Add(binder);
            binder.NeedsReorder = true;
        }

        private void RemoveBinder(Guid id)
        {
            var binder = _binders[id];
            Renderer.Remove(binder);
            _binders.Remove(id);
        }
    }
}
