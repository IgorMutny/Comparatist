using Comparatist.Data.Entities;

namespace Comparatist.Data.Persistence
{
    public interface IRepository
    {
        void Clear();
        IEnumerable<IRecord> Export();
        void Import(IEnumerable<IRecord> records);
    }

    public interface IRepository<T> : IRepository where T : class, IRecord
    {
        event Action<T>? RecordAdded;
        event Action<T>? RecordUpdated;
        event Action<T>? RecordDeleted;

        void Add(T entity);
        void Update(T entity);
        void Delete(Guid id);

        bool TryGet(Guid id, out T record);
        IEnumerable<T> GetAll();
    }
}
