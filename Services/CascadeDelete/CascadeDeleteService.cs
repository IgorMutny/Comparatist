using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class CascadeDeleteService
    {
        private Dictionary<Type, ICascadeDeleteStrategy> _strategies;
        private HashSet<Guid> _visitedIds = new();

        public CascadeDeleteService(IDatabase database)
        {
            _strategies = new Dictionary<Type, ICascadeDeleteStrategy>
            {
                {typeof(Language), new LanguageCascadeDeleteStrategy(database) },
                {typeof(Category), new CategoryCascadeDeleteStrategy(database) },
                {typeof(Root), new RootCascadeDeleteStrategy(database) },
                {typeof(Stem), new StemCascadeDeleteStrategy(database) },
                {typeof(Word), new WordCascadeDeleteStrategy(database) },
            };
        }

        public void Delete(IRecord record)
        {
            _visitedIds.Clear();

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
