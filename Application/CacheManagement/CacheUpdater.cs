using Comparatist.Data.Persistence;
using Comparatist.Data.Entities;
using Comparatist.Application.Cache;

namespace Comparatist.Application.CacheManagement
{
    internal abstract class CacheUpdater<T> : IDisposable where T : class, IRecord
    {
        public CacheUpdater(IDatabase database, ProjectCache cache)
        {
            Database = database;
            Cache = cache;
        }

        protected IDatabase Database { get; }
        protected ProjectCache Cache { get; }

        public void Initialize()
        {
            var categoryRepo = Database.GetRepository<T>();
            categoryRepo.RecordAdded += OnAdded;
            categoryRepo.RecordUpdated += OnUpdated;
            categoryRepo.RecordDeleted += OnDeleted;
        }

        public void Dispose()
        {
            var categoryRepo = Database.GetRepository<T>();
            categoryRepo.RecordAdded -= OnAdded;
            categoryRepo.RecordUpdated -= OnUpdated;
            categoryRepo.RecordDeleted -= OnDeleted;
        }

        public abstract void RebuildCache();

        protected abstract void OnAdded(T record);
        protected abstract void OnUpdated(T record);
        protected abstract void OnDeleted(T record);
    }
}
