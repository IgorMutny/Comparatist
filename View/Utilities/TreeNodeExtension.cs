namespace Comparatist.View.Utilities
{
    public static class TreeNodeExtensions
    {
        public static bool IsSameOrDescendantOf(this TreeNode node, TreeNode possibleAncestor)
        {
            var current = node;

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
