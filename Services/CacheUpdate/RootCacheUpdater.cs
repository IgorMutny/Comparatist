using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.Cache;
using Comparatist.Services.Exceptions;

namespace Comparatist.Services.CacheUpdate
{
    internal class RootCacheUpdater : CacheUpdater<Root>
    {
        public RootCacheUpdater(IDatabase database, ProjectCache cache) : base(database, cache) { }

        public override void RebuildCache()
        {
            var allRoots = Database.GetRepository<Root>().GetAll();

            foreach (var root in allRoots)
                OnAdded(root);
        }

        protected override void OnAdded(Root root)
        {
            var cached = new CachedRoot { Record = root };

            AddRootToCache(cached);
            UpdateUncategorizedRootIds(root);

            foreach (var categoryId in root.CategoryIds)
                AddToCachedCategory(cached, categoryId);
        }

        protected override void OnUpdated(Root root)
        {
            var cached = GetRootFromCache(root.Id);

            var oldCategoryIds = cached.Record.CategoryIds.ToHashSet();
            var newCategoryIds = root.CategoryIds.ToHashSet();
            cached.Record = root;

            var addedCategoryIds = newCategoryIds.Except(oldCategoryIds).ToHashSet();
            var removedCategoryIds = oldCategoryIds.Except(newCategoryIds).ToHashSet();

            if (addedCategoryIds.Count != 0 || removedCategoryIds.Count != 0)
            {
                UpdateUncategorizedRootIds(root);

                foreach (var categoryId in addedCategoryIds)
                    AddToCachedCategory(cached, categoryId);

                foreach (var categoryId in removedCategoryIds)
                    RemoveFromCachedCategory(root, categoryId);
            }
        }

        protected override void OnDeleted(Root root)
        {
            Cache.UncategorizedRootIds.Remove(root.Id);

            foreach (var categoryId in root.CategoryIds)
                RemoveFromCachedCategory(root, categoryId);

            DeleteRootFromCache(root.Id);
        }

        private void AddRootToCache(CachedRoot cached)
        {
            var id = cached.Record.Id;

            if (Cache.Roots.ContainsKey(id))
                throw new CachedRecordAlreadyExistsException(typeof(CachedRoot), id);

            Cache.Roots.Add(id, cached);
        }

        private CachedRoot GetRootFromCache(Guid id)
        {
            if (!Cache.Roots.TryGetValue(id, out var cached))
                throw new CachedRecordNotFoundException(typeof(CachedRoot), id);

            return cached;
        }

        private void DeleteRootFromCache(Guid id)
        {
            if (!Cache.Roots.Remove(id))
                throw new CachedRecordNotFoundException(typeof(CachedRoot), id);
        }

        private void AddToCachedCategory(CachedRoot cached, Guid categoryId)
        {
            if (!Cache.Categories.TryGetValue(categoryId, out var category))
                throw new CachedRecordNotFoundException(typeof(CachedCategory), categoryId);

            category.Roots.Add(cached.Record.Id, cached);
        }

        private void RemoveFromCachedCategory(Root root, Guid categoryId)
        {
            if (!Cache.Categories.TryGetValue(categoryId, out var category))
                throw new CachedRecordNotFoundException(typeof(CachedCategory), categoryId);

            category.Roots.Remove(root.Id);
        }

        private void UpdateUncategorizedRootIds(Root root)
        {
            if (root.CategoryIds.Count == 0)
                Cache.UncategorizedRootIds.Add(root.Id);
            else
                Cache.UncategorizedRootIds.Remove(root.Id);
        }
    }
}
