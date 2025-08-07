using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal class BlockBinder
    {
        private CachedRoot _previousState;
        private WordGridRenderer _renderer;

        public BlockBinder(CachedRoot state, SectionBinder parent, WordGridRenderer renderer)
        {
            _previousState = state;
            Parent = parent;
            _renderer = renderer;
        }

        public bool NeedsReorder { get; private set; }
        public Root Root => _previousState.Record;
        public SectionBinder Parent { get; private set; }

        public void Update(CachedRoot state)
        {
            if (state.Record.EqualsContent(_previousState.Record))
                return;

            var oldValue = _previousState.Record.Value;
            _previousState = state;
            _renderer.UpdateBlock(this);

            if (oldValue != state.Record.Value)
                NeedsReorder = true;
        }

        public void ClearReorderFlag()
        {
            NeedsReorder = false;
        }
    }
}
