using Comparatist.Services.Cache;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.WordGrid
{
    internal class SectionBinder
    {
        public Guid Id { get; private set; }
        private CachedCategory _previousState;
        private WordGridRenderer _renderer;

        public SectionBinder(CachedCategory state, WordGridRenderer renderer)
        {
            Id = state.Record.Id;
            _previousState = state;
            _renderer = renderer;
        }

        public void Initialize()
        {
            _renderer.AddSection(_previousState);
        }
    }
}
