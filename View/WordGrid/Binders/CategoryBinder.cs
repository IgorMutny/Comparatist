using Comparatist.Data.Entities;
using Comparatist.Application.Cache;

namespace Comparatist.View.WordGrid.Binders
{
    internal class CategoryBinder :
        CompositeBinder<CachedCategory, RootBinder>
    {
        public CategoryBinder(CachedCategory state, WordGridRenderer renderer)
            : base(state, renderer) { }

        public Category Category => CurrentState.Record;
        public override Guid Id => Category.Id;

        public override void OnCreate()
        {
            foreach (var root in CurrentState.OrderedRoots.Reverse())
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
            if (Children.TryGetValue(root.Id, out var rootBinder))
                rootBinder.ExpandOrCollapse();
        }

        public void ExpandAll()
        {
            foreach(var child in Children.Values)
                child.Expand();
        }

        public void CollapseAll()
        {
			foreach(var child in Children.Values)
				child.Collapse();
		}

        protected override RootBinder CreateChild(ICachedRecord cached)
        {
            if (cached is not CachedRoot cachedRoot)
                throw new ArgumentException();

            return new RootBinder(cachedRoot, this, Renderer);
        }
    }
}
