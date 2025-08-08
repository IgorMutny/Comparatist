using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.Cache;
using Comparatist.Services.Exceptions;

namespace Comparatist.Services.CacheUpdate
{
    internal class WordCacheUpdater: CacheUpdater<Word>
    {
        public WordCacheUpdater(IDatabase database, ProjectCache cache) : base(database, cache) { }

        public override void RebuildCache()
        {
            var allWords = Database.GetRepository<Word>().GetAll();

            foreach (var word in allWords)
                OnAdded(word);
        }

        protected override void OnAdded(Word word)
        {
            var cached = new CachedWord { Record = word };

            if(Cache.Words.ContainsKey(word.Id))
                throw new CachedRecordAlreadyExistsException(typeof(CachedWord), word.Id);

            Cache.Words.Add(word.Id, cached);

            if (!Cache.Stems.TryGetValue(word.StemId, out var stem))
                throw new CachedRecordNotFoundException(typeof(CachedStem), word.StemId);

            stem.Words.Add(word.Id, cached);
        }

        protected override void OnUpdated(Word word)
        {
            if (!Cache.Words.TryGetValue(word.Id, out var cached))
                throw new CachedRecordNotFoundException(typeof(CachedWord), word.Id);

            cached.Record = word;
        }

        protected override void OnDeleted(Word word)
        {
            if (!Cache.Stems.TryGetValue(word.StemId, out var stem))
                throw new CachedRecordNotFoundException(typeof(CachedStem), word.StemId);

            stem.Words.Remove(word.Id);

            if (!Cache.Words.Remove(word.Id))
                throw new CachedRecordNotFoundException(typeof(CachedWord), word.Id);
        }
    }
}
