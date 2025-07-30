using Comparatist.Core.Records;
using System.Diagnostics.CodeAnalysis;

namespace Comparatist.Core.Persistence
{

    public class Repository<T> : IRepository<T> where T : IRecord
    {
        private Dictionary<Guid, T> _storage = new();

        public Guid Add(T entity)
        {
            var id = Guid.NewGuid();

            if (_storage.ContainsKey(id))
                throw new InvalidOperationException($"Guid {id} already exists");

            entity.Id = id;
            _storage[id] = entity;
            return id;
        }

        public void Update(T entity)
        {
            if (_storage.ContainsKey(entity.Id))
                _storage[entity.Id] = (T)entity.Clone();
            else
                throw new InvalidOperationException($"Guid {entity.Id} does not exist");
        }

        public void Delete(Guid id)
        {
            if (_storage.TryGetValue(id, out var entity))
                entity.IsDeleted = true;
            else
                throw new InvalidOperationException($"Guid {id} does not exist");
        }

        public bool TryGet(Guid id, [NotNullWhen(true)] out T record)
        {
            if (_storage.TryGetValue(id, out var r) && !r.IsDeleted)
            {
                record = (T)r.Clone();
                return true;
            }

            record = default!;
            return false;
        }

        public IEnumerable<T> GetAll()
        {
            return _storage.Values
                .Where(e => !e.IsDeleted)
                .Select(record => (T)record.Clone());
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
                .Where(pair => pair.Value.IsDeleted)
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
