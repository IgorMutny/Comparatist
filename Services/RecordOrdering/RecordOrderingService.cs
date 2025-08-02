using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.RecordOrdering
{
    internal class RecordOrderingService
    {
        private IDatabase _database;

        public RecordOrderingService(IDatabase database)
        {
            _database = database;
        }

        public void Reorder<T>(IEnumerable<T> records) where T: IOrderableRecord
        {
            var repository = GetRepository<T>();

            int index = 0;
            foreach (var record in records)
            {
                record.Order = index++;
                repository.Update(record);
            }
        }

        private IRepository<T> GetRepository<T>() where T : IRecord
        {
            return typeof(T) switch
            {
                var t when t == typeof(Language) => (IRepository<T>)_database.Languages,
                var t when t == typeof(Category) => (IRepository<T>)_database.Categories,
                var t when t == typeof(Root) => (IRepository<T>)_database.Roots,
                var t when t == typeof(Stem) => (IRepository<T>)_database.Stems,
                var t when t == typeof(Word) => (IRepository<T>)_database.Words,
                _ => throw new NotSupportedException($"Unsupported record type: {typeof(T)}")
            };
        }
    }
}
