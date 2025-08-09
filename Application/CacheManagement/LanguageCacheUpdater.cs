using Comparatist.Data.Persistence;
using Comparatist.Data.Entities;
using Comparatist.Application.Cache;
using Comparatist.Application.Exceptions;

namespace Comparatist.Application.CacheManagement
{
    internal class LanguageCacheUpdater : CacheUpdater<Language>
    {
        public LanguageCacheUpdater(IDatabase database, ProjectCache cache) : base(database, cache) { }

        public override void RebuildCache()
        {
            var languages = Database.GetRepository<Language>().GetAll();

            foreach (var language in languages)
                OnAdded(language);
        }

        protected override void OnAdded(Language language)
        {
            if (Cache.Languages.ContainsKey(language.Id))
                throw new CachedRecordAlreadyExistsException(typeof(Language), language.Id);

            var cached = new CachedLanguage { Record = language };
            Cache.Languages.Add(language.Id, cached);
        }

        protected override void OnUpdated(Language language)
        {
            if (!Cache.Languages.ContainsKey(language.Id))
                throw new CachedRecordNotFoundException(typeof(Language), language.Id);

            var cached = Cache.Languages[language.Id];
            cached.Record = language;
        }

        protected override void OnDeleted(Language language)
        {
            if (!Cache.Languages.Remove(language.Id))
                throw new CachedRecordNotFoundException(typeof(Language), language.Id);
        }
    }
}
