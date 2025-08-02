using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.TableCache
{
    internal class TableCacheService
    {
        private bool _isDirty = false;
        private IDatabase _database;
        private TableCache _cache;

        public TableCacheService(IDatabase database)
        {
            _database = database;
            _cache = new TableCache();
        }

        public void MarkDirty()
        {
            _isDirty = true;
        }

        public IEnumerable<CachedBlock> GetAllBlocksByAlphabet()
        {
            UpdateCacheIfDirty();

            return _cache.Blocks
                .Select(pair => (CachedBlock)pair.Value.Clone())
                .OrderBy(e => e.Root.Value);
        }

        public CachedBlock GetBlock(Guid rootId)
        {
            UpdateCacheIfDirty();

            if (_cache.Blocks.TryGetValue(rootId, out var b))
                return (CachedBlock)b.Clone();
            else
                throw new InvalidOperationException($"Root {rootId} not found in cache");
        }

        public CachedRow GetRow(Guid stemId)
        {
            UpdateCacheIfDirty();

            if (_cache.Rows.TryGetValue(stemId, out var r))
                return (CachedRow)r.Clone();
            else
                throw new InvalidOperationException($"Stem {stemId} not found in cache");
        }

        public void RebuildCache()
        {
            _cache.Blocks.Clear();
            _cache.Rows.Clear();

            foreach (var root in _database.Roots.GetAll())
                AddRoot(root);

            foreach (var stem in _database.Stems.GetAll())
                AddStem(stem);

            foreach (var word in _database.Words.GetAll())
                AddWord(word);
        }

        public void AddRoot(Root root)
        {
            _cache.Blocks.Add(root.Id, new CachedBlock { Root = root });
        }

        public void AddStem(Stem stem)
        {
            var row = new CachedRow { Stem = stem };
            _cache.Rows.Add(stem.Id, row);

            foreach (var rootId in stem.RootIds)
            {
                if (!_cache.Blocks.TryGetValue(rootId, out var block))
                    throw new InvalidOperationException($"Root {rootId} not found in cache");

                block.Rows.Add(stem.Id, row);
            }
        }

        public void AddWord(Word word)
        {
            if (!_cache.Rows.TryGetValue(word.StemId, out var row))
                throw new InvalidOperationException($"Stem {word.StemId} not found in cache");

            row.Cells.Add(word.LanguageId, word);
        }

        public void UpdateRoot(Root root)
        {
            if (!_cache.Blocks.TryGetValue(root.Id, out var block))
                throw new InvalidOperationException($"Root {root.Id} not found in cache");

            block.Root = root;
        }

        public void UpdateStem(Stem stem)
        {
            if (!_cache.Rows.TryGetValue(stem.Id, out var row))
                throw new InvalidOperationException($"Stem {stem.Id} not found in cache");

            row.Stem = stem;

            foreach (var rootId in stem.RootIds)
            {
                if (!_cache.Blocks.TryGetValue(rootId, out var block))
                    throw new InvalidOperationException($"Root {rootId} not found in cache");

                if (!block.Rows.ContainsKey(stem.Id))
                    block.Rows.Add(stem.Id, row);
            }
        }

        public void UpdateWord(Word word)
        {
            if (!_cache.Rows.TryGetValue(word.StemId, out var row))
                throw new InvalidOperationException($"Stem {word.StemId} not found in cache");

            row.Cells[word.LanguageId] = word;
        }

        public void DeleteRoot(Root root)
        {
            if (!_cache.Blocks.ContainsKey(root.Id))
                throw new InvalidOperationException($"Root {root.Id} not found in cache");

            _cache.Blocks.Remove(root.Id);
        }

        public void DeleteStem(Stem stem)
        {
            if (!_cache.Rows.Remove(stem.Id))
                throw new InvalidOperationException($"Stem {stem.Id} not found in cache");

            foreach (var rootId in stem.RootIds)
            {
                if (!_cache.Blocks.TryGetValue(rootId, out var block))
                    throw new InvalidOperationException($"Root {rootId} not found in cache");

                block.Rows.Remove(stem.Id);
            }
        }

        public void DeleteWord(Word word)
        {
            if (!_cache.Rows.TryGetValue(word.StemId, out var row))
                throw new InvalidOperationException($"Stem {word.StemId} not found in cache");

            if (!row.Cells.Remove(word.LanguageId))
                throw new InvalidOperationException($"Word {word.Id} not found in cache");
        }

        private void UpdateCacheIfDirty()
        {
            if (_isDirty)
                RebuildCache();

            _isDirty = false;
        }
    }
}
