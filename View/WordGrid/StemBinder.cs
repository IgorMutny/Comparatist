using Comparatist.Core.Records;
using Comparatist.Services.Cache;
using System.Text;

namespace Comparatist.View.WordGrid
{
    internal class StemBinder
    {
        private CachedStem _previousState;
        private WordGridRenderer _renderer;
        private Dictionary<Guid, WordBinder> _binders = new();

        public StemBinder(CachedStem state, RootBinder parent, WordGridRenderer renderer)
        {
            _previousState = state;
            Parent = parent;
            _renderer = renderer;
        }

        public bool NeedsReorder { get; set; }
        public Stem Stem => _previousState.Record;
        public RootBinder Parent { get; private set; }

        public void Initialize()
        {
            foreach (var word in _previousState.Words.Values)
                AddBinder(word);
        }

        public void OnRemove()
        {
            foreach (var id in _binders.Keys)
                RemoveBinder(id);
        }

        public void Update(CachedStem state)
        {
            UpdateWordContents(state);

            if (state.EqualsContent(_previousState))
                return;
            
            UpdateWords(state);

            var oldValue = _previousState.Record.Value;
            _previousState = state;
            _renderer.UpdateStem(this);

            if (oldValue != state.Record.Value)
                NeedsReorder = true;
        }

        private void UpdateWords(CachedStem state)
        {
            var addedStemIds = state.Words.Keys.Except(_previousState.Words.Keys).ToList();

            foreach (var id in addedStemIds)
            {
                var stem = state.Words[id];
                AddBinder(stem);
            }

            var deletedStemIds = _previousState.Words.Keys.Except(state.Words.Keys).ToList();

            foreach (var id in deletedStemIds)
                RemoveBinder(id);
        }

        private void UpdateWordContents(CachedStem state)
        {
            var oldStemIds = state.Words.Keys.Intersect(_previousState.Words.Keys).ToList();

            foreach (var id in oldStemIds)
                if (_binders.TryGetValue(id, out var binder))
                    binder.Update(state.Words[id]);
        }

        private void AddBinder(CachedWord word)
        {
            var binder = new WordBinder(word, this, _renderer);
            _binders.Add(word.Record.Id, binder);
            _renderer.AddWord(binder, this);
        }

        private void RemoveBinder(Guid id)
        {
            var binder = _binders[id];
            _renderer.RemoveWord(binder);
            _binders.Remove(id);
        }
    }
}
