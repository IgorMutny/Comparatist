using Comparatist.Services.Cache;

namespace Comparatist.View.Tags
{
    public class RootTag
    {
        public readonly Guid Id;
        public readonly string Value;
        public readonly string Translation;
        public readonly string Comment;
        public readonly bool IsNative;
        public readonly bool IsChecked;
        private readonly List<Guid> _categoryIds;
        private readonly List<StemTag> _stems;

        public RootTag()
        {
            Id = default;
            Value = string.Empty;
            Translation = string.Empty;
            Comment = string.Empty;
            IsNative = false;
            IsChecked = false;
            _categoryIds = new();
            _stems = new();
        }

        public RootTag(Guid id,
            string value,
            string translation,
            string comment,
            bool isNative,
            bool isChecked,
            List<Guid> categoryIds)
        {
            Id = id;
            Value = value;
            Translation = translation;
            Comment = comment;
            IsNative = isNative;
            IsChecked = isChecked;
            _categoryIds = categoryIds;
            _stems = new();
        }

        public RootTag(CachedRoot root)
        {
            Id = root.Root.Id;
            Value = root.Root.Value;
            Translation = root.Root.Translation;
            Comment = root.Root.Comment;
            IsNative = root.Root.IsNative;
            IsChecked = root.Root.IsChecked;
            _categoryIds = root.Root.CategoryIds;

            _stems = new();
            foreach (var stem in root.Stems)
                _stems.Add(new StemTag(stem));
        }

        public IReadOnlyList<Guid> CategoryIds => _categoryIds;
        public IReadOnlyList<StemTag> Stems => _stems;
    }
}
