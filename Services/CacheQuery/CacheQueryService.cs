using Comparatist.Services.Exceptions;
using Comparatist.Services.Cache;

namespace Comparatist.Services.CacheQuery
{
    internal class CacheQueryService
    {
        private ProjectCache _cache;

        public CacheQueryService(ProjectCache cache)
        {
            _cache = cache;
        }

        public IEnumerable<CachedLanguage> GetAllLanguages()
        {
            return _cache.Languages
                .Select(pair => new KeyValuePair<Guid, CachedLanguage>(
                    pair.Key, 
                    (CachedLanguage)pair.Value.Clone()))
                .OrderBy(e => e.Value.Record.Order)
                .ToDictionary()
                .Values;
        }

        public IEnumerable<CachedCategory> GetCategoryTree()
        {
            var result = new Dictionary<Guid, CachedCategory>();

            foreach (var id in _cache.BaseCategoryIds)
            {
                if (!_cache.Categories.TryGetValue(id, out var cached))
                    throw new CachedRecordNotFoundException(typeof(CachedCategory), id);

                result.Add(id, (CachedCategory)cached.Clone());
            }

            result = result.OrderBy(pair => pair.Value.Record.Order).ToDictionary();

            foreach (var e in result)
                ReorderCategoryChildren(e.Value);

            return result.Values;
        }

        private void ReorderCategoryChildren(CachedCategory category)
        {
            category.Children = category.Children.OrderBy(e => e.Value.Record.Order).ToDictionary();

            foreach (var child in category.Children.Values)
                ReorderCategoryChildren(child);
        }
    }
}
