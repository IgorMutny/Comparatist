using Comparatist.Services.Exceptions;
using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.Services.CacheUpdate
{
    internal class CategoryCacheUpdater : CacheUpdater<Category>
    {
        public CategoryCacheUpdater(IDatabase database, ProjectCache cache) : base(database, cache) { }

        public override void RebuildCache()
        {
            Cache.Categories.Clear();
            Cache.BaseCategoryIds.Clear();

            var allCategories = Database.GetRepository<Category>().GetAll();

            foreach (var category in allCategories)
            {
                var cached = new CachedCategory { Record = category };
                AddCategoryToCache(cached);
            }

            foreach (var cached in Cache.Categories.Values)
            {
                UpdateBaseCategoryIds(cached.Record);
                AddToCachedParent(cached, cached.Record.ParentId);
            }
        }

        protected override void OnAdded(Category category)
        {
            var cached = new CachedCategory { Record = category };
            UpdateBaseCategoryIds(category);
            AddCategoryToCache(cached);
            AddToCachedParent(cached, category.ParentId);
        }

        protected override void OnUpdated(Category category)
        {
            var cached = GetCategoryFromCache(category.Id);
            var oldParentId = cached.Record.ParentId;
            var newParentId = category.ParentId;
            cached.Record = category;

            if (oldParentId != newParentId)
            {
                UpdateBaseCategoryIds(category);
                RemoveFromCachedParent(category, oldParentId);
                AddToCachedParent(cached, newParentId);
            }
        }

        protected override void OnDeleted(Category category)
        {
            var parentId = category.ParentId;
            Cache.BaseCategoryIds.Remove(category.Id);
            RemoveFromCachedParent(category, category.ParentId);
            DeleteCategoryFromCache(category.Id);
        }

        private void AddCategoryToCache(CachedCategory cached)
        {
            var id = cached.Record.Id;

            if (Cache.Categories.ContainsKey(id))
                throw new CachedRecordAlreadyExistsException(typeof(CachedCategory), id);

            Cache.Categories.Add(id, cached);
        }

        private CachedCategory GetCategoryFromCache(Guid id)
        {
            if (!Cache.Categories.TryGetValue(id, out var cached))
                throw new CachedRecordNotFoundException(typeof(CachedCategory), id);

            return cached;
        }

        private void DeleteCategoryFromCache(Guid id)
        {
            if (!Cache.Categories.Remove(id))
                throw new CachedRecordNotFoundException(typeof(CachedCategory), id);
        }

        private void AddToCachedParent(CachedCategory cached, Guid parentId)
        {
            if (parentId == Guid.Empty)
                return;

            var parentCached = GetCategoryFromCache(parentId);
            parentCached.Children.Add(cached.Record.Id, cached);
        }

        private void RemoveFromCachedParent(Category category, Guid parentId)
        {
            if (parentId != Guid.Empty && Cache.Categories.TryGetValue(parentId, out var parentCached))
                parentCached.Children.Remove(category.Id);
        }

        private void UpdateBaseCategoryIds(Category category)
        {
            if (category.ParentId == Guid.Empty)
                Cache.BaseCategoryIds.Add(category.Id);
            else
                Cache.BaseCategoryIds.Remove(category.Id);
        }
    }
}
