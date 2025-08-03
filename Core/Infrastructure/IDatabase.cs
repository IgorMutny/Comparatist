using Comparatist.Core.Records;

namespace Comparatist.Core.Infrastructure
{
    public interface IDatabase
    {
        ProjectMetadata Metadata { get; }
        IRepository<T> GetRepository<T>() where T : class, IRecord;
        void Save(string path);
        void Load(string path);
    }
}
