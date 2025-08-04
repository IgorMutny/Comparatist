using Comparatist.Services.Exceptions;
using Comparatist.Services.Cache;
using Comparatist.Services.Infrastructure;
using Comparatist.Core.Records;

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

        public IEnumerable<CachedCategory> GetAllCategories()
        {
            return _cache.Categories
                .Select(pair => new KeyValuePair<Guid, CachedCategory>(
                    pair.Key, 
                    (CachedCategory)pair.Value.Clone()))
                .OrderBy(e => e.Value.Record.Order)
                .ToDictionary().Values;
        }

        public IEnumerable<CachedCategory> GetWordTable(SortingTypes type)
        {
            return type switch
            {
                SortingTypes.Alphabet => GetWordTableByAlphabet(),
                SortingTypes.Categories => GetWordTableByCategories(),
                _ => throw new NotSupportedException()
            };

        }

        private IEnumerable<CachedCategory> GetWordTableByAlphabet()
        {
            var allRoots = _cache.Roots
                .Select(pair => new KeyValuePair<Guid, CachedRoot>(
                    pair.Key,
                    (CachedRoot)pair.Value.Clone()))
                .OrderBy(e => e.Value.Record.Value)
                .ToDictionary();

            return new List<CachedCategory> { new CachedCategory
            {
                Record = new Category { Value = "By alphabet" },
                Roots = allRoots
            } };
        }

        private IEnumerable<CachedCategory> GetWordTableByCategories()
        {
            var categories = GetCategoryTree();

            foreach (var category in categories)
                ReorderRootsRecursively(category);

            return categories;
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
                ReorderCategoryRecursively(e.Value);

            return result.Values;
        }

        private void ReorderCategoryRecursively(CachedCategory category)
        {
            category.Children = category.Children.OrderBy(e => e.Value.Record.Order).ToDictionary();

            foreach (var child in category.Children.Values)
                ReorderCategoryRecursively(child);
        }

        private void ReorderRootsRecursively(CachedCategory category)
        {
            category.Roots = category.Roots.OrderBy(e => e.Value.Record.Value).ToDictionary();

            foreach (var child in category.Children.Values)
                ReorderRootsRecursively(child);
        }
    }
}
