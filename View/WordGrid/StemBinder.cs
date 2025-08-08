using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal class StemBinder
    {
        private CachedStem _previousState;
        private WordGridRenderer _renderer;

        public StemBinder(CachedStem state, RootBinder parent, WordGridRenderer renderer)
        {
            _previousState = state;
            Parent = parent;
            _renderer = renderer;
        }

        public bool NeedsReorder { get; set; }
        public Stem Stem => _previousState.Record;
        public RootBinder Parent { get; private set; }

        public void Update(CachedStem state)
        {
            if (state.EqualsContent(_previousState))
                return;

            var oldValue = _previousState.Record.Value;
            _previousState = state;
            _renderer.UpdateStem(this);

            if (oldValue != state.Record.Value)
                NeedsReorder = true;
        }
    }
}
