using Comparatist.Data.Entities;
using Comparatist.View.Utilities;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryTreeDragDropHelper
    {
        private TreeView _tree;
        private Action<Category, Category?> _dropAction;

        public CategoryTreeDragDropHelper(
            TreeView tree,
            Action<Category, Category?> dropAction)
        {
            _tree = tree;
            _dropAction = dropAction;
        }

        public void OnItemDrag(object? sender, ItemDragEventArgs e)
        {
            if (e.Item is not TreeNode item)
                return;

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
                || source.Tag is not Category sourceCategory)
            {
                return;
            }

            var targetPoint = _tree.PointToClient(new Point(e.X, e.Y));
            var target = _tree.GetNodeAt(targetPoint);

            if (target.IsSameOrDescendantOf(source))
                return;

            if (target == null || target.Tag is not Category targetCategory)
                _dropAction(sourceCategory, null);
            else
                _dropAction(sourceCategory, targetCategory);
        }
    }
}
