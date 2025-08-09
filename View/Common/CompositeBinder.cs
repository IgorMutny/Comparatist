using Comparatist.Application.Cache;
using Comparatist.View.WordGrid;

namespace Comparatist.View.Common
{
    internal abstract class CompositeBinder<TCached, TChildBinder, TRenderer>
        : Binder<TCached, TRenderer>, ICompositeBinder
        where TCached : class, ICachedRecord 
        where TChildBinder : class, IBinder
        where TRenderer: class, IRenderer
    {
        private bool _childrenCanBeReordered;

        protected CompositeBinder(TCached state, WordGridRenderer renderer)
            : base(state, renderer)
        {
            _childrenCanBeReordered = typeof(IOrderableBinder).IsAssignableFrom(typeof(TChildBinder));
        }

        protected Dictionary<Guid, TChildBinder> Children { get; private set; } = new();

        protected void AddChild(ICachedRecord root, bool needsReorder = false)
        {
            var binder = CreateChild(root);

            if (binder is IOrderableBinder orderableBinder)
                orderableBinder.NeedsReorder = needsReorder;

            Children.Add(binder.Id, binder);
            Renderer.Add(binder, this);
            binder.OnCreate();
        }

        protected void RemoveChild(Guid id)
        {
            var binder = Children[id];

            if (binder is ICompositeBinder compositeBinder)
                compositeBinder.RemoveAllChildren();

            Renderer.Remove(binder);
            Children.Remove(id);
        }

        public void RemoveAllChildren()
        {
            foreach (var id in Children.Keys)
                RemoveChild(id);
        }

        protected void UpdateChildrenSet<TChild>(
            Dictionary<Guid, TChild> currentChildren,
            Dictionary<Guid, TChild> previousChildren)
            where TChild : ICachedRecord
        {
            var addedIds = currentChildren.Keys.Except(previousChildren.Keys).ToList();

            foreach (var id in addedIds)
                AddChild(currentChildren[id], true);

            var deletedIds = previousChildren.Keys.Except(currentChildren.Keys).ToList();

            foreach (var id in deletedIds)
                RemoveChild(id);

            if (_childrenCanBeReordered)
                UpdateChildrenOrder();
        }

        protected void UpdateChildrenContent<TChild>(
            Dictionary<Guid, TChild> currentChildren,
            Dictionary<Guid, TChild> previousChildren)
            where TChild : ICachedRecord
        {
            var oldIds = currentChildren.Keys.Intersect(previousChildren.Keys).ToList();
            bool needReorder = false;

            foreach (var id in oldIds)
            {
                var binder = Children[id];
                binder.Update(currentChildren[id]);

                if (binder is IOrderableBinder orderable && orderable.NeedsReorder)
                    needReorder = true;
            }

            if (needReorder && _childrenCanBeReordered)
                UpdateChildrenOrder();
        }

        protected void UpdateChildrenOrder()
        {
            if (!_childrenCanBeReordered)
                return;

            var orderedChildren = Children.Values
                .Cast<IOrderableBinder>()
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

                Renderer.Move((TChildBinder)currentBinder, (TChildBinder?)previousBinder);
                currentBinder.NeedsReorder = false;
            }
        }

        protected abstract TChildBinder CreateChild(ICachedRecord cached);
    }
}
