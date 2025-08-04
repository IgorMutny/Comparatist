using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.Services.Infrastructure
{
    public interface IProjectService: IDisposable
    {
        Result Add<T>(T record) where T : class, IRecord;
        Result Update<T>(T record) where T : class, IRecord;
        Result UpdateMany<T>(IEnumerable<T> records) where T : class, IRecord;
        Result Delete<T>(T record) where T : class, IRecord;
        Result<IEnumerable<CachedLanguage>> GetAllLanguages();
        Result<IEnumerable<CachedCategory>> GetAllCategories();
        Result<IEnumerable<CachedCategory>> GetCategoryTree();
        Result<IEnumerable<CachedCategory>> GetWordTable(SortingTypes sortingType);
        Result LoadDatabase(string path);
        Result SaveDatabase(string path);
    }
}