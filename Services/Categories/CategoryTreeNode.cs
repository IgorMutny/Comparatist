using Comparatist.Core.Records;

namespace Comparatist.Services.CategoryTree
{
    public class CategoryTreeNode
    {
        public required Category Category;
        public List<CategoryTreeNode> Children = new();
    }
}
