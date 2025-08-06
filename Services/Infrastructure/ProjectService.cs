using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.Cache;
using Comparatist.Services.CacheQuery;
using Comparatist.Services.CacheUpdate;
using Comparatist.Services.CascadeDelete;

namespace Comparatist.Services.Infrastructure
{
    public class ProjectService : IProjectService
    {
        private IDatabase _database;
        private ProjectCache _projectCache;
        private CascadeDeleteService _cascadeDelete;
        private LanguageCacheUpdater _languageCacheUpdater;
        private CategoryCacheUpdater _categoryCacheUpdater;
        private RootCacheUpdater _rootCacheUpdater;
        private StemCacheUpdater _stemCacheUpdater;
        private WordCacheUpdater _wordCacheUpdater;
        private CacheQueryService _cacheQueryService;

        public ProjectService()
        {
            _database = new Database();
            _projectCache = new ProjectCache();

            _cascadeDelete = new CascadeDeleteService(_database);
            _languageCacheUpdater = new LanguageCacheUpdater(_database, _projectCache);
            _categoryCacheUpdater = new CategoryCacheUpdater(_database, _projectCache);
            _rootCacheUpdater = new RootCacheUpdater(_database, _projectCache);
            _stemCacheUpdater = new StemCacheUpdater(_database, _projectCache);
            _wordCacheUpdater = new WordCacheUpdater(_database, _projectCache);
            _languageCacheUpdater.Initialize();
            _categoryCacheUpdater.Initialize();
            _rootCacheUpdater.Initialize();
            _stemCacheUpdater.Initialize();
            _wordCacheUpdater.Initialize();
            _cacheQueryService = new CacheQueryService(_projectCache);
        }

        public void Dispose()
        {
            _wordCacheUpdater.Dispose();
            _stemCacheUpdater.Dispose();
            _rootCacheUpdater.Dispose();
            _categoryCacheUpdater.Dispose();
            _languageCacheUpdater.Dispose();
        }

        public Result LoadDatabase(string path)
        {
            return Execute(() =>
            {
                _database.Load(path);
                _projectCache.Clear();
                RebuildProjectCache();
            });
        }

        public Result SaveDatabase(string path)
        {
            return Execute(() => _database.Save(path));
        }

        public Result<Dictionary<Guid, CachedLanguage>> GetAllLanguages()
        {
            return Execute(_cacheQueryService.GetAllLanguages);
        }

        public Result<Dictionary<Guid, CachedCategory>> GetCategoryTree()
        {
            return Execute(_cacheQueryService.GetCategoryTree);
        }

        public Result<Dictionary<Guid, CachedCategory>> GetWordTable(SortingTypes sortingType)
        {
            return Execute(() => _cacheQueryService.GetWordTable(sortingType));
        }

        public Result Add<T>(T record) where T : class, IRecord
        {
            return Execute(() => _database.GetRepository<T>().Add(record));
        }

        public Result Update<T>(T record) where T : class, IRecord
        {
            return Execute(() => _database.GetRepository<T>().Update(record));
        }

        public Result UpdateMany<T>(IEnumerable<T> records) where T : class, IRecord
        {
            return Execute(() =>
            {
                var repo = _database.GetRepository<T>();

                foreach (var record in records)
                    repo.Update(record);
            });
        }

        public Result Delete<T>(T record) where T : class, IRecord
        {
            return Execute(() => _cascadeDelete.Delete(record));
        }

        private Result Execute(Action action)
        {
            try
            {
                action();
                return Result.OK;
            }
            catch (Exception e)
            {
                return new Result(false, $"{e.Message}: {e.StackTrace}");
            }
        }

        private Result<T> Execute<T>(Func<T> func)
        {
            try
            {
                T value = func();
                return new Result<T>(true, value, string.Empty);
            }
            catch (Exception e)
            {
                return new Result<T>(false, default, $"{e.Message}: {e.StackTrace}");
            }
        }

        private void RebuildProjectCache()
        {
            _languageCacheUpdater.RebuildCache();
            _categoryCacheUpdater.RebuildCache();
            _rootCacheUpdater.RebuildCache();
        }
    }
}


