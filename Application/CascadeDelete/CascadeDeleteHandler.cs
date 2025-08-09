using Comparatist.Data.Persistence;
using Comparatist.Data.Entities;

namespace Comparatist.Application.CascadeDelete
{
    internal abstract class CascadeDeleteHandler<T> : ICascadeDeleteHandler
    where T : class, IRecord
    {
        protected IDatabase Database { get; private set; } = new Database();

        public CascadeDeleteHandler(IDatabase database)
        {
            Database = database;
        }

        public IEnumerable<IRecord> GetBoundedRecords(IRecord record)
        {
            if (record is not T typed)
                throw new ArgumentException($"Record {record.Id} is not a {typeof(T)}");

            return GetBoundedRecords(typed);
        }

        public void Delete(IRecord record)
        {
            if (record is not T typed)
                throw new ArgumentException($"Record {record.Id} is not a {typeof(T)}");

            Database.GetRepository<T>().Delete(record.Id);
        }

        protected abstract IEnumerable<IRecord> GetBoundedRecords(T record);
    }
}

