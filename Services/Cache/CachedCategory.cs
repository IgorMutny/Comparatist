using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedCategory
    {
        public required Category Record;
        public List<CachedCategory> Children = new();
        public List<CachedRoot> Roots = new();

        public object Clone()
        {
            return new CachedCategory
            {
                Record = (Category)Record.Clone(),
                Children = Children.Select(e => (CachedCategory)e.Clone()).ToList(),
                Roots = Roots.Select(e => (CachedRoot)e.Clone()).ToList()
            };
        }
    }
}
