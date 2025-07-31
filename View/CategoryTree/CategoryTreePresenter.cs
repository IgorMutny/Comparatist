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
            View.EditNodeRequest += OnEditNodeRequest;
            View.MoveNodeRequest += OnMoveNodeRequest;
            View.DeleteNodeRequest += OnDeleteNodeRequest;
        }

        protected override void Unsubscribe()
        {
            View.AddNodeRequest -= OnAddNodeRequest;
            View.EditNodeRequest -= OnEditNodeRequest;
            View.MoveNodeRequest -= OnMoveNodeRequest;
            View.DeleteNodeRequest -= OnDeleteNodeRequest;
        }

        private void OnAddNodeRequest(string name, CachedCategoryNode? parent)
        {
            var parentId = parent == null ? NoParentId : parent.Category.Id;
            var newCategory = new Category() { Value = name, ParentId = parentId };
            Execute(() => Service.AddCategory(newCategory));
            Render();
        }

        private void OnEditNodeRequest(CachedCategoryNode node, string newName)
        {
            var category = node.Category;
            category.Value = newName;
            Execute(() => Service.UpdateCategory(category));
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

        private void OnDeleteNodeRequest(CachedCategoryNode node)
        {
            var category = node.Category;
            Execute(() => Service.DeleteCategory(category));
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
