using Comparatist.Core.Records;

namespace Comparatist.Core.Infrastructure
{
    public interface IRepository<T> where T : IRecord
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(Guid id);

        bool TryGet(Guid id, out T record);
        IEnumerable<T> GetAll();

        IEnumerable<T> Export();
        void Import(IEnumerable<T> records);
        void Clear();
    }
}
