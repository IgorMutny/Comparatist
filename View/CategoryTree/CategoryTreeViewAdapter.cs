using Comparatist.Core.Records;
using Comparatist.Services.Cache;
using Comparatist.View.Infrastructure;
using Comparatist.View.Utilities;
using Comparatist.View.Utilities.Comparatist.View.Utilities;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryTreeViewAdapter : ViewAdapter<TreeView>
    {
        private DisposableMenu _treeMenu;
        private DisposableMenu _nodeMenu;
        private CategoryTreeDragDropHelper _dragDropHelper;
        private List<Category> _cache = new();

        public event Action<Category>? AddRequest;
        public event Action<Category>? UpdateRequest;
        public event Action<IEnumerable<Category>>? UpdateManyRequest;
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

        public void Render(IReadOnlyList<CachedCategory> rootNodes)
        {
            Control.BeginUpdate();
            Control.Nodes.Clear();
            _cache = new();

            foreach (var root in rootNodes)
                Control.Nodes.Add(CreateTreeNode(root));

            Control.EndUpdate();
            Control.ExpandAll();
        }

        private TreeNode CreateTreeNode(CachedCategory node)
        {
            var treeNode = new TreeNode($"{node.Record.Value} {node.Record.Order}") { Tag = node.Record };
            _cache?.Add(node.Record);

            foreach (var child in node.Children)
                treeNode.Nodes.Add(CreateTreeNode(child.Value));

            return treeNode;
        }

        private void AddNode()
        {
            var name = InputBox.Show("Enter the name:", "Add category");

            if (string.IsNullOrWhiteSpace(name))
                return;

            var category = new Category { Value = name, ParentId = Guid.Empty, Order = _cache.Count };
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
                ParentId = parentCategory.Id,
                Order = parentCategory.Order + 1
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

            category.Value = name;
            UpdateRequest?.Invoke(category);
        }

        private void MoveNode(Category source, Category? target)
        {
            if (target == null)
            {
                SetAsRoot(source);
            }
            else
            {
                var position = DropPositionBox.Show($"Move {source.Value} to {target.Value}:", "Move category");
                switch (position)
                {
                    case DropPositions.Inside:
                        SetAsChildOf(source, target); break;
                    case DropPositions.Before:
                        PlaceBefore(source, target); break;
                    case DropPositions.After:
                        PlaceAfter(source, target); break;
                }
            }
        }

        private void PlaceAfter(Category source, Category target)
        {
            source.ParentId = target.ParentId;
            _cache.Remove(source);
            var newOrder = Math.Min(target.Order, _cache.Count);
            _cache.Insert(newOrder, source);
            ReorderCache();
        }

        private void PlaceBefore(Category source, Category target)
        {
            source.ParentId = target.ParentId;
            _cache.Remove(source);
            var newOrder = Math.Min(target.Order, _cache.Count);
            _cache.Insert(newOrder, source);
            ReorderCache();
        }

        private void SetAsRoot(Category source)
        {
            var confirmation = MessageBox.Show(
                                $"Set category {source.Value} as root category?",
                                "Move category",
                                MessageBoxButtons.OKCancel);

            if (confirmation == DialogResult.OK)
            {
                source.ParentId = Guid.Empty;
                UpdateRequest?.Invoke(source);
            }
        }

        private void ReorderCache()
        {
            for (int i = 0; i < _cache.Count; i++)
                _cache[i].Order = i;

            UpdateManyRequest?.Invoke(_cache);
        }

        private void SetAsChildOf(Category source, Category target)
        {
            source.ParentId = target.Id;
            UpdateRequest?.Invoke(source);
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
                DeleteRequest?.Invoke(category);
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
