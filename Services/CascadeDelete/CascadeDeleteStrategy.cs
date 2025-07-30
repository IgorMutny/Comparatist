using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal abstract class CascadeDeleteStrategy<T> : ICascadeDeleteStrategy
    where T : IRecord
    {
        protected IDatabase Database { get; private set; } = new Database();

        public void SetDatabase(IDatabase database)
        {
            Database = database;
        }

        public IEnumerable<IRecord> Delete(IRecord record)
        {
            if (record is not T typed)
                throw new ArgumentException($"Record {record.Id} is not a {typeof(T)}");

            return Delete(typed);
        }

        protected abstract IEnumerable<IRecord> Delete(T record);
    }
}

