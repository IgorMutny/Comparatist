using Comparatist.Services.CategoryTree;
using Comparatist.View.Extensions;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryTreeViewAdapter : ViewAdapter
    {
        private TreeView _tree;
        private DisposableMenu _treeMenu;
        private DisposableMenu _nodeMenu;
        private CategoryTreeDragDropHelper _dragDropHelper;

        public event Action<string, CachedCategoryNode?>? AddNodeRequest;
        public event Action<CachedCategoryNode, string>? EditNodeRequest;
        public event Action<CachedCategoryNode, CachedCategoryNode?>? MoveNodeRequest;
        public event Action<CachedCategoryNode>? DeleteNodeRequest;

        public CategoryTreeViewAdapter(TreeView tree)
        {
            _tree = tree;
            _dragDropHelper = new CategoryTreeDragDropHelper(tree, MoveNode);

            _treeMenu = new DisposableMenu(
                ("Add category", AddNode));

            _nodeMenu = new DisposableMenu(
                ("Add child category", AddChildNode),
                ("Edit category", EditNode),
                ("Delete category", DeleteNode));

            SetupTree();
        }

        private void SetupTree()
        {
            _tree.AllowDrop = true;
            _tree.ItemDrag += _dragDropHelper.OnItemDrag;
            _tree.DragOver += _dragDropHelper.OnDragOver;
            _tree.DragDrop += _dragDropHelper.OnDragDrop;
            _tree.MouseUp += OnMouseUp;
        }

        protected override void Unsubscribe()
        {
            _tree.ItemDrag -= _dragDropHelper.OnItemDrag;
            _tree.DragOver -= _dragDropHelper.OnDragOver;
            _tree.DragDrop -= _dragDropHelper.OnDragDrop;
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
            var name = InputBox.Show("Enter the name:", "Add category");

            if (string.IsNullOrWhiteSpace(name))
                return;

            AddNodeRequest?.Invoke(name, null);
        }

        private void AddChildNode()
        {
            if (_tree.SelectedNode?.Tag is not CachedCategoryNode parentNode)
                throw new Exception();

            var name = InputBox.Show(
                $"Enter the name of a child of {parentNode.Category.Value}:",
                "Add category");

            if (string.IsNullOrWhiteSpace(name))
                return;

            AddNodeRequest?.Invoke(name, parentNode);
        }

        private void EditNode()
        {
            if (_tree.SelectedNode?.Tag is not CachedCategoryNode node)
                throw new Exception();

            var name = InputBox.Show(
                $"Enter a new name for {node.Category.Value}:",
                "Edit category",
                node.Category.Value);

            if (string.IsNullOrWhiteSpace(name))
                return;

            EditNodeRequest?.Invoke(node, name);
        }

        private void MoveNode(CachedCategoryNode sourceTag, CachedCategoryNode? targetTag)
        {
            if (targetTag == null)
                SetAsRoot(sourceTag);
            else
                SetAsChildOf(sourceTag, targetTag);
        }

        private void SetAsChildOf(CachedCategoryNode sourceTag, CachedCategoryNode targetTag)
        {
            var confirmation = MessageBox.Show(
                $"Set category {sourceTag.Category.Value} as a child of {targetTag.Category.Value}?",
                "Move category",
                MessageBoxButtons.OKCancel);

            if (confirmation == DialogResult.OK)
                MoveNodeRequest?.Invoke(sourceTag, targetTag);
        }

        private void SetAsRoot(CachedCategoryNode sourceTag)
        {
            var confirmation = MessageBox.Show(
                                $"Set category {sourceTag.Category.Value} as root category?",
                                "Move category",
                                MessageBoxButtons.OKCancel);

            if (confirmation == DialogResult.OK)
                MoveNodeRequest?.Invoke(sourceTag, null);
        }

        private void DeleteNode()
        {
            if (_tree.SelectedNode?.Tag is not CachedCategoryNode node)
                throw new Exception();

            var result = MessageBox.Show(
                $"Delete {node.Category.Value}? All child categories will be also deleted!",
                "Delete category",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
                DeleteNodeRequest?.Invoke(node);
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
