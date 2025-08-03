using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.CascadeDelete;
using Comparatist.Services.CategoryTableComposing;
using Comparatist.Services.CategoryTree;
using Comparatist.Services.TableCache;

namespace Comparatist.Services.Infrastructure
{
    public class ProjectService : IProjectService
    {
        private CascadeDeleteService _cascadeDelete;
        private TableCacheService _tableCache;
        private CategoryTreeService _categoryTree;
        private CategoryTableComposingService _categoryTableComposing;
        private IDatabase _database;

        public ProjectService()
        {
            _database = new Database();
            _cascadeDelete = new CascadeDeleteService(_database);
            _tableCache = new TableCacheService(_database);
            _categoryTree = new CategoryTreeService(_database);
            _categoryTableComposing = new CategoryTableComposingService();
        }

        public Result LoadDatabase(string path)
        {
            return Execute(() =>
            {
                _database.Load(path);
                _tableCache.RebuildCache();
                _categoryTree.RebuildCache();
                RebuildCategoryTableCache();
            });
        }

        public Result SaveDatabase(string path)
        {
            return Execute(() =>
            {
                _database.Save(path);
            });
        }

        public Result<IEnumerable<Language>> GetAllLanguages()
        {
            return Execute(() =>
                (IEnumerable<Language>)_database.GetRepository<Language>()
                    .GetAll()
                    .OrderBy(e => e.Order));
        }

        public Result<IEnumerable<Category>> GetAllCategories()
        {
            return Execute(() =>
                (IEnumerable<Category>)_database.GetRepository<Category>()
                    .GetAll()
                    .OrderBy(e => e.Order));
        }

        public Result<IEnumerable<CachedCategoryNode>> GetCategoryTree()
        {
            return Execute(_categoryTree.GetTree);
        }

        public Result<IEnumerable<CachedSection>> GetWordTable(SortingTypes sortingType)
        {
            switch (sortingType)
            {
                case SortingTypes.Alphabet: return Execute(_tableCache.GetTable);
                case SortingTypes.Categories: return Execute(_categoryTableComposing.GetTable);
                default: throw new ArgumentException($"Sorting type {sortingType} is not supported");
            }
        }

        public Result Add<T>(T record) where T : class, IRecord
        {
            return Execute(() =>
            {
                _database.GetRepository<T>().Add(record);

                switch (record)
                {
                    case Root r: _tableCache.Add(r); break;
                    case Stem s: _tableCache.Add(s); break;
                    case Word w: _tableCache.Add(w); break;

                    case Category: _categoryTree.MarkDirty(); break;
                    case Language: _tableCache.MarkDirty(); break;
                }

                RebuildCategoryTableCache();
            });
        }

        public Result Update<T>(T record) where T : class, IRecord
        {
            return Execute(() =>
            {
                _database.GetRepository<T>().Update(record);

                switch (record)
                {
                    case Root r: _tableCache.Update(r); break;
                    case Stem s: _tableCache.Update(s); break;
                    case Word w: _tableCache.Update(w); break;
                    case Category: _categoryTree.MarkDirty(); break;
                    case Language: _tableCache.MarkDirty(); break;
                }

                RebuildCategoryTableCache();
            });
        }

        public Result UpdateMany<T>(IEnumerable<T> records) where T : class, IRecord
        {
            return Execute(() =>
            {
                var repo = _database.GetRepository<T>();

                foreach (var record in records)
                    repo.Update(record);

                var firstRecord = records.FirstOrDefault();

                switch (firstRecord)
                {
                    case Root r: _tableCache.MarkDirty(); break;
                    case Stem s: _tableCache.MarkDirty(); break;
                    case Word w: _tableCache.MarkDirty(); break;
                    case Category: _categoryTree.MarkDirty(); break;
                    case Language: _tableCache.MarkDirty(); break;
                }

                RebuildCategoryTableCache();
            });
        }

        public Result Delete<T>(T record) where T : class, IRecord
        {
            return Execute(() =>
            {
                _cascadeDelete.Delete(record);

                switch (record)
                {
                    case Root r: _tableCache.Delete(r); break;
                    case Stem s: _tableCache.Delete(s); break;
                    case Word w: _tableCache.Delete(w); break;
                    case Category: _categoryTree.MarkDirty(); break;
                    case Language: _tableCache.MarkDirty(); break;
                }

                RebuildCategoryTableCache();
            });
        }

        private Result Execute(Action action)
        {
            try
            {
                action();
                return Result.OK;
            }
            catch (Exception e)
            {
                return new Result(false, e.Message);
            }
        }

        private Result<T> Execute<T>(Func<T> func)
        {
            try
            {
                T value = func();
                return new Result<T>(true, value, string.Empty);
            }
            catch (Exception e)
            {
                return new Result<T>(false, default, e.Message);
            }
        }

        private void RebuildCategoryTableCache()
        {
            _categoryTableComposing.RebuildCache(_categoryTree.GetTree(), _tableCache.GetTable());
        }
    }
}


