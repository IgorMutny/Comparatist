using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class CascadeDeleteService
    {
        private IDatabase _database;
        private Dictionary<Type, ICascadeDeleteStrategy> _strategies;
        private HashSet<Guid> _visitedIds = new();

        public CascadeDeleteService()
        {
            _database = new Database();
            _strategies = new Dictionary<Type, ICascadeDeleteStrategy>
            {
                {typeof(Language), new LanguageCascadeDeleteStrategy() },
                {typeof(Category), new CategoryCascadeDeleteStrategy() },
                {typeof(Root), new RootCascadeDeleteStrategy() },
                {typeof(Stem), new StemCascadeDeleteStrategy() },
                {typeof(Word), new WordCascadeDeleteStrategy() },
            };
        }

        public void SetDatabase(IDatabase database)
        {
            _visitedIds.Clear();
            _database = database;

            foreach (var strategy in _strategies.Values)
                strategy.SetDatabase(_database);
        }

        public void SetAndValidateDatabase(IDatabase database)
        {
            SetDatabase(database);
            ValidateDatabase();
        }

        public void Delete(IRecord record)
        {
            _visitedIds.Clear();
            DeleteInternal(record);
        }

        public void ValidateDatabase()
        {
            _visitedIds.Clear();
            var deletedRecords = _database.GetAllRecords().Where(r => r.IsDeleted);

            foreach (var record in deletedRecords)
                DeleteInternal(record);

            _database.RemoveDeletedRecords();
        }

        private void DeleteInternal(IRecord record)
        {
            if (_visitedIds.Contains(record.Id))
                return;

            _visitedIds.Add(record.Id);

            var type = record.GetType();

            if (!_strategies.TryGetValue(type, out var strategy))
                throw new InvalidOperationException($"No strategy for type {type}");

            var boundedRecords = strategy.Delete(record);

            foreach (var boundedRecord in boundedRecords)
                Delete(boundedRecord);
        }
    }
}
