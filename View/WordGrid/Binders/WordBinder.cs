using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal class WordBinder : Binder<CachedWord>
    { 
        public WordBinder(CachedWord state, StemBinder parent, WordGridRenderer renderer)
            : base(state, renderer)
        {
            Parent = parent;
        }

        public Word Word => CurrentState.Record;
        public StemBinder Parent { get; private set; }
        public override Guid Id => Word.Id; 

        protected override void OnUpdate()
        {
            if (CurrentState.EqualsContent(PreviousState))
                return;

            Renderer.Update(this);
        }
    }
}
