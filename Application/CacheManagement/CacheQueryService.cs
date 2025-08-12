using Comparatist.Application.Cache;
using Comparatist.Data.Entities;

namespace Comparatist.Application.CacheManagement
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
                .Select(pair => (CachedLanguage)pair.Value.Clone())
                .OrderBy(e => e.Record.Order);
        }

        public IEnumerable<CachedRoot> GetAllRoots()
        {
            return _cache.Roots
                .Select(pair => (CachedRoot)pair.Value.Clone())
                .OrderBy(e => e.Record.Value);
        }

        public IEnumerable<CachedCategory> GetAllCategories()
        {
            var allCategories = new List<CachedCategory>();
            var baseCategories = GetBaseCategories();

            foreach (var category in baseCategories)
                CollectCategoriesRecursively(category, allCategories);

            return allCategories.Select(e => (CachedCategory)e.Clone());
        }

        private void CollectCategoriesRecursively(CachedCategory category, List<CachedCategory> result)
        {
            result.Add(category);
            var orderedChildren = category.Children.Values.OrderBy(e => e.Record.Order);

            foreach (var child in orderedChildren)
                CollectCategoriesRecursively(child, result);
        }

        public IEnumerable<CachedCategory> GetBaseCategories()
        {
            return _cache.BaseCategoryIds
                .Select(id => (CachedCategory)_cache.Categories[id].Clone())
                .OrderBy(e => e.Record.Order);
        }

        public IEnumerable<CachedRoot> GetUncategorizedRoots()
        {
            return _cache.UncategorizedRootIds
                .Select(id => (CachedRoot)_cache.Roots[id].Clone()).
                OrderBy(e => e.Record.Value);
        }

        public IEnumerable<CachedCategory> GetWordTableByAlphabet()
        {
            var cachedCategory = new CachedCategory
            {
                Record = new Category{ Id = Guid.Empty, Value = "By alphabet" },
                Roots = GetAllRoots()
                    .Select(root => new KeyValuePair<Guid, CachedRoot>(
                        root.Record.Id,
                        root))
                    .ToDictionary()
            };

            return new List<CachedCategory> { cachedCategory };
        }

        public IEnumerable<CachedCategory> GetWordTableByCategories()
        {
            var result = GetAllCategories().ToList();

            var uncategorized = new Category
            {
                Id = Guid.Empty,
                Value = "Uncategorized",
                Order = result.Count()
            };

            var cachedUncategorized = new CachedCategory
            {
                Record = uncategorized,
                Roots = GetUncategorizedRoots()
                    .Select(root => new KeyValuePair<Guid, CachedRoot>(
                        root.Record.Id,
                        root))
                    .ToDictionary()
            };

            result.Add(cachedUncategorized);
            return result;
        }
    }
}