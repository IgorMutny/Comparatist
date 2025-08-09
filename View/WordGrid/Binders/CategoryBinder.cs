using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal class CategoryBinder : CompositeBinder<CachedCategory, RootBinder>
    {
        public CategoryBinder(CachedCategory state, WordGridRenderer renderer)
            : base(state, renderer) { }

        public Category Category => CurrentState.Record;
        public override Guid Id => Category.Id;

        public override void OnCreate()
        {
            var orderedRoots = CurrentState.Roots.Values
                .OrderByDescending(e => e.Record.Value)
                .ToList();

            foreach (var root in orderedRoots)
                AddChild(root);
        }

        protected override void OnUpdate()
        {
            UpdateChildrenContent(CurrentState.Roots, PreviousState.Roots);

            if (CurrentState.EqualsContent(PreviousState))
                return;

            UpdateChildrenSet(CurrentState.Roots, PreviousState.Roots);
        }

        public void ExpandOrCollapse(Root root)
        {
            Children[root.Id].ExpandOrCollapse();
        }

        protected override RootBinder CreateChild(ICachedRecord cached)
        {
            if (cached is not CachedRoot cachedRoot)
                throw new ArgumentException();

            return new RootBinder(cachedRoot, this, Renderer);
        }
    }
}
