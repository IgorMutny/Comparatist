using Comparatist.Data.Entities;
using Comparatist.Application.Cache;
using Comparatist.View.Common;

namespace Comparatist.View.WordGrid.Binders
{
    internal class RootBinder :
        CompositeBinder<CachedRoot, StemBinder>, IOrderableBinder
    { 
        private bool _isExpanded;

        public RootBinder(CachedRoot state, CategoryBinder parent, WordGridRenderer renderer)
            : base(state, renderer)
        {
            Parent = parent;
        }

        public bool NeedsReorder { get; set; }
        public Root Root => CurrentState.Record;
        public CategoryBinder Parent { get; private set; }
        public string ExpandedMark => _isExpanded ? "▼" : "▶";
        public IComparable Order => CurrentState.Record.Value;
        public override Guid Id => Root.Id;

        protected override void OnUpdate()
        {
            if (_isExpanded)
                UpdateChildrenContent(CurrentState.Stems, PreviousState.Stems);

            if (CurrentState.EqualsContent(PreviousState))
                return;

            if (_isExpanded)
                UpdateChildrenSet(CurrentState.Stems, PreviousState.Stems);

            Renderer.Update(this);

            if (PreviousState?.Record.Value != CurrentState.Record.Value)
                NeedsReorder = true;
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
            var orderedStems = CurrentState.Stems.Values
                .OrderByDescending(e => e.Record.Value)
                .ToList();

            foreach (var stem in orderedStems)
                AddChild(stem);

            _isExpanded = true;
            Renderer.Update(this);
        }

        private void Collapse()
        {
            RemoveAllChildren();
            _isExpanded = false;
            Renderer.Update(this);
        }

        protected override StemBinder CreateChild(ICachedRecord cached)
        {
            if (cached is not CachedStem cachedStem)
                throw new ArgumentException();

            return new StemBinder(cachedStem, this, Renderer);
        }
    }
}
