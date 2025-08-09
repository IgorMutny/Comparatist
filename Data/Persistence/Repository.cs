using Comparatist.Data.Exceptions;
using Comparatist.Data.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Comparatist.Data.Persistence
{
    public class Repository<T> : IRepository<T> where T : class, IRecord
    {
        private Dictionary<Guid, T> _storage = new();

        public event Action<T>? RecordAdded;
        public event Action<T>? RecordUpdated;
        public event Action<T>? RecordDeleted;

        public void Add(T newRecord)
        {
            var id = Guid.NewGuid();

            if (_storage.ContainsKey(id))
                throw new RecordAlreadyExistsException(typeof(T), id);

            newRecord.Id = id;
            _storage[id] = (T)newRecord.Clone();
            RecordAdded?.Invoke((T)_storage[id].Clone());
        }

        public void Update(T updatedRecord)
        {
            if (!_storage.ContainsKey(updatedRecord.Id))
                throw new RecordNotFoundException(typeof(T), updatedRecord.Id);

            var id = updatedRecord.Id;
            _storage[id] = (T)updatedRecord.Clone();
            RecordUpdated?.Invoke(updatedRecord);
        }

        public void Delete(Guid id)
        {
            if (!_storage.ContainsKey(id))
                throw new RecordNotFoundException(typeof(T), id);
            var entity = _storage[id];
            _storage.Remove(id);
            RecordDeleted?.Invoke((T)entity.Clone());
        }

        public bool TryGet(Guid id, [NotNullWhen(true)] out T recordCopy)
        {
            if (_storage.TryGetValue(id, out var record))
            {
                recordCopy = (T)record.Clone();
                return true;
            }

            recordCopy = default!;
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
