using Comparatist.Core.Exceptions;
using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.Cache;
using Comparatist.Services.Infrastructure;

namespace Comparatist.Services.CacheUpdate
{
    internal class CategoryCacheUpdater : IInitializable, IDisposable
    {
        private IDatabase _database;
        private ProjectCache _cache;

        public CategoryCacheUpdater(IDatabase database, ProjectCache cache)
        {
            _database = database;
            _cache = cache;
        }

        public void Initialize()
        {
            var categoryRepo = _database.GetRepository<Category>();
            categoryRepo.RecordAdded += OnCategoryAdded;
            categoryRepo.RecordUpdated += OnCategoryUpdated;
            categoryRepo.RecordDeleted += OnCategoryDeleted;
        }

        public void Dispose()
        {
            var categoryRepo = _database.GetRepository<Category>();
            categoryRepo.RecordAdded -= OnCategoryAdded;
            categoryRepo.RecordUpdated -= OnCategoryUpdated;
            categoryRepo.RecordDeleted -= OnCategoryDeleted;
        }

        public void RebuildCache()
        {
            _cache.Categories.Clear();
            _cache.BaseCategoryIds.Clear();

            var allCategories = _database.GetRepository<Category>().GetAll();

            foreach (var category in allCategories)
            {
                var cached = new CachedCategory { Record = category };
                AddCategoryToCache(cached);
            }

            foreach (var cached in _cache.Categories.Values)
            {
                UpdateBaseCategoryIds(cached.Record);
                AddToCachedParent(cached, cached.Record.ParentId);
            }
        }

        private void OnCategoryAdded(Category category)
        {
            var cached = new CachedCategory { Record = category };
            UpdateBaseCategoryIds(category);
            AddCategoryToCache(cached);
            AddToCachedParent(cached, category.ParentId);
        }

        private void OnCategoryUpdated(Category category)
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

        private void OnCategoryDeleted(Category category)
        {
            var parentId = category.ParentId;
            _cache.BaseCategoryIds.Remove(category.Id);
            RemoveFromCachedParent(category, category.ParentId);
            DeleteCategoryFromCache(category.Id);
        }

        private void AddCategoryToCache(CachedCategory cached)
        {
            var id = cached.Record.Id;

            if (_cache.Categories.ContainsKey(id))
                throw new CachedRecordAlreadyExistsException(typeof(CachedCategory), id);

            _cache.Categories.Add(id, cached);
        }

        private CachedCategory GetCategoryFromCache(Guid id)
        {
            if (!_cache.Categories.TryGetValue(id, out var cached))
                throw new CachedRecordNotFoundException(typeof(CachedCategory), id);

            return cached;
        }

        private void DeleteCategoryFromCache(Guid id)
        {
            if (!_cache.Categories.Remove(id))
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
            if (parentId != Guid.Empty && _cache.Categories.TryGetValue(parentId, out var parentCached))
                parentCached.Children.Remove(category.Id);
        }

        private void UpdateBaseCategoryIds(Category category)
        {
            if (category.ParentId == Guid.Empty)
                _cache.BaseCategoryIds.Add(category.Id);
            else
                _cache.BaseCategoryIds.Remove(category.Id);
        }
    }
}
