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
        Result<Dictionary<Guid, CachedLanguage>> GetAllLanguages();
        Result<Dictionary<Guid, CachedCategory>> GetCategoryTree();
        Result<Dictionary<Guid, CachedCategory>> GetWordTable(SortingTypes sortingType);
        Result LoadDatabase(string path);
        Result SaveDatabase(string path);
    }
}