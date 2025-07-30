using Comparatist.Core.Records;

namespace Comparatist.Core.Persistence
{
    public interface IRepository<T> where T : IRecord
    {
        Guid Add(T entity);
        void Delete(Guid id);
        bool TryGet(Guid id, out T record);
        IEnumerable<T> GetAll();

        List<T> Export();
        void Import(List<T> records);
        void RemoveDeletedRecords();
        void Clear();
    }
}
