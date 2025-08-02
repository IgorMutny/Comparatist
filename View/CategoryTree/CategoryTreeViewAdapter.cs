using Comparatist.Core.Records;
using Comparatist.Services.CategoryTree;
using Comparatist.View.Infrastructure;
using Comparatist.View.Utities;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryTreeViewAdapter : ViewAdapter<TreeView>
    {
        private DisposableMenu _treeMenu;
        private DisposableMenu _nodeMenu;
        private CategoryTreeDragDropHelper _dragDropHelper;

        public event Action<Category>? AddRequest;
        public event Action<Category>? UpdateRequest;
        public event Action<Category>? DeleteRequest;

        public CategoryTreeViewAdapter(TreeView tree) : base(tree)
        {
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
            Control.Dock = DockStyle.Fill;
            Control.AllowDrop = true;
            Control.Visible = false;
            Control.ItemDrag += _dragDropHelper.OnItemDrag;
            Control.DragOver += _dragDropHelper.OnDragOver;
            Control.DragDrop += _dragDropHelper.OnDragDrop;
            Control.MouseUp += OnMouseUp;
        }

        protected override void Unsubscribe()
        {
            Control.ItemDrag -= _dragDropHelper.OnItemDrag;
            Control.DragOver -= _dragDropHelper.OnDragOver;
            Control.DragDrop -= _dragDropHelper.OnDragDrop;
            Control.MouseUp -= OnMouseUp;
            _treeMenu.Dispose();
            _nodeMenu.Dispose();
        }

        public void Render(IReadOnlyList<CachedCategoryNode> rootNodes)
        {
            Control.BeginUpdate();
            Control.Nodes.Clear();

            foreach (var root in rootNodes)
                Control.Nodes.Add(CreateTreeNode(root));

            Control.EndUpdate();
            Control.ExpandAll();
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

            var category = new Category { Value = name, ParentId = Guid.Empty };
            AddRequest?.Invoke(category);
        }

        private void AddChildNode()
        {
            if (Control.SelectedNode?.Tag is not CachedCategoryNode parentNode)
                return;

            var name = InputBox.Show(
                $"Enter the name of a child of {parentNode.Category.Value}:",
                "Add category");

            if (string.IsNullOrWhiteSpace(name))
                return;

            var category = new Category
            {
                Id = Guid.Empty,
                Value = name,
                ParentId = parentNode.Category.Id
            };

            AddRequest?.Invoke(category);
        }

        private void EditNode()
        {
            if (Control.SelectedNode?.Tag is not CachedCategoryNode node)
                return;

            var name = InputBox.Show(
                $"Enter a new name for {node.Category.Value}:",
                "Edit category",
                node.Category.Value);

            if (string.IsNullOrWhiteSpace(name))
                return;

            var category = (Category)node.Category.Clone();
            category.Value = name;
            UpdateRequest?.Invoke(category);
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
            {
                var category = (Category)sourceTag.Category.Clone();
                category.ParentId = targetTag.Category.Id;
                UpdateRequest?.Invoke(category);
            }
        }

        private void SetAsRoot(CachedCategoryNode sourceTag)
        {
            var confirmation = MessageBox.Show(
                                $"Set category {sourceTag.Category.Value} as root category?",
                                "Move category",
                                MessageBoxButtons.OKCancel);

            if (confirmation == DialogResult.OK)
            {
                var category = (Category)sourceTag.Category.Clone();
                category.ParentId = Guid.Empty;
                UpdateRequest?.Invoke(category);
            }
        }

        private void DeleteNode()
        {
            if (Control.SelectedNode?.Tag is not CachedCategoryNode node)
                return;

            var result = MessageBox.Show(
                $"Delete {node.Category.Value}? All child categories will be also deleted!",
                "Delete category",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                var category = (Category)node.Category.Clone();
                DeleteRequest?.Invoke(category);
            }
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var point = new Point(e.X, e.Y);
            var node = Control.GetNodeAt(point);

            if (node == null)
            {
                Control.SelectedNode = null;
                _treeMenu.Show(Control, point);
            }
            else
            {
                Control.SelectedNode = node;
                _nodeMenu.Show(Control, point);
            }
        }
    }
}
