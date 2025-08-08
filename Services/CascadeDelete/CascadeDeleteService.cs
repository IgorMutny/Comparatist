using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class CascadeDeleteService
    {
        private Dictionary<Type, ICascadeDeleteHandler> _handlers;
        private HashSet<Guid> _visitedIds = new();

        public CascadeDeleteService(IDatabase database)
        {
            _handlers = new Dictionary<Type, ICascadeDeleteHandler>
            {
                {typeof(Language), new LanguageCascadeDeleteHandler(database) },
                {typeof(Category), new CategoryCascadeDeleteHandler(database) },
                {typeof(Root), new RootCascadeDeleteHandler(database) },
                {typeof(Stem), new StemCascadeDeleteHandler(database) },
                {typeof(Word), new WordCascadeDeleteHandler(database) },
            };
        }

        public void Delete(IRecord record)
        {
            _visitedIds.Clear();
            DeleteRecursively(record);
        }

        private void DeleteRecursively(IRecord record)
        {
            if (_visitedIds.Contains(record.Id))
                return;

            _visitedIds.Add(record.Id);

            var type = record.GetType();

            if (!_handlers.TryGetValue(type, out var handler))
                throw new NotSupportedException();

            var boundedRecords = handler.GetBoundedRecords(record);

            foreach (var boundedRecord in boundedRecords)
                DeleteRecursively(boundedRecord);

            handler.Delete(record);
        }
    }
}
