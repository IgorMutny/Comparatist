using Comparatist.Data.Persistence;
using Comparatist.Data.Entities;

namespace Comparatist.Application.Cache
{
    public class CachedWord: IDisplayableCachedRecord, IContentEquatable<CachedWord>
    {
        public required Word Record { get; set; }

        public string Value => Record.Value;
        public string Translation => Record.Translation;
		public string Comment => Record.Comment;
		public bool IsChecked => Record.IsChecked;

        public object Clone()
        {
            return new CachedWord { Record = (Word)Record.Clone() };
        }

        public bool EqualsContent(CachedWord? other)
        {
            if (other == null)
                return false;

            return Record.EqualsContent(other.Record);
        }
    }
}
