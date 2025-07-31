namespace Comparatist.View.Extensions
{
    public static class TreeNodeExtensions
    {
        public static bool IsDescendantOf(this TreeNode node, TreeNode possibleAncestor)
        {
            var current = node.Parent;
            while (current != null)
            {
                if (current == possibleAncestor)
                    return true;
                current = current.Parent;
            }
            return false;
        }
    }
}
