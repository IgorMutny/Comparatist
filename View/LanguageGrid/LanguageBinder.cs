using Comparatist.Application.Cache;
using Comparatist.Data.Entities;
using Comparatist.View.Common;

namespace Comparatist.View.LanguageGrid
{
    internal class LanguageBinder : 
        Binder<CachedLanguage, LanguageGridRenderer>, IOrderableBinder
    {
        public LanguageBinder(CachedLanguage state, LanguageGridRenderer renderer)
            : base(state, renderer)
        { }

        public override Guid Id => CurrentState.Record.Id;
        public Language Language => CurrentState.Record;
        public bool NeedsReorder { get; set; }

        public IComparable Order => CurrentState.Record.Order;

        protected override void OnUpdate()
        {
            if (CurrentState.EqualsContent(PreviousState))
                return;

            Renderer.Update(this);

            if (PreviousState.Record.Order != CurrentState.Record.Order)
                NeedsReorder = true;
        }
    }
}
