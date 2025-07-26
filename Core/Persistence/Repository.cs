using System.Diagnostics.CodeAnalysis;

namespace Comparatist
{

    public class Repository<T> : IRepository<T> where T : IRecord
    {
        private Dictionary<Guid, T> _storage = new();

        public Guid Add(T entity)
        {
            var id = Guid.NewGuid();
            entity.Id = id;
            _storage[id] = entity;
            return id;
        }

        public void Delete(Guid id)
        {
            if (_storage.TryGetValue(id, out var entity))
                entity.Deleted = true;
        }

        public bool TryGet(Guid id, [NotNullWhen(true)] out T record)
        {
            if (_storage.TryGetValue(id, out var r) && !r.Deleted)
            {
                record = r;
                return true;
            }

            record = default!;
            return false;
        }

        public IEnumerable<T> GetAll()
        {
            return _storage.Values.Where(e => !e.Deleted);
        }

        public IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            return _storage.Values.Where(e => !e.Deleted && predicate(e));
        }

        public List<T> Export()
        {
            return _storage.Values.ToList();
        }

        public void Import(List<T> records)
        {
            Clear();

            foreach (var record in records)
                _storage.Add(record.Id, record);
        }

        public void RemoveDeletedRecords()
        {
            var deletedKeys = _storage
                .Where(pair => pair.Value.Deleted)
                .Select(pair => pair.Key)
                .ToList();

            foreach (var key in deletedKeys)
                _storage.Remove(key);
        }

        public void Clear()
        {
            _storage.Clear();
        }
    }
}
