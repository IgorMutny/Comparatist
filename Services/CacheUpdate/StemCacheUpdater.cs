using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.Services.CacheUpdate
{
    internal class StemCacheUpdater : CacheUpdater<Stem>
    {
        public StemCacheUpdater(IDatabase database, ProjectCache cache) : base(database, cache) { }

        public override void RebuildCache()
        {

        }

        protected override void OnAdded(Stem stem)
        {

        }

        protected override void OnUpdated(Stem stem)
        {

        }

        protected override void OnDeleted(Stem stem)
        {

        }
    }
}
