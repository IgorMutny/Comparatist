using Comparatist.Data.Entities;
using Comparatist.Application.Cache;

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
        Result<Dictionary<Guid, CachedLanguage>> GetAllLanguages();
        Result<Dictionary<Guid, CachedCategory>> GetAllCategories();
        Result<Dictionary<Guid, CachedRoot>> GetAllRoots();
        Result<Dictionary<Guid, CachedCategory>> GetWordTableByAlphabet();
        Result<Dictionary<Guid, CachedCategory>> GetWordTableByCategory();
        Result<Dictionary<Guid, CachedCategory>> GetCategoryTree();
    }
}