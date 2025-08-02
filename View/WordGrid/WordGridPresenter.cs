using Comparatist.Core.Records;
using Comparatist.Services.Infrastructure;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.WordGrid
{
    internal class WordGridPresenter : Presenter<WordGridViewAdapter, DataGridView>
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
            Execute(() => Service.AddRoot(root));
            UpdateView();
        }

        private void OnUpdateRootRequest(Root root)
        {
            Execute(() => Service.UpdateRoot(root));
            UpdateView();
        }

        private void OnDeleteRootRequest(Root root)
        {
            Execute(() => Service.DeleteRoot(root));
            UpdateView();
        }

        private void OnAddStemRequest(Stem stem)
        {
            Execute(() => Service.AddStem(stem));
            UpdateView();
        }

        private void OnUpdateStemRequest(Stem stem)
        {
            Execute(() => Service.UpdateStem(stem));
            UpdateView();
        }

        private void OnDeleteStemRequest(Stem stem)
        {
            Execute(() => Service.DeleteStem(stem));
            UpdateView();
        }

        private void OnAddWordRequest(Word word)
        {
            Execute(() => Service.AddWord(word));
            UpdateView();
        }

        private void OnUpdateWordRequest(Word word)
        {
            Execute(() => Service.UpdateWord(word));
            UpdateView();
        }

        private void OnDeleteWordRequest(Word word)
        {
            Execute(() => Service.DeleteWord(word));
            UpdateView();
        }

        protected override void UpdateView()
        {
            var blocks = Execute(Service.GetAllBlocksByAlphabet);
            var languages = Execute(Service.GetAllLanguages);
            var categories = Execute(Service.GetAllCategories);

            if (blocks != null && languages != null && categories != null)
            {
                View.AllCategories = categories;
                View.AllRoots = blocks.Select(x => x.Root);
                View.AllLanguages = languages;
                View.Render(blocks);
            }
        }
    }
}
