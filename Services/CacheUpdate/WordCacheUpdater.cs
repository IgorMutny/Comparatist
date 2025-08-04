using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.Services.CacheUpdate
{
    internal class WordCacheUpdater: CacheUpdater<Word>
    {
        public WordCacheUpdater(IDatabase database, ProjectCache cache) : base(database, cache) { }

        public override void RebuildCache()
        {

        }

        protected override void OnAdded(Word word)
        {

        }

        protected override void OnUpdated(Word word)
        {

        }

        protected override void OnDeleted(Word word)
        {

        }
    }
}
