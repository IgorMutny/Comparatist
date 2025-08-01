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
            Render();
        }

        protected override void Unsubscribe()
        {
            View.AddRequest -= OnAddRequest;
            View.UpdateRequest -= OnUpdateRequest;
            View.DeleteRequest -= OnDeleteRequest;
        }

        private void OnAddRequest(Language language)
        {
            Execute(() => Service.AddLanguage(language));
            Render();
        }

        private void OnUpdateRequest(Language language)
        {
            Execute(() => Service.UpdateLanguage(language));
            Render();
        }

        private void OnDeleteRequest(Language language)
        {
            Execute(() => Service.DeleteLanguage(language));
            Render();
        }

        private void Render()
        {
            var languages = Execute(Service.GetAllLanguages);

            if (languages != null)
                View.Render(languages.ToList());
        }
    }
}
