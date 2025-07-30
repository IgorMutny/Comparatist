using Comparatist.Core.Records;

namespace Comparatist.View.Tags
{
    public class CategoryTag
    {
        public readonly Guid Id;
        public readonly string Value;

        public CategoryTag(Category category)
        {
            Id = category.Id;
            Value = category.Value;
        }
    }
}
