namespace Comparatist
{
    public interface IRepository<T> where T : IRecord
    {
        Guid Add(T entity);
        void Update(Guid id, T entity);
        void Delete(Guid id);
        T? GetById(Guid id);

        Dictionary<Guid, T> Export();
        IEnumerable<GuidedRecord<T>> GetAll();
        IEnumerable<GuidedRecord<T>> Filter(Func<T, bool> predicate);

        void CleanupDeleted();
        void Clear();
    }

    public class InMemoryRepository<T> : IRepository<T> where T : IRecord
    {
        private Dictionary<Guid, T> _storage = new();

        public Guid Add(T entity)
        {
            var id = Guid.NewGuid();
            _storage[id] = entity;
            return id;
        }

        public void Update(Guid id, T entity)
        {
            _storage[id] = entity;
        }

        public void Delete(Guid id)
        {
            if (_storage.TryGetValue(id, out var entity))
                entity.Deleted = true;
        }

        public T? GetById(Guid id)
        {
            _storage.TryGetValue(id, out var entity);

            if (entity != null && !entity.Deleted)
                return entity;
            else
                return default;
        }

        public IEnumerable<GuidedRecord<T>> GetAll()
        {
            return _storage
                .Where(e => !e.Value.Deleted)
                .Select((pair) => new GuidedRecord<T>() {Id = pair.Key, Record = pair.Value });
        }

        public IEnumerable<GuidedRecord<T>> Filter(Func<T, bool> predicate)
        {
            return _storage
                .Where(kv => !kv.Value.Deleted && predicate(kv.Value))
                .Select((pair) => new GuidedRecord<T>() { Id = pair.Key, Record = pair.Value });
        }

        public void CleanupDeleted()
        {
            var keysToRemove = new List<Guid>();

            foreach (var id in _storage)
                if (id.Value.Deleted == true)
                    keysToRemove.Add(id.Key);

            foreach (var key in keysToRemove)
                _storage.Remove(key);
        }

        public void Clear()
        {
            _storage.Clear();
        }

        public Dictionary<Guid, T> Export() => _storage;
        public void Import(Dictionary<Guid, T> data) => _storage = data;
    }

    public class GuidedRecord<T> where T: IRecord
    {
        public required Guid Id { get; set; }
        public required T Record { get; set; }
    }
}
