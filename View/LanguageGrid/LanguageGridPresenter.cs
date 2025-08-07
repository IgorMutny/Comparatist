using Comparatist.Core.Records;
using Comparatist.Services.Cache;
using Comparatist.Services.Infrastructure;
using Comparatist.View.Infrastructure;
using System.Text;

namespace Comparatist.View.LanguageGrid
{
    internal class LanguageGridPresenter :
        Presenter<LanguageGridRenderer, LanguageGridInputHandler, DataGridView>
    {
        private Dictionary<Guid, CachedLanguage>? _previousLanguages;

        public LanguageGridPresenter(
            IProjectService service,
            LanguageGridRenderer renderer,
            LanguageGridInputHandler inputHandler) :
            base(service, renderer, inputHandler)
        { }

        protected override void Subscribe()
        {
            InputHandler.AddRequest += OnAddRequest;
            InputHandler.UpdateRequest += OnUpdateRequest;
            InputHandler.UpdateManyRequest += OnUpdateManyRequest;
            InputHandler.DeleteRequest += OnDeleteRequest;
        }

        protected override void Unsubscribe()
        {
            InputHandler.AddRequest -= OnAddRequest;
            InputHandler.UpdateRequest -= OnUpdateRequest;
            InputHandler.UpdateManyRequest -= OnUpdateManyRequest;
            InputHandler.DeleteRequest -= OnDeleteRequest;
        }

        protected override void OnShow()
        {
            RedrawAll();
        }

        protected override void OnHide()
        {
            Reset();
        }

        private void Reset()
        {
            _previousLanguages?.Clear();
            Renderer.Reset();
        }

         private void OnAddRequest(Language language)
        {
            Execute(() => Service.Add(language));
            DrawDiff();
        }

        private void OnUpdateRequest(Language language)
        {
            Execute(() => Service.Update(language));
            DrawDiff();
        }

        private void OnUpdateManyRequest(IEnumerable<Language> languages)
        {
            Execute(() => Service.UpdateMany(languages));
            DrawDiff();
        }

        private void OnDeleteRequest(Language language)
        {
            Execute(() => Service.Delete(language));
            DrawDiff();
        }

        private void RedrawAll()
        {
            Reset();

            var languages = Execute(Service.GetAllLanguages);

            if (languages == null)
            {
                Renderer.ShowError("No language cache received");
                return;
            }

            var orderedLanguages = languages.Values.OrderBy(e => e.Record.Order).ToList();
            foreach (var language in orderedLanguages)
                Renderer.Add(language);

            _previousLanguages = languages.ToDictionary(
                pair => pair.Key,
                pair => (CachedLanguage)pair.Value.Clone());
        }

        private void DrawDiff()
        {
            var languages = Execute(Service.GetAllLanguages);

            if (languages == null)
            {
                Renderer.ShowError("No language cache received");
                return;
            }

            var currentIds = languages.Keys.ToList();
            var previousIds = _previousLanguages!.Keys.ToList();

            var orderedIds = languages
                .Select(pair => new KeyValuePair<int, Guid>(
                    pair.Value.Record.Order,
                    pair.Key))
                .ToDictionary();

            var addedIds = currentIds.Except(previousIds);

            foreach (var addedId in addedIds)
            {
                var language = languages[addedId];
                Renderer.Add(language);
            }

            var removedIds = previousIds.Except(currentIds);

            foreach (var removedId in removedIds)
                Renderer.Remove(removedId);

            foreach (var pair in languages)
            {
                if (_previousLanguages.TryGetValue(pair.Key, out var previousLanguage))
                {
                    if (previousLanguage.Record.Value != pair.Value.Record.Value)
                        Renderer.Update(pair.Key, pair.Value);

                    if (previousLanguage.Record.Order != pair.Value.Record.Order)
                    {
                        if (!orderedIds.TryGetValue(pair.Value.Record.Order + 1, out var nextId))
                            nextId = Guid.Empty;

                        Renderer.Move(pair.Key, nextId);
                    }
                }
            }

            _previousLanguages = languages;
        }
    }
}
