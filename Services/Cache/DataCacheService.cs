using Comparatist.Core.Persistence;
using Comparatist.Core.Records;
using Comparatist.View.Tags;

namespace Comparatist.Services.Cache
{
    public class DataCacheService
    {
        private IDatabase _database;
        private DataCache _cache;

        public DataCacheService()
        {
            _database = new Database();
            _cache = new DataCache();
        }

        public IReadOnlyList<RootTag> GetAlphabeticalTableData()
        {
            return _cache.Roots.Values
                .OrderBy(r => r.Root.Value)
                .Select(cachedRoot => new RootTag(cachedRoot))
                .ToList();
        }

        public IEnumerable<RootTag> AllRoots =>
            _database.Roots.GetAll()
            .OrderBy(r => r.Value)
            .Select(root => new RootTag(
                root.Id,
                root.Value,
                root.Translation,
                root.Comment,
                root.IsNative,
                root.IsChecked,
                root.CategoryIds));

        public IEnumerable<LanguageTag> AllLanguages =>
            _database.Languages.GetAll()
            .Select(language => new LanguageTag(language));

        public IEnumerable<CategoryTag> AllCategories =>
            _database.Categories.GetAll()
            .Select(category => new CategoryTag(category));

        public void BuildFromDataBase(IDatabase database)
        {
            _database = database;
            RebuildCache();
        }

        private void RebuildCache()
        {
            _cache.Stems.Clear();
            _cache.Roots.Clear();

            foreach (var root in _database.Roots.GetAll())
                AddRootToCache(root);

            foreach (var stem in _database.Stems.GetAll())
                AddStemToCache(stem);

            foreach (var word in _database.Words.GetAll())
                AddWordToCache(word);
        }

        private void AddRootToCache(Root root)
        {
            _cache.Roots.Add(root.Id, new CachedRoot { Root = root });
        }

        private void AddStemToCache(Stem stem)
        {
            var cachedStem = new CachedStem { Stem = stem };
            _cache.Stems.Add(stem.Id, cachedStem);

            foreach (var rootId in stem.RootIds)
            {
                if (_cache.Roots.TryGetValue(rootId, out var cachedRoot))
                    cachedRoot.Stems.Add(cachedStem);
                else
                    throw new InvalidOperationException($"Root {rootId} not found in cache");
            }
        }

        private void AddWordToCache(Word word)
        {
            //if (!_cache.Stems.TryGetValue(word.StemId, out var cachedStem))
            // throw new InvalidOperationException($"Stem {word.StemId} not found in cache");
            if (_cache.Stems.TryGetValue(word.StemId, out var cachedStem))
                cachedStem.WordsByLanguage[word.LanguageId] = word;
        }

        private void DeleteRootFromCache(Guid rootId)
        {
            if (!_cache.Roots.ContainsKey(rootId))
                throw new InvalidOperationException($"Root {rootId} not found in cache");

            _cache.Roots.Remove(rootId);
        }

        private void DeleteStemFromCache(Guid stemId, Stem stem)
        {
            _cache.Stems.Remove(stemId);

            foreach (var rootId in stem.RootIds)
            {
                if (_cache.Roots.TryGetValue(rootId, out var cachedRoot))
                {
                    var cachedStem = cachedRoot.Stems.FirstOrDefault(s => s.Stem.Id == stemId);

                    if (cachedStem == null || !cachedRoot.Stems.Remove(cachedStem))
                        throw new InvalidOperationException($"Stem {stemId} not found in cache");
                }
                else
                {
                    throw new InvalidOperationException($"Root {rootId} not found in cache");
                }
            }
        }

        private void DeleteWordFromCache(Word word)
        {
            if (_cache.Stems.TryGetValue(word.StemId, out var cachedStem))
            {
                if (!cachedStem.WordsByLanguage.Remove(word.LanguageId))
                    throw new InvalidOperationException($"Word {word.Id} not found in cache");
            }
            else
            {
                throw new InvalidOperationException($"Stem {word.StemId} not found in cache");
            }
        }

        public void AddRoot(RootTag data)
        {
            var newRoot = new Root
            {
                Value = data.Value,
                Translation = data.Translation,
                Comment = data.Comment,
                CategoryIds = (List<Guid>)data.CategoryIds,
                IsNative = data.IsNative,
                IsChecked = data.IsChecked
            };

            _database.Roots.Add(newRoot);
            AddRootToCache(newRoot);
        }

        public void UpdateRoot(RootTag data)
        {
            if (_database.Roots.TryGet(data.Id, out var root))
            {
                root.Value = data.Value;
                root.Translation = data.Translation;
                root.Comment = data.Comment;
                root.CategoryIds = (List<Guid>)data.CategoryIds;
                root.IsNative = data.IsNative;
                root.IsChecked = data.IsChecked;
            }
            else
            {
                throw new InvalidOperationException($"Root {data.Id} not found in database");
            }
        }

        public void DeleteRoot(Guid rootId)
        {
            if (!_database.Roots.TryGet(rootId, out var root))
                throw new InvalidOperationException($"Root {rootId} not found in database");

            _database.Roots.Delete(rootId);
            DeleteRootFromCache(rootId);
        }

        public void AddStem(StemTag data)
        {
            var newStem = new Stem
            {
                Value = data.Value,
                Translation = data.Translation,
                Comment = data.Comment,
                RootIds = (List<Guid>)data.RootIds,
                IsNative = data.IsNative,
                IsChecked = data.IsChecked
            };

            _database.Stems.Add(newStem);
            AddStemToCache(newStem);
        }

        public void UpdateStem(StemTag data)
        {
            if (_database.Stems.TryGet(data.Id, out var stem))
            {
                stem.Value = data.Value;
                stem.Translation = data.Translation;
                stem.Comment = data.Comment;
                stem.RootIds = (List<Guid>)data.RootIds;
                stem.IsNative = data.IsNative;
                stem.IsChecked = data.IsChecked;
            }
            else
            {
                throw new InvalidOperationException($"Stem {data.Id} not found in database");
            }
        }

        public void DeleteStem(Guid stemId)
        {
            if (!_database.Stems.TryGet(stemId, out var stem))
                throw new InvalidOperationException($"Stem {stemId} not found in database");

            _database.Stems.Delete(stemId);
            DeleteStemFromCache(stemId, stem);
        }

        public void AddWord(WordTag data)
        {
            var newWord = new Word
            {
                Value = data.Value,
                Translation = data.Translation,
                Comment = data.Comment,
                StemId = data.StemId,
                LanguageId = data.LanguageId,
                IsNative = data.IsNative,
                IsChecked = data.IsChecked
            };

            _database.Words.Add(newWord);
            AddWordToCache(newWord);
        }

        public void UpdateWord(WordTag data)
        {
            if (_database.Words.TryGet(data.Id, out var word))
            {
                word.Value = data.Value;
                word.Translation = data.Translation;
                word.Comment = data.Comment;
                word.IsNative = data.IsNative;
                word.IsChecked = data.IsChecked;
            }
            else
            {
                throw new InvalidOperationException($"Word {data.Id} not found in database");
            }
        }

        public void DeleteWord(Guid wordId)
        {
            if (!_database.Words.TryGet(wordId, out var word))
                throw new InvalidOperationException($"Word {wordId} not found in database");

            _database.Words.Delete(wordId);
            DeleteWordFromCache(word);
        }
    }
}
