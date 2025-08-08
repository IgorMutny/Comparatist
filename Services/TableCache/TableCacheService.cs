using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.Services.TableCache
{
    internal class TableCacheService
    {
        private readonly Dictionary<Guid, CachedRoot> _blocks = new();
        private readonly Dictionary<Guid, CachedStem> _rows = new();
        private bool _isDirty = false;
        private IDatabase _database;

        public TableCacheService(IDatabase database)
        {
            _database = database;
        }

        public void MarkDirty()
        {
            _isDirty = true;
        }

        public IEnumerable<CachedCategory> GetTable()
        {
            UpdateCacheIfDirty();

            var section = new CachedCategory { Record = new() };

            section.Roots = _blocks
                .Select(pair => new KeyValuePair<Guid, CachedRoot>
                (pair.Key, (CachedRoot)pair.Value.Clone()))
                .OrderBy(e => e.Value.Record.Value)
                .ToDictionary();

            return new List<CachedCategory> { section };
        }

        public void RebuildCache()
        {
            _blocks.Clear();
            _rows.Clear();

            foreach (var root in _database.GetRepository<Root>().GetAll())
                AddRoot(root);

            foreach (var stem in _database.GetRepository<Stem>().GetAll())
                AddStem(stem);

            foreach (var word in _database.GetRepository<Word>().GetAll())
                AddWord(word);
        }

        public void Add<T>(T entity) where T : IRecord
        {
            switch (entity)
            {
                case Root root: AddRoot(root); break;
                case Stem stem: AddStem(stem); break;
                case Word word: AddWord(word); break;
                default: break;
            }
        }

        public void Update<T>(T entity) where T : IRecord
        {
            switch (entity)
            {
                case Root root: UpdateRoot(root); break;
                case Stem stem: UpdateStem(stem); break;
                case Word word: UpdateWord(word); break;
                default: break;
            }
        }

        public void Delete<T>(T entity) where T : IRecord
        {
            switch (entity)
            {
                case Root root: DeleteRoot(root); break;
                case Stem stem: DeleteStem(stem); break;
                case Word word: DeleteWord(word); break;
                default: break;
            }
        }

        private void AddRoot(Root root)
        {
            _blocks.Add(root.Id, new CachedRoot { Record = root });
        }

        private void AddStem(Stem stem)
        {
            var row = new CachedStem { Record = stem };
            _rows.Add(stem.Id, row);

            foreach (var rootId in stem.RootIds)
            {
                if (!_blocks.TryGetValue(rootId, out var block))
                    throw new InvalidOperationException($"Root {rootId} not found in cache");

                block.Stems.Add(stem.Id, row);
            }
        }

        private void AddWord(Word word)
        {
            if (!_rows.TryGetValue(word.StemId, out var row))
                throw new InvalidOperationException($"Stem {word.StemId} not found in cache");

            row.Words.Add(word.LanguageId, new CachedWord { Record = word });
        }

        private void UpdateRoot(Root root)
        {
            if (!_blocks.TryGetValue(root.Id, out var block))
                throw new InvalidOperationException($"Root {root.Id} not found in cache");

            block.Record = root;
        }

        private void UpdateStem(Stem stem)
        {
            if (!_rows.TryGetValue(stem.Id, out var row))
                throw new InvalidOperationException($"Stem {stem.Id} not found in cache");

            row.Record = stem;

            foreach (var rootId in stem.RootIds)
            {
                if (!_blocks.TryGetValue(rootId, out var block))
                    throw new InvalidOperationException($"Root {rootId} not found in cache");

                block.Stems[stem.Id].Record = stem;
            }
        }

        private void UpdateWord(Word word)
        {
            if (!_rows.TryGetValue(word.StemId, out var row))
                throw new InvalidOperationException($"Stem {word.StemId} not found in cache");

            row.Words[word.LanguageId] = new CachedWord { Record = word };
        }

        private void DeleteRoot(Root root)
        {
            if (!_blocks.ContainsKey(root.Id))
                throw new InvalidOperationException($"Root {root.Id} not found in cache");

            _blocks.Remove(root.Id);
        }

        private void DeleteStem(Stem stem)
        {
            if (!_rows.Remove(stem.Id))
                throw new InvalidOperationException($"Stem {stem.Id} not found in cache");

            foreach (var rootId in stem.RootIds)
            {
                if (!_blocks.TryGetValue(rootId, out var block))
                    throw new InvalidOperationException($"Root {rootId} not found in cache");

                if (!block.Stems.Remove(stem.Id))
                    throw new InvalidOperationException($"Stem {stem.Id} not found in cache"); ;
            }
        }

        private void DeleteWord(Word word)
        {
            if (!_rows.TryGetValue(word.StemId, out var row))
                throw new InvalidOperationException($"Stem {word.StemId} not found in cache");

            if (!row.Words.Remove(word.LanguageId))
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
