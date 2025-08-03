using Comparatist.Core.Records;

namespace Comparatist.Core.Infrastructure
{
    public interface IRepository
    {
        void Clear();
        IEnumerable<IRecord> Export();
        void Import(IEnumerable<IRecord> records);
    }

    public interface IRepository<T> : IRepository where T : class, IRecord
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(Guid id);

        bool TryGet(Guid id, out T record);
        IEnumerable<T> GetAll();
    }
}
