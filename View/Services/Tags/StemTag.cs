using Comparatist.Services.TableCache;

namespace Comparatist.View.Tags
{
    public class StemTag
    {
        public readonly Guid Id;
        public readonly string Value;
        public readonly string Translation;
        public readonly string Comment;
        public readonly bool IsNative;
        public readonly bool IsChecked;
        private readonly List<Guid> _rootIds;
        private readonly Dictionary<Guid, WordTag?> _wordsByLanguage;

        public StemTag()
        {
            Id = default;
            Value = string.Empty;
            Translation = string.Empty;
            Comment = string.Empty;
            IsNative = false;
            IsChecked = false;
            _rootIds = new();
            _wordsByLanguage = new();
        }

        public StemTag(Guid id,
            string value,
            string translation,
            string comment,
            bool isNative,
            bool isChecked,
            List<Guid> rootIds)
        {
            Id = id;
            Value = value;
            Translation = translation;
            Comment = comment;
            IsNative = isNative;
            IsChecked = isChecked;
            _rootIds = rootIds;
            _wordsByLanguage = new();
        }

        public StemTag(CachedRow stem)
        {
            Id = stem.Stem.Id;
            Value = stem.Stem.Value;
            Translation = stem.Stem.Translation;
            Comment = stem.Stem.Comment;
            IsNative = stem.Stem.IsNative;
            IsChecked = stem.Stem.IsChecked;
            _rootIds = stem.Stem.RootIds;
            _wordsByLanguage = new();

           // foreach (var pair in stem.WordsByLanguage)
           //     _wordsByLanguage[pair.Key] = pair.Value == null ? null : new WordTag(pair.Value);
        }

        public IReadOnlyList<Guid> RootIds => _rootIds;
        public IReadOnlyDictionary<Guid, WordTag?> WordsByLanguage => _wordsByLanguage;
    }
}
