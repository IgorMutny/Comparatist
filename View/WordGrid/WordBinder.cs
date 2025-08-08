using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal class WordBinder
    {
        private CachedWord _previousState;
        private WordGridRenderer _renderer;

        public WordBinder(CachedWord state, StemBinder parent, WordGridRenderer renderer)
        {
            _previousState = state;
            Parent = parent;
            _renderer = renderer;
        }

        public Word Word => _previousState.Record;
        public StemBinder Parent { get; private set; }

        public void Update(CachedWord state)
        {
            if (state.EqualsContent(_previousState))
                return;

            _previousState = state;
            _renderer.UpdateWord(this);
        }
    }
}
