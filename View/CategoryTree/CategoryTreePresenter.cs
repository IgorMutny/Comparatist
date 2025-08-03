using Comparatist.Core.Records;
using Comparatist.Services.Infrastructure;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryTreePresenter : Presenter<CategoryTreeViewAdapter>
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
            Execute(() => Service.Add(category));
            UpdateView();
        }

        private void OnUpdateRequest(Category category)
        {
            Execute(() => Service.Update(category));
            UpdateView();
        }

        private void OnDeleteRequest(Category category)
        {
            Execute(() => Service.Delete(category));
            UpdateView();
        }

        protected override void UpdateView()
        {
            var nodes = Execute(Service.GetCategoryTree);

            if (nodes != null)
                View.Render(nodes.ToList());
        }
    }
}
