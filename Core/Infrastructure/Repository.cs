using Comparatist.Core.Records;
using System.Diagnostics.CodeAnalysis;

namespace Comparatist.Core.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class, IRecord
    {
        private Dictionary<Guid, T> _storage = new();

        public void Add(T entity)
        {
            var id = Guid.NewGuid();

            if (_storage.ContainsKey(id))
                throw new InvalidOperationException($"{typeof(T).Name} with id {id} already exists");

            entity.Id = id;
            _storage[id] = (T)entity.Clone();
        }

        public void Update(T entity)
        {
            if (_storage.ContainsKey(entity.Id))
                _storage[entity.Id] = (T)entity.Clone();
            else
                throw new InvalidOperationException($"{typeof(T).Name} with id {entity.Id} does not exist");
        }

        public void Delete(Guid id)
        {
            if (!_storage.Remove(id))
                throw new InvalidOperationException($"{typeof(T).Name} with id {id} does not exist");
        }

        public bool TryGet(Guid id, [NotNullWhen(true)] out T record)
        {
            if (_storage.TryGetValue(id, out var r))
            {
                record = (T)r.Clone();
                return true;
            }

            record = default!;
            return false;
        }

        public IEnumerable<T> GetAll()
        {
            return _storage.Values.Select(record => (T)record.Clone());
        }

        public IEnumerable<IRecord> Export()
        {
            return _storage.Values;
        }

        public void Import(IEnumerable<IRecord> records)
        {
            Clear();

            foreach (var record in records)
                _storage.Add(record.Id, (T)record);
        }

        public void Clear()
        {
            _storage.Clear();
        }
    }
}
