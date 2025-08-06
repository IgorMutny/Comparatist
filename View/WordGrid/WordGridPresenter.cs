using Comparatist.Core.Records;
using Comparatist.Services.Infrastructure;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.WordGrid
{
    internal class WordGridPresenter : Presenter_old<WordGridViewAdapter>
    {
        public WordGridPresenter(IProjectService service, WordGridViewAdapter view) :
            base(service, view)
        { }

        protected override void Subscribe()
        {
            View.AddRootRequest += OnAddRootRequest;
            View.UpdateRootRequest += OnUpdateRootRequest;
            View.DeleteRootRequest += OnDeleteRootRequest;
            View.AddStemRequest += OnAddStemRequest;
            View.UpdateStemRequest += OnUpdateStemRequest;
            View.DeleteStemRequest += OnDeleteStemRequest;
            View.AddWordRequest += OnAddWordRequest;
            View.UpdateWordRequest += OnUpdateWordRequest;
            View.DeleteWordRequest += OnDeleteWordRequest;
        }

        protected override void Unsubscribe()
        {
            View.AddRootRequest -= OnAddRootRequest;
            View.UpdateRootRequest -= OnUpdateRootRequest;
            View.DeleteRootRequest -= OnDeleteRootRequest;
            View.AddStemRequest -= OnAddStemRequest;
            View.UpdateStemRequest -= OnUpdateStemRequest;
            View.DeleteStemRequest -= OnDeleteStemRequest;
            View.AddWordRequest -= OnAddWordRequest;
            View.UpdateWordRequest -= OnUpdateWordRequest;
            View.DeleteWordRequest -= OnDeleteWordRequest;
        }

        private void OnAddRootRequest(Root root)
        {
            Execute(() => Service.Add(root));
            UpdateView();
        }

        private void OnUpdateRootRequest(Root root)
        {
            Execute(() => Service.Update(root));
            UpdateView();
        }

        private void OnDeleteRootRequest(Root root)
        {
            Execute(() => Service.Delete(root));
            UpdateView();
        }

        private void OnAddStemRequest(Stem stem)
        {
            Execute(() => Service.Add(stem));
            UpdateView();
        }

        private void OnUpdateStemRequest(Stem stem)
        {
            Execute(() => Service.Update(stem));
            UpdateView();
        }

        private void OnDeleteStemRequest(Stem stem)
        {
            Execute(() => Service.Delete(stem));
            UpdateView();
        }

        private void OnAddWordRequest(Word word)
        {
            Execute(() => Service.Add(word));
            UpdateView();
        }

        private void OnUpdateWordRequest(Word word)
        {
            Execute(() => Service.Update(word));
            UpdateView();
        }

        private void OnDeleteWordRequest(Word word)
        {
            Execute(() => Service.Delete(word));
            UpdateView();
        }

        protected override void UpdateView()
        {
            var sections = Execute(() => Service.GetWordTable(View.SortingType));
            var languages = Execute(Service.GetAllLanguages);
            var categories = Execute(Service.GetAllCategories);

            if (sections != null && languages != null && categories != null)
            {
                View.AllCategories = categories.Select(e => e.Record);
                View.AllLanguages = languages.Select(e => e.Value.Record);
                View.Render(sections);
            }
        }
    }
}
