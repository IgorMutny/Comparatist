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
            _renderer.AddSection(this);

            foreach (var e in _previousState.Roots)
            {
                AddBinder(e.Value);
            }
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
                if (!_blockBinders.TryGetValue(id, out var binder))
                    continue;

                binder.Update(state.Roots[id]);
            }

            _previousState = state;
        }

        private void AddBinder(CachedRoot root)
        {
            var binder = new BlockBinder(root, this, _renderer);
            _blockBinders.Add(root.Record.Id, binder);
            binder.Initialize();
        }

        private void RemoveBinder(Guid id)
        {
            if (!_blockBinders.TryGetValue(id, out var binder))
                return;

            binder.Destroy();
            _blockBinders.Remove(id);
        }
    }
}
