using Comparatist.Data.Entities;
using Comparatist.Application.Cache;
using Comparatist.Data.Persistence;

namespace Comparatist.Application.Management
{
    public interface IProjectService: IDisposable
    {
        Result LoadDatabase(string path);
        Result SaveDatabase(string path);
        Result Add<T>(T record) where T : class, IRecord;
        Result Update<T>(T record) where T : class, IRecord;
        Result UpdateMany<T>(IEnumerable<T> records) where T : class, IRecord;
        Result Delete<T>(T record) where T : class, IRecord;
        Result<ProjectMetadata> GetProjectMetadata();
        Result<IEnumerable<CachedLanguage>> GetAllLanguages();
        Result<IEnumerable<CachedRoot>> GetAllRoots();
        Result<IEnumerable<CachedCategory>> GetAllCategories();
        Result<IEnumerable<CachedCategory>> GetBaseCategories();
        Result<IEnumerable<CachedRoot>> GetUncategorizedRoots();
        Result<IEnumerable<CachedCategory>> GetWordTableByAlphabet();
        Result<IEnumerable<CachedCategory>> GetWordTableByCategories();
    }
}