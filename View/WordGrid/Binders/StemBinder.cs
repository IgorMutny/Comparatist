using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal class StemBinder: CompositeBinder<CachedStem, WordBinder>, IOrderableBinder
    { 
        public StemBinder(CachedStem state, RootBinder parent, WordGridRenderer renderer)
            : base (state, renderer)
        {
            Parent = parent;
        }

        public bool NeedsReorder { get; set; }
        public Stem Stem => CurrentState.Record;
        public RootBinder Parent { get; private set; }
        public string Order => CurrentState.Record.Value;
        public override Guid Id => Stem.Id;

        public override void OnCreate()
        {
            foreach (var word in CurrentState.Words.Values)
                AddChild(word);
        }

        protected override void OnUpdate()
        {
            UpdateChildrenContent(CurrentState.Words, PreviousState.Words);

            if (CurrentState.EqualsContent(PreviousState))
                return;
            
            UpdateChildrenSet(CurrentState.Words, PreviousState.Words);

            if (PreviousState?.Record.Value != CurrentState.Record.Value)
                NeedsReorder = true;

            Renderer.Update(this); 
        }

        protected override WordBinder CreateChild(ICachedRecord cached)
        {
            if (cached is not CachedWord cachedWord)
                throw new ArgumentException();

            return new WordBinder(cachedWord, this, Renderer);
        }
    }
}
