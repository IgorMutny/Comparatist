using Comparatist.Services.CategoryTree;
using Comparatist.View.Extensions;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryTreeDragDropHelper
    {
        private TreeView _tree;
        private Action<CachedCategoryNode, CachedCategoryNode?> _dropAction;

        public CategoryTreeDragDropHelper(
            TreeView tree,
            Action<CachedCategoryNode, CachedCategoryNode?> dropAction)
        {
            _tree = tree;
            _dropAction = dropAction;
        }

        public void OnItemDrag(object? sender, ItemDragEventArgs e)
        {
            if (e.Item is not TreeNode item)
                throw new Exception();

            _tree.DoDragDrop(item, DragDropEffects.Move);
        }

        public void OnDragOver(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetData(typeof(TreeNode)) is not TreeNode sourceNode)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var targetPoint = _tree.PointToClient(new Point(e.X, e.Y));
            var targetNode = _tree.GetNodeAt(targetPoint);

            if (targetNode == null)
                e.Effect = DragDropEffects.Move;
            else if (targetNode.IsSameOrDescendantOf(sourceNode))
                e.Effect = DragDropEffects.None;
            else
                e.Effect = DragDropEffects.Move;
        }


        public void OnDragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetData(typeof(TreeNode)) is not TreeNode source
                || source.Tag is not CachedCategoryNode sourceTag)
            {
                throw new Exception();
            }

            var targetPoint = _tree.PointToClient(new Point(e.X, e.Y));
            var target = _tree.GetNodeAt(targetPoint);

            if (target.IsSameOrDescendantOf(source))
                throw new Exception();

            if (target.Tag is not CachedCategoryNode targetTag)
                _dropAction(sourceTag, null);
            else
                _dropAction(sourceTag, targetTag);
        }
    }
}
