using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal class BlockBinder
    {
        private CachedRoot _previousState;
        private WordGridRenderer _renderer;
        private SectionBinder _parent;

        public BlockBinder(CachedRoot state, SectionBinder parent, WordGridRenderer renderer)
        {
            _previousState = state;
            _parent = parent;
            _renderer = renderer;
        }

        public Root Root => _previousState.Record;

        public void Initialize()
        {
            _renderer.AddBlock(this, _parent);
        }

        public void Update(CachedRoot state)
        {
            if (state.Record.EqualsContent(_previousState.Record))
                return;

            _previousState = state;
            _renderer.UpdateBlock(this);
        }

        public void Destroy()
        {
            _renderer.RemoveBlock(this);
        }
    }
}
