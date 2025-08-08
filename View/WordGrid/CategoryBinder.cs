using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal class CategoryBinder
    {
        private CachedCategory _previousState;
        private WordGridRenderer _renderer;
        private Dictionary<Guid, RootBinder> _binders = new();

        public CategoryBinder(CachedCategory state, WordGridRenderer renderer)
        {
            _previousState = state;
            _renderer = renderer;
        }

        public Category Category => _previousState.Record;

        public void Initialize()
        {
            var orderedRoots = _previousState.Roots.Values
                .OrderByDescending(e => e.Record.Value)
                .ToList();

            foreach (var root in orderedRoots)
                AddBinder(root);
        }

        public void Update(CachedCategory state)
        {
            var addedRootIds = state.Roots.Keys.Except(_previousState.Roots.Keys).ToList();

            foreach (var id in addedRootIds)
            {
                var root = state.Roots[id];
                AddBinder(root, needsReorder: true);
            }

            var deletedRootIds = _previousState.Roots.Keys.Except(state.Roots.Keys).ToList();

            foreach (var id in deletedRootIds)
                RemoveBinder(id);

            var oldRootIds = state.Roots.Keys.Except(addedRootIds).ToList();

            foreach (var id in oldRootIds)
            {
                var binder = _binders[id];
                binder.Update(state.Roots[id]);
            }

            _previousState = state;
            UpdateRootOrder();
        }

        public void ExpandOrCollapse(Root root)
        {
            if (!_binders.TryGetValue(root.Id, out var binder))
                return;

            binder.ExpandOrCollapse();
        }

        private void AddBinder(CachedRoot root, bool needsReorder = false)
        {
            var binder = new RootBinder(root, this, _renderer);
            binder.NeedsReorder = needsReorder;
            _binders.Add(root.Record.Id, binder);
            _renderer.AddRoot(binder, this);
        }

        private void RemoveBinder(Guid id)
        {
            var binder = _binders[id];
            binder.OnRemove();
            _renderer.RemoveRoot(binder);
            _binders.Remove(id);
        }

        private void UpdateRootOrder()
        {
            var orderedBinders = _binders.Values
                .OrderBy(b => b.Root.Value)
                .ToList();

            for (int i = 0; i < orderedBinders.Count; i++)
            {
                var currentBinder = orderedBinders[i];

                if (!currentBinder.NeedsReorder)
                    continue;

                var previousBinder = i > 0
                    ? orderedBinders[i - 1]
                    : null;

                _renderer.MoveRoot(currentBinder, previousBinder);
                currentBinder.NeedsReorder = false;
            }
        }
    }
}
