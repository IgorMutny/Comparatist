using Comparatist.Data.Entities;
using Comparatist.Application.Management;
using Comparatist.View.Common;
using Comparatist.Application.Cache;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryTreePresenter : 
        Presenter<CategoryTreeRenderer, CategoryTreeInputHandler>
    {
        private Dictionary<Guid, CategoryNodeBinder> _binders = new();
        private Dictionary<Guid, CachedCategory> _previousState = new();
        private Dictionary<Guid, CachedCategory> _currentState = new();

        public CategoryTreePresenter(IProjectService service, CategoryTreeRenderer renderer, CategoryTreeInputHandler inputHandler) :
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

        private void OnAddRequest(Category category)
        {
            Execute(() => Service.Add(category));
            DrawDiff();
        }

        private void OnUpdateRequest(Category category)
        {
            Execute(() => Service.Update(category));
            DrawDiff();
        }

        private void OnUpdateManyRequest(IEnumerable<Category> categories)
        {
            Execute(() => Service.UpdateMany(categories));
            DrawDiff();
        }

        private void OnDeleteRequest(Category category)
        {
            Execute(() => Service.Delete(category));
            DrawDiff();
        }

        private void Reset()
        {
            _binders.Clear();
            _previousState.Clear();
            _currentState.Clear();
            Renderer.Reset();
        }

        private void RedrawAll()
        {
            Reset();

            var baseCategories = Execute(Service.GetCategoryTree);

            if (baseCategories == null)
            {
                Renderer.ShowError("No category cache received");
                return;
            }

            _currentState = baseCategories;
            _previousState = baseCategories;

            var orderedCategories = baseCategories.Values
                .OrderBy(e => e.Record.Order)
                .ToList();

            foreach (var category in orderedCategories)
                AddBinder(category);
        }

        private void DrawDiff()
        {
            var state = Execute(Service.GetCategoryTree);

            if (state == null)
            {
                Renderer.ShowError("No category cache received");
                return;
            }

            _currentState = state;
            UpdateBinderSet();
            UpdateBindersContent();
            _previousState = state;
        }

        private void UpdateBinderSet()
        {
            var currentIds = _currentState.Keys;
            var previousIds = _previousState.Keys;

            var addedIds = currentIds.Except(previousIds);
            var removedIds = previousIds.Except(currentIds);

            foreach (var addedId in addedIds)
            {
                var category = _currentState[addedId];
                AddBinder(category);
            }

            foreach (var removedId in removedIds)
                RemoveBinder(removedId);

            if (addedIds.Count() > 0 || removedIds.Count() > 0)
                UpdateBinderOrder();
        }

        private void UpdateBindersContent()
        {
            var oldCategoryIds = _previousState.Keys.Intersect(_currentState.Keys);
            var needsReorder = false;

            foreach (var categoryId in oldCategoryIds)
            {
                var binder = _binders[categoryId];
                binder.Update(_currentState[categoryId]);

                if (binder.NeedsReorder)
                    needsReorder = true;
            }

            if (needsReorder)
                UpdateBinderOrder();
        }

        private void UpdateBinderOrder()
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

        private void AddBinder(CachedCategory category)
        {
            var binder = new CategoryNodeBinder(category, Renderer);
            _binders.Add(binder.Id, binder);
            Renderer.Add(binder, null);
            binder.OnCreate();
        }

        private void RemoveBinder(Guid id)
        {
            var binder = _binders[id];
            Renderer.Remove(binder);
            _binders.Remove(id);
        }
    }
}
