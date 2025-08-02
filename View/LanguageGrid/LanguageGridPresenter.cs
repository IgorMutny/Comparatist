using Comparatist.Core.Records;
using Comparatist.Services.Infrastructure;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.LanguageGrid
{
    internal class LanguageGridPresenter : Presenter<LanguageGridViewAdapter>
    {
        public LanguageGridPresenter(IProjectService service, LanguageGridViewAdapter view) :
            base(service, view)
        { }

        protected override void Subscribe()
        {
            View.AddRequest += OnAddRequest;
            View.UpdateRequest += OnUpdateRequest;
            View.DeleteRequest += OnDeleteRequest;
            View.ReorderRequest += OnReorderRequest;
            UpdateView();
        }

        protected override void Unsubscribe()
        {
            View.AddRequest -= OnAddRequest;
            View.UpdateRequest -= OnUpdateRequest;
            View.DeleteRequest -= OnDeleteRequest;
            View.ReorderRequest -= OnReorderRequest;
        }

        private void OnAddRequest(Language language)
        {
            Execute(() => Service.AddLanguage(language));
            UpdateView();
        }

        private void OnUpdateRequest(Language language)
        {
            Execute(() => Service.UpdateLanguage(language));
            UpdateView();
        }

        private void OnDeleteRequest(Language language)
        {
            Execute(() => Service.DeleteLanguage(language));
            UpdateView();
        }

        private void OnReorderRequest(IEnumerable<Language> languages)
        {
            Execute(() => Service.Reorder(languages));
            UpdateView();
        }

        protected override void UpdateView()
        {
            var languages = Execute(Service.GetAllLanguages);

            if (languages != null)
                View.Render(languages.ToList());
        }
    }
}
