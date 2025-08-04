using Comparatist.Core.Records;
using Comparatist.Services.Cache;
using Comparatist.Services.CategoryTree;
using Comparatist.Services.TableCache;

namespace Comparatist.Services.Infrastructure
{
    public interface IProjectService
    {
        Result Add<T>(T record) where T : class, IRecord;
        Result Update<T>(T record) where T : class, IRecord;
        Result UpdateMany<T>(IEnumerable<T> records) where T : class, IRecord;
        Result Delete<T>(T record) where T : class, IRecord;
        Result<IEnumerable<Language>> GetAllLanguages();
        Result<IEnumerable<Category>> GetAllCategories();
        Result<IEnumerable<CachedCategory>> GetCategoryTree();
        Result<IEnumerable<CachedCategory>> GetWordTable(SortingTypes sortingType);
        Result LoadDatabase(string path);
        Result SaveDatabase(string path);
    }
}