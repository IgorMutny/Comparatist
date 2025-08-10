using Comparatist.Data.Entities;
using Comparatist.Application.Cache;
using Comparatist.Application.Management;
using Comparatist.View.Common;

namespace Comparatist.View.LanguageGrid
{
    internal class LanguageGridPresenter :
        Presenter<LanguageGridRenderer, LanguageGridInputHandler>
    {
        private Dictionary<Guid, CachedLanguage> _previousState = new();
        private Dictionary<Guid, CachedLanguage> _currentState = new();
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
            RedrawAll();
        }

        protected override void OnHide()
        {
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

        private void RedrawAll()
        {
            Reset();

            var languages = Execute(Service.GetAllLanguages);

            if (languages == null)
            {
                Renderer.ShowError("No language cache received");
                return;
            }

            _currentState = languages;
            _previousState = languages;

            var orderedLanguages = languages.Values.OrderBy(e => e.Record.Order).ToList();

            foreach (var language in orderedLanguages)
                AddBinder(language);
        }

        private void DrawDiff()
        {
            var state = Execute(Service.GetAllLanguages);

            if (state == null)
            {
                Renderer.ShowError("No language cache received");
                return;
            }

            _currentState = state;
            UpdateBinderSet();
            UpdateBindersContent();
            _previousState = state;
        }

        private void UpdateBindersContent()
        {
            var oldBinderIds = _currentState.Keys.Intersect(_previousState.Keys);
            bool needsReorder = false;

            foreach (var id in oldBinderIds)
            {
                var binder = _binders[id];
                binder.Update(_currentState[id]);

                if (binder.NeedsReorder)
                    needsReorder = true;
            }

            if (needsReorder)
                UpdateChildrenOrder();
        }

        private void UpdateBinderSet()
        {
            var currentIds = _currentState.Keys;
            var previousIds = _previousState.Keys;

            var addedIds = currentIds.Except(previousIds);
            var removedIds = previousIds.Except(currentIds);

            foreach (var addedId in addedIds)
            {
                var language = _currentState[addedId];
                AddBinder(language);
            }

            foreach (var removedId in removedIds)
                RemoveBinder(removedId);

            UpdateChildrenOrder();
        }

        private void UpdateChildrenOrder()
        {
            var orderedBinders = _binders.Values
                .OrderBy(b => b.Order)
                .ToList();

            for (int i = orderedBinders.Count - 1; i >= 0; i--)
            {
                var currentBinder = orderedBinders[i];

                if (!currentBinder.NeedsReorder)
                    continue;

                var previousBinder = i > 0
                    ? orderedBinders[i - 1]
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
