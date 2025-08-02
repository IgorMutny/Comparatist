using Comparatist.Core.Records;
using Comparatist.Services.CategoryTree;
using Comparatist.View.Infrastructure;
using Comparatist.View.Utilities;

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
            var treeNode = new TreeNode(node.Category.Value) { Tag = node.Category };

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
            if (Control.SelectedNode?.Tag is not Category parentCategory)
                return;

            var name = InputBox.Show(
                $"Enter the name of a child of {parentCategory.Value}:",
                "Add category");

            if (string.IsNullOrWhiteSpace(name))
                return;

            var category = new Category
            {
                Id = Guid.Empty,
                Value = name,
                ParentId = parentCategory.Id
            };

            AddRequest?.Invoke(category);
        }

        private void EditNode()
        {
            if (Control.SelectedNode?.Tag is not Category category)
                return;

            var name = InputBox.Show(
                $"Enter a new name for {category.Value}:",
                "Edit category",
                category.Value);

            if (string.IsNullOrWhiteSpace(name))
                return;

            var updatedCategory = (Category)category.Clone();
            updatedCategory.Value = name;
            UpdateRequest?.Invoke(updatedCategory);
        }

        private void MoveNode(Category source, Category? target)
        {
            if (target == null)
                SetAsRoot(source);
            else
                SetAsChildOf(source, target);
        }

        private void SetAsRoot(Category source)
        {
            var confirmation = MessageBox.Show(
                                $"Set category {source.Value} as root category?",
                                "Move category",
                                MessageBoxButtons.OKCancel);

            if (confirmation == DialogResult.OK)
            {
                var updatedCategory = (Category)source.Clone();
                updatedCategory.ParentId = Guid.Empty;
                UpdateRequest?.Invoke(updatedCategory);
            }
        }

        private void SetAsChildOf(Category source, Category target)
        {
            var confirmation = MessageBox.Show(
                $"Set category {source.Value} as a child of {target.Value}?",
                "Move category",
                MessageBoxButtons.OKCancel);

            if (confirmation == DialogResult.OK)
            {
                var updatedCategory = (Category)source.Clone();
                updatedCategory.ParentId = target.Id;
                UpdateRequest?.Invoke(updatedCategory);
            }
        }

        private void DeleteNode()
        {
            if (Control.SelectedNode?.Tag is not Category category)
                return;

            var result = MessageBox.Show(
                $"Delete {category.Value}? All child categories will be also deleted!",
                "Delete category",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                var deletedCategory = (Category)category.Clone();
                DeleteRequest?.Invoke(deletedCategory);
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
