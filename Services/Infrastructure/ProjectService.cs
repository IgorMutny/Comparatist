using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.CascadeDelete;
using Comparatist.Services.TableCache;

namespace Comparatist.Services.Infrastructure
{
    public class ProjectService
    {
        private CascadeDeleteService _cascadeDelete;
        private TableCacheService _tableCache;
        private IDatabase _database;

        public ProjectService()
        {
            _database = new Database();
            _cascadeDelete = new CascadeDeleteService(_database);
            _tableCache = new TableCacheService(_database);
        }

        public Result LoadDatabase(string path)
        {
            return Execute(() =>
            {
                _database.Load(path);
                _tableCache.RebuildCache();
            });
        }

        public Result SaveDatabase(string path)
        {
            return Execute(() =>
            {
                _database.Save(path);
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
                _tableCache.DeleteRoot(root.Id);
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
                _tableCache.DeleteStem(stem.Id);
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
                _tableCache.DeleteWord(word.Id);
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
                try { _tableCache.RebuildCache(); } catch { }
                return new Result(false, e.Message);
            }
        }
    }
}
