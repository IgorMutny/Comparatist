using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal class RootBinder
    {
        private CachedRoot _previousState;
        private WordGridRenderer _renderer;
        private bool _isExpanded;
        private Dictionary<Guid, StemBinder> _binders = new();

        public RootBinder(CachedRoot state, CategoryBinder parent, WordGridRenderer renderer)
        {
            _previousState = state;
            Parent = parent;
            _renderer = renderer;
        }

        public bool NeedsReorder { get; set; }
        public Root Root => _previousState.Record;
        public CategoryBinder Parent { get; private set; }

        public void Update(CachedRoot state)
        {
            if(_isExpanded)
                UpdateStemContents(state);

            if (state.EqualsContent(_previousState))
                return;

            if (_isExpanded)
                UpdateStems(state);

            var oldValue = _previousState.Record.Value;
            _previousState = state;
            _renderer.UpdateRoot(this);

            if (oldValue != state.Record.Value)
                NeedsReorder = true;
        }

        private void UpdateStems(CachedRoot state)
        {
            var addedStemIds = state.Stems.Keys.Except(_previousState.Stems.Keys).ToList();

            foreach (var id in addedStemIds)
            {
                var stem = state.Stems[id];
                AddBinder(stem, needsReorder: true);
            }

            var deletedStemIds = _previousState.Stems.Keys.Except(state.Stems.Keys).ToList();

            foreach (var id in deletedStemIds)
                RemoveBinder(id);

            UpdateStemOrder();
        }

        private void UpdateStemContents(CachedRoot state)
        {
            var oldStemIds = state.Stems.Keys.Intersect(_previousState.Stems.Keys).ToList();
            bool needReorder = false;

            foreach (var id in oldStemIds)
            {
                var binder = _binders[id];
                binder.Update(state.Stems[id]);

                if (binder.NeedsReorder)
                    needReorder = true;
            }

            if (needReorder)
                UpdateStemOrder();
        }

        public void ExpandOrCollapse()
        {
            if (_isExpanded)
                Collapse();
            else
                Expand();
        }

        private void Expand()
        {
            var orderedStems = _previousState.Stems.Values
                .OrderByDescending(e => e.Record.Value)
                .ToList();

            foreach (var stem in orderedStems)
                AddBinder(stem);

            _isExpanded = true;
        }

        private void Collapse()
        {
            foreach (var pair in _binders)
                RemoveBinder(pair.Key);

            _isExpanded = false;
        }

        private void AddBinder(CachedStem stem, bool needsReorder = false)
        {
            var binder = new StemBinder(stem, this, _renderer);
            binder.NeedsReorder = needsReorder;
            _binders.Add(stem.Record.Id, binder);
            _renderer.AddStem(binder, this);
        }

        private void RemoveBinder(Guid id)
        {
            var binder = _binders[id];
            _renderer.RemoveStem(binder);
            _binders.Remove(id);
        }

        private void UpdateStemOrder()
        {
            var orderedBinders = _binders.Values
                .OrderBy(b => b.Stem.Value)
                .ToList();

            for (int i = 0; i < orderedBinders.Count; i++)
            {
                var currentBinder = orderedBinders[i];

                if (!currentBinder.NeedsReorder)
                    continue;

                var previousBinder = i > 0
                    ? orderedBinders[i - 1]
                    : null;

                _renderer.MoveStem(currentBinder, previousBinder);
                currentBinder.NeedsReorder = false;
            }
        }
    }
}
