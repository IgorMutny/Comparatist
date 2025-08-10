using Comparatist.Application.Cache;
using Comparatist.Data.Entities;
using Comparatist.View.Common;
using System.Text;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryNodeBinder
        : Binder<CachedCategory, CategoryTreeRenderer>, IOrderableBinder
    {
        private Dictionary<Guid, CategoryNodeBinder> _children = new();

        public CategoryNodeBinder(CachedCategory state, CategoryTreeRenderer renderer)
            : base(state, renderer)
        { }

        public override Guid Id => CurrentState.Record.Id;
        public Category Category => CurrentState.Record;
        public bool NeedsReorder { get; set; }

        public IComparable Order => Category.Order;

        public override void OnCreate()
        {
            var orderedChildren = CurrentState.Children.Values
                .OrderBy(e => e.Record.Order)
                .ToList();

            foreach (var child in orderedChildren)
                AddChild(child);
        }

        protected override void OnUpdate()
        {
            UpdateChildrenContent();

            if (CurrentState.EqualsContent(PreviousState))
                return;

            UpdateChildrenSet();
            Renderer.Update(this);

            if (CurrentState.Record.Order != PreviousState.Record.Order)
                NeedsReorder = true;
        }

        private void UpdateChildrenSet()
        {
            var currentIds = CurrentState.Children.Keys;
            var previousIds = PreviousState.Children.Keys;

            var addedIds = currentIds.Except(previousIds);
            var removedIds = previousIds.Except(currentIds);

            foreach (var addedId in addedIds)
            {
                var category = CurrentState.Children[addedId];
                AddChild(category);
            }

            foreach (var removedId in removedIds)
                RemoveChild(removedId);

            if (addedIds.Count() > 0 || removedIds.Count() > 0)
                UpdateChildrenOrder();
        }

        private void UpdateChildrenContent()
        {
            var oldCategoryIds = PreviousState.Children.Keys.Intersect(CurrentState.Children.Keys);
            bool needsReorder = false;

            foreach (var categoryId in oldCategoryIds)
            {
                var binder = _children[categoryId];
                binder.Update(CurrentState.Children[categoryId]);

                if (binder.NeedsReorder)
                    needsReorder = true;

            }

            if (needsReorder)
                UpdateChildrenOrder();
        }

        private void UpdateChildrenOrder()
        {
            var orderedChildren = _children.Values
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

        private void AddChild(CachedCategory category)
        {
            var child = new CategoryNodeBinder(category, Renderer);
            _children.Add(child.Id, child);
            Renderer.Add(child, this);
            child.OnCreate();
        }

        private void RemoveChild(Guid id)
        {
            var child = _children[id];
            Renderer.Remove(child);
            _children.Remove(id);
        }
    }
}
