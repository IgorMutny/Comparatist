using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal class SectionBinder
    {
        private CachedCategory _previousState;
        private WordGridRenderer _renderer;
        private Dictionary<Guid, BlockBinder> _blockBinders = new();

        public SectionBinder(CachedCategory state, WordGridRenderer renderer)
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
                AddBinder(root);
            }

            var deletedRootIds = _previousState.Roots.Keys.Except(state.Roots.Keys).ToList();

            foreach (var id in deletedRootIds)
                RemoveBinder(id);

            var oldRootIds = state.Roots.Keys.Except(addedRootIds).ToList();

            foreach (var id in oldRootIds)
            {
                var binder = _blockBinders[id];
                binder.Update(state.Roots[id]);
            }

            _previousState = state;
            UpdateBlockOrder();
        }

        private void AddBinder(CachedRoot root)
        {
            var binder = new BlockBinder(root, this, _renderer);
            _blockBinders.Add(root.Record.Id, binder);
            _renderer.AddBlock(binder, this);
        }

        private void RemoveBinder(Guid id)
        {
            var binder = _blockBinders[id];
            _renderer.RemoveBlock(binder);
            _blockBinders.Remove(id);
        }

        private void UpdateBlockOrder()
        {
            var orderedBinders = _blockBinders.Values
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

                _renderer.MoveBlock(currentBinder, previousBinder);
            }
        }
    }
}
