using Comparatist.Services.CategoryTree;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryTreeViewAdapter : ViewAdapter
    {
        private TreeView _tree;
        private DisposableMenu _treeMenu;
        private DisposableMenu _nodeMenu;

        public event Action<CachedCategoryNode, CachedCategoryNode?>? MoveNodeRequest;
        public event Action<string>? AddNodeRequest;

        public CategoryTreeViewAdapter(TreeView tree)
        {
            _tree = tree;
            _treeMenu = new DisposableMenu(("Add node", AddNode));
            _nodeMenu = new DisposableMenu();
            SetupTree();
        }

        private void SetupTree()
        {
            _tree.DragDrop += OnDragDrop;
            _tree.MouseUp += OnMouseUp;
        }

        protected override void Unsubscribe()
        {
            _tree.DragDrop -= OnDragDrop;
            _tree.MouseUp -= OnMouseUp;
            _treeMenu.Dispose();
        }

        public void Render(IReadOnlyList<CachedCategoryNode> rootNodes)
        {
            _tree.BeginUpdate();
            _tree.Nodes.Clear();
            foreach (var root in rootNodes)
                _tree.Nodes.Add(CreateTreeNode(root));
            _tree.EndUpdate();
            _tree.ExpandAll();
        }

        private TreeNode CreateTreeNode(CachedCategoryNode node)
        {
            var treeNode = new TreeNode(node.Category.Value) { Tag = node };

            foreach (var child in node.Children)
                treeNode.Nodes.Add(CreateTreeNode(child));

            return treeNode;
        }

        private void AddNode()
        {
            var name = InputBox.Show("Add node", "Enter the name:");
            if (string.IsNullOrWhiteSpace(name))
                return;

            AddNodeRequest?.Invoke(name);
        }

        private void AddNodeAsChild()
        {

        }

        private void EditNode()
        {

        }

        private void MoveNode()
        {

        }

        private void DeleteNode()
        {

        }

        private void OnDragDrop(object? sender, DragEventArgs e)
        {
            var source = _tree.SelectedNode;
            var targetPoint = _tree.PointToClient(new Point(e.X, e.Y));
            var target = _tree.GetNodeAt(targetPoint);

            if (source.Tag is not CachedCategoryNode sourceTag)
                throw new Exception();

            if (target == null)
            {
                MoveNodeRequest?.Invoke(sourceTag, null);
            }
            else
            {
                if (target.Tag is not CachedCategoryNode targetTag)
                    throw new Exception();

                MoveNodeRequest?.Invoke(sourceTag, targetTag);
            }
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var point = new Point(e.X, e.Y);
            var node = _tree.GetNodeAt(point);

            if (node == null)
            {
                _tree.SelectedNode = null;
                _treeMenu.Show(_tree, point);
            }
            else
            {
                _tree.SelectedNode = node;
                _nodeMenu.Show(_tree, point);
            }

        }
    }
}
