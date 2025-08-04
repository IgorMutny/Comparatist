using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.Services.CacheUpdate
{
    internal class RootCacheUpdater : CacheUpdater<Root>
    {
        public RootCacheUpdater(IDatabase database, ProjectCache cache) : base(database, cache) { }

        public override void RebuildCache()
        {

        }

        protected override void OnAdded(Root root)
        {

        }

        protected override void OnUpdated(Root root)
        {

        }

        protected override void OnDeleted(Root root)
        {

        }
    }
}
