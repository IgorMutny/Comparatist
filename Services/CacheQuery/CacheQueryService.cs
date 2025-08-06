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

        public Dictionary<Guid, CachedLanguage> GetAllLanguages()
        {
            return _cache.Languages
                .Select(pair => new KeyValuePair<Guid, CachedLanguage>(
                    pair.Key,
                    (CachedLanguage)pair.Value.Clone()))
                .ToDictionary();
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

        public Dictionary<Guid, CachedCategory> GetWordTable(SortingTypes type)
        {
            return type switch
            {
                SortingTypes.Alphabet => GetWordTableByAlphabet(),
                SortingTypes.Categories => GetWordTableByCategories(),
                _ => throw new NotSupportedException()
            };

        }

        private Dictionary<Guid, CachedCategory> GetWordTableByAlphabet()
        {
            var allRoots = _cache.Roots
                .Select(pair => new KeyValuePair<Guid, CachedRoot>(
                    pair.Key,
                    (CachedRoot)pair.Value.Clone()))
                .OrderBy(e => e.Value.Record.Value)
                .ToDictionary();

            var category = new CachedCategory
            {
                Record = new Category { Value = "By alphabet" },
                Roots = allRoots
            };

            return new Dictionary<Guid, CachedCategory> { { Guid.Empty, category } };
        }

        private Dictionary<Guid, CachedCategory> GetWordTableByCategories()
        {
            var categories = GetCategoryTree();

            var uncategorizedRoots = _cache.UncategorizedRootIds
                .Select(id => new KeyValuePair<Guid, CachedRoot>(
                    id,
                    (CachedRoot)_cache.Roots[id].Clone()))
                .ToDictionary();

            var uncategorizedCategory = new CachedCategory
            {
                Record = new Category { Value = "Uncategorized", Order = int.MaxValue },
                Roots = uncategorizedRoots
            };

            categories.Add(Guid.Empty, uncategorizedCategory);

            return categories;
        }

        public Dictionary<Guid, CachedCategory> GetCategoryTree()
        {
            var result = new Dictionary<Guid, CachedCategory>();

            foreach (var id in _cache.BaseCategoryIds)
            {
                if (!_cache.Categories.TryGetValue(id, out var cached))
                    throw new CachedRecordNotFoundException(typeof(CachedCategory), id);

                result.Add(id, (CachedCategory)cached.Clone());
            }

            return result;
        }
    }
}
