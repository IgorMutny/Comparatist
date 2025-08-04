using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.Cache;
using Comparatist.Services.Exceptions;

namespace Comparatist.Services.CacheUpdate
{
    internal class StemCacheUpdater : CacheUpdater<Stem>
    {
        public StemCacheUpdater(IDatabase database, ProjectCache cache) : base(database, cache) { }

        public override void RebuildCache()
        {
            var allStems = Database.GetRepository<Stem>().GetAll();

            foreach (var stem in allStems)
                OnAdded(stem);
        }

        protected override void OnAdded(Stem stem)
        {
            var cached = new CachedStem { Record = stem };

            AddStemToCache(cached);

            foreach (var rootId in stem.RootIds)
                AddToCachedRoot(cached, rootId);
        }

        protected override void OnUpdated(Stem stem)
        {
            var cached = GetStemFromCache(stem.Id);

            var oldRootIds = cached.Record.RootIds.ToHashSet();
            var newRootIds = stem.RootIds.ToHashSet();
            cached.Record = stem;

            var addedRootIds = newRootIds.Except(oldRootIds).ToHashSet();
            var removedRootIds = oldRootIds.Except(newRootIds).ToHashSet();

            if (addedRootIds.Count != 0 || removedRootIds.Count != 0)
            {
                foreach (var rootId in addedRootIds)
                    AddToCachedRoot(cached, rootId);

                foreach (var rootId in removedRootIds)
                    RemoveFromCachedRoot(stem, rootId);
            }
        }

        protected override void OnDeleted(Stem stem)
        {
            foreach (var rootId in stem.RootIds)
                RemoveFromCachedRoot(stem, rootId);

            DeleteStemFromCache(stem.Id);
        }

        private void AddStemToCache(CachedStem cached)
        {
            var id = cached.Record.Id;

            if (Cache.Stems.ContainsKey(id))
                throw new CachedRecordAlreadyExistsException(typeof(CachedStem), id);

            Cache.Stems.Add(id, cached);
        }

        private CachedStem GetStemFromCache(Guid id)
        {
            if (!Cache.Stems.TryGetValue(id, out var cached))
                throw new CachedRecordNotFoundException(typeof(CachedStem), id);

            return cached;
        }

        private void DeleteStemFromCache(Guid id)
        {
            if (!Cache.Stems.Remove(id))
                throw new CachedRecordNotFoundException(typeof(CachedStem), id);
        }

        private void AddToCachedRoot(CachedStem cached, Guid rootId)
        {
            if (!Cache.Roots.TryGetValue(rootId, out var root))
                throw new CachedRecordNotFoundException(typeof(CachedRoot), rootId);

            root.Stems.Add(cached.Record.Id, cached);
        }

        private void RemoveFromCachedRoot(Stem stem, Guid rootId)
        {
            if (!Cache.Roots.TryGetValue(rootId, out var root))
                throw new CachedRecordNotFoundException(typeof(CachedRoot), rootId);

            root.Stems.Remove(stem.Id);
        }
    }
}
