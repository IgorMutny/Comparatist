using Comparatist.Core.Records;
using Comparatist.Services.CategoryTree;
using Comparatist.Services.Infrastructure;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryTreePresenter : Presenter<CategoryTreeViewAdapter>
    {
        private readonly Guid NoParentId = Guid.Empty;

        public CategoryTreePresenter(IProjectService service, CategoryTreeViewAdapter view) : base(service, view) { }

        protected override void Subscribe()
        {
            View.AddNodeRequest += OnAddNodeRequest;
            View.MoveNodeRequest += OnMoveNodeRequest;
        }

        protected override void Unsubscribe()
        {
            View.AddNodeRequest -= OnAddNodeRequest;
            View.MoveNodeRequest -= OnMoveNodeRequest;
        }

        private void OnAddNodeRequest(string name)
        {
            var newCategory = new Category() { Value = name, ParentId = NoParentId };
            Execute(() => Service.AddCategory(newCategory));
            Render();
        }

        private void OnMoveNodeRequest(CachedCategoryNode source, CachedCategoryNode? target)
        {
            var category = source.Category;
            var parentId = target == null ? NoParentId : target.Category.Id;
            category.ParentId = parentId;

            Execute(() => Service.UpdateCategory(category));
            Render();
        }

        private void Render()
        {
            var nodes = Execute(Service.GetTree);

            if (nodes != null)
                View.Render(nodes.ToList());
        }
    }
}
