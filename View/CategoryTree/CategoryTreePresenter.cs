using Comparatist.Core.Records;
using Comparatist.Services.Infrastructure;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryTreePresenter : Presenter<CategoryTreeViewAdapter, TreeView>
    {
        public CategoryTreePresenter(IProjectService service, CategoryTreeViewAdapter view) :
            base(service, view)
        { }

        protected override void Subscribe()
        {
            View.AddRequest += OnAddRequest;
            View.UpdateRequest += OnUpdateRequest;
            View.DeleteRequest += OnDeleteRequest;
            UpdateView();
        }

        protected override void Unsubscribe()
        {
            View.AddRequest -= OnAddRequest;
            View.UpdateRequest -= OnUpdateRequest;
            View.DeleteRequest -= OnDeleteRequest;
        }

        private void OnAddRequest(Category category)
        {
            Execute(() => Service.AddCategory(category));
            UpdateView();
        }

        private void OnUpdateRequest(Category category)
        {
            Execute(() => Service.UpdateCategory(category));
            UpdateView();
        }

        private void OnDeleteRequest(Category category)
        {
            Execute(() => Service.DeleteCategory(category));
            UpdateView();
        }

        protected override void UpdateView()
        {
            var nodes = Execute(Service.GetTree);

            if (nodes != null)
                View.Render(nodes.ToList());
        }
    }
}
