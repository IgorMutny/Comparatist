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
            View.UpdateManyRequest += OnUpdateManyRequest;
            View.DeleteRequest += OnDeleteRequest;
            UpdateView();
        }

        protected override void Unsubscribe()
        {
            View.AddRequest -= OnAddRequest;
            View.UpdateRequest -= OnUpdateRequest;
            View.UpdateManyRequest -= OnUpdateManyRequest;
            View.DeleteRequest -= OnDeleteRequest;
        }

        private void OnAddRequest(Language language)
        {
            Execute(() => Service.Add(language));
            UpdateView();
        }

        private void OnUpdateRequest(Language language)
        {
            Execute(() => Service.Update(language));
            UpdateView();
        }

        private void OnUpdateManyRequest(IEnumerable<Language> languages)
        {
            Execute(() => Service.UpdateMany(languages));
            UpdateView();
        }

        private void OnDeleteRequest(Language language)
        {
            Execute(() => Service.Delete(language));
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
