using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.CascadeDelete;
using Comparatist.Services.CategoryTree;
using Comparatist.Services.TableCache;

namespace Comparatist.Services.Infrastructure
{
    public class ProjectService : IProjectService
    {
        private CascadeDeleteService _cascadeDelete;
        private TableCacheService _tableCache;
        private CategoryTreeService _categoryTree;
        private IDatabase _database;

        public ProjectService()
        {
            _database = new Database();
            _cascadeDelete = new CascadeDeleteService(_database);
            _tableCache = new TableCacheService(_database);
            _categoryTree = new CategoryTreeService(_database);
        }

        public Result LoadDatabase(string path)
        {
            return Execute(() =>
            {
                _database.Load(path);
                _tableCache.RebuildCache();
                _categoryTree.RebuildCache();
            });
        }

        public Result SaveDatabase(string path)
        {
            return Execute(() =>
            {
                _database.Save(path);
            });
        }

        public Result<IEnumerable<CachedCategoryNode>> GetTree()
        {
            return Execute(_categoryTree.GetTree);
        }

        public Result<IEnumerable<Language>> GetAllLanguages()
        {
            return Execute(() => 
                (IEnumerable<Language>)_database.Languages.GetAll()
                    .OrderBy(e => e.Order));
        }

        public Result<IEnumerable<Category>> GetAllCategories()
        {
            return Execute(_database.Categories.GetAll);
        }

        public Result<IEnumerable<CachedBlock>> GetAllBlocksByAlphabet()
        {
            return Execute(_tableCache.GetAllBlocksByAlphabet);
        }

        public Result<CachedBlock> GetBlock(Guid rootId)
        {
            return Execute(() =>
            {
                return _tableCache.GetBlock(rootId);
            });
        }

        public Result<CachedRow> GetRow(Guid stemId)
        {
            return Execute(() =>
            {
                return _tableCache.GetRow(stemId);
            });
        }

        public Result AddLanguage(Language language)
        {
            return Execute(() =>
            {
                _database.Languages.Add(language);
                _tableCache.MarkDirty();
            });
        }

        public Result UpdateLanguage(Language language)
        {
            return Execute(() =>
            {
                _database.Languages.Update(language);
                _tableCache.MarkDirty();
            });
        }

        public Result DeleteLanguage(Language language)
        {
            return Execute(() =>
            {
                _cascadeDelete.Delete(language);
                _tableCache.MarkDirty();
            });
        }

        public Result AddCategory(Category category)
        {
            return Execute(() =>
            {
                _database.Categories.Add(category);
                _categoryTree.MarkDirty();
            });
        }

        public Result UpdateCategory(Category category)
        {
            return Execute(() =>
            {
                _database.Categories.Update(category);
                _categoryTree.MarkDirty();
            });
        }

        public Result DeleteCategory(Category category)
        {
            return Execute(() =>
            {
                _cascadeDelete.Delete(category);
                _categoryTree.MarkDirty();
            });
        }

        public Result AddRoot(Root root)
        {
            return Execute(() =>
            {
                _database.Roots.Add(root);
                _tableCache.AddRoot(root);
            });
        }

        public Result UpdateRoot(Root root)
        {
            return Execute(() =>
            {
                _database.Roots.Update(root);
                _tableCache.UpdateRoot(root);
            });
        }

        public Result DeleteRoot(Root root)
        {
            return Execute(() =>
            {
                _cascadeDelete.Delete(root);
                _tableCache.DeleteRoot(root);
            });
        }

        public Result AddStem(Stem stem)
        {
            return Execute(() =>
            {
                _database.Stems.Add(stem);
                _tableCache.AddStem(stem);
            });
        }

        public Result UpdateStem(Stem stem)
        {
            return Execute(() =>
            {
                _database.Stems.Update(stem);
                _tableCache.UpdateStem(stem);
            });
        }

        public Result DeleteStem(Stem stem)
        {
            return Execute(() =>
            {
                _cascadeDelete.Delete(stem);
                _tableCache.DeleteStem(stem);
            });
        }

        public Result AddWord(Word word)
        {
            return Execute(() =>
            {
                _database.Words.Add(word);
                _tableCache.AddWord(word);
            });
        }

        public Result UpdateWord(Word word)
        {
            return Execute(() =>
            {
                _database.Words.Update(word);
                _tableCache.UpdateWord(word);
            });
        }

        public Result DeleteWord(Word word)
        {
            return Execute(() =>
            {
                _cascadeDelete.Delete(word);
                _tableCache.DeleteWord(word);
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
    }
}
