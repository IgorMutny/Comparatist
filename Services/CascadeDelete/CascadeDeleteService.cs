using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class CascadeDeleteService
    {
        private IDatabase _database;
        private Dictionary<Type, ICascadeDeleteStrategy> _strategies;
        private HashSet<Guid> _visitedIds = new();

        public CascadeDeleteService(IDatabase database)
        {
            _database = database;

            _strategies = new Dictionary<Type, ICascadeDeleteStrategy>
            {
                {typeof(Language), new LanguageCascadeDeleteStrategy(database) },
                {typeof(Category), new CategoryCascadeDeleteStrategy(database) },
                {typeof(Root), new RootCascadeDeleteStrategy(database) },
                {typeof(Stem), new StemCascadeDeleteStrategy(database) },
                {typeof(Word), new WordCascadeDeleteStrategy(database) },
            };
        }

        public void ValidateDatabase()
        {
            _visitedIds.Clear();
            var deletedRecords = _database.GetAllRecords().Where(r => r.IsDeleted);

            foreach (var record in deletedRecords)
                DeleteInternal(record);
        }

        public void Delete(IRecord record)
        {
            _visitedIds.Clear();
            DeleteInternal(record);
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
