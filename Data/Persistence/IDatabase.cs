using Comparatist.Data.Entities;

namespace Comparatist.Data.Persistence
{
    public interface IDatabase
    {
        ProjectMetadata GetMetadata();
        IRepository<T> GetRepository<T>() where T : class, IRecord;
        void Save(string path);
        void Load(string path);
    }
}
