using Comparatist.Data.Entities;
using Comparatist.View.Common;
using Comparatist.View.Utilities;
using Comparatist.View.Utilities.Comparatist.View.Utilities;
using System.Diagnostics.Tracing;
using System.Text;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryTreeInputHandler : InputHandler<TreeView>
    {
        public event Action<Category>? AddRequest;
        public event Action<Category>? UpdateRequest;
        public event Action<IEnumerable<Category>>? UpdateManyRequest;
        public event Action<Category>? DeleteRequest;

        private DisposableMenu _treeMenu;
        private DisposableMenu _nodeMenu;

        public CategoryTreeInputHandler(TreeView tree) : base(tree)
        {
            _treeMenu = new DisposableMenu(
                ("Add category", AddNode));

            _nodeMenu = new DisposableMenu(
                ("Add child category", AddChildNode),
                ("Edit category", EditNode),
                ("Delete category", DeleteNode));

            Control.AllowDrop = true;
            Control.Visible = false;

            Control.ItemDrag += OnItemDrag;
            Control.DragOver += OnDragOver;
            Control.DragDrop += OnDragDrop;
            Control.MouseUp += OnMouseUp;
        }

        private void AddNode()
        {
            var name = InputBox.Show("Enter the name:", "Add category");

            if (string.IsNullOrWhiteSpace(name))
                return;

            var category = new Category
            {
                Value = name,
                ParentId = Guid.Empty,
                Order = Control.Nodes.Count
            };
            AddRequest?.Invoke(category);
        }

        public override void Dispose()
        {
            Control.ItemDrag -= OnItemDrag;
            Control.DragOver -= OnDragOver;
            Control.DragDrop -= OnDragDrop;
            Control.MouseUp -= OnMouseUp;
            _treeMenu.Dispose();
            _nodeMenu.Dispose();
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

            var order = Control.SelectedNode.Nodes.Count;

            var category = new Category
            {
                Id = Guid.Empty,
                Value = name,
                ParentId = parentCategory.Id,
                Order = order
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

        public void OnItemDrag(object? sender, ItemDragEventArgs e)
        {
            if (e.Item is not TreeNode item)
                return;

            Control.DoDragDrop(item, DragDropEffects.Move);
        }

        public void OnDragOver(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetData(typeof(TreeNode)) is not TreeNode sourceNode)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var targetPoint = Control.PointToClient(new Point(e.X, e.Y));
            var targetNode = Control.GetNodeAt(targetPoint);

            if (targetNode == null)
                e.Effect = DragDropEffects.Move;
            else if (targetNode.IsSameOrDescendantOf(sourceNode))
                e.Effect = DragDropEffects.None;
            else
                e.Effect = DragDropEffects.Move;
        }

        public void OnDragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetData(typeof(TreeNode)) is not TreeNode source)
            {
                return;
            }

            var targetPoint = Control.PointToClient(new Point(e.X, e.Y));
            var target = Control.GetNodeAt(targetPoint);

            if (target.IsSameOrDescendantOf(source))
                return;

            MoveNode(source, target);
        }


        private void MoveNode(TreeNode source, TreeNode? target)
        {
            if (target == null)
            {
                SetAsBaseNode(source);
            }
            else
            {
                if (source.Tag is not Category sourceCategory
                    || target.Tag is not Category targetCategory)
                {
                    return;
                }

                var position = DropPositionBox.Show(
                    $"Move {sourceCategory.Value} to {targetCategory.Value}:",
                    "Move category");

                switch (position)
                {
                    case DropPositions.Inside:
                        SetAsChildNode(source, target); break;
                    case DropPositions.Before:
                        PlaceNear(source, target, 0); break;
                    case DropPositions.After:
                        PlaceNear(source, target, 1); break;
                }
            }
        }

        private void PlaceNear(TreeNode source, TreeNode target, int offset)
        {
            if (source.Tag is not Category sourceCategory
                || target.Tag is not Category targetCategory)
            {
                return;
            }

            int insertIndex = target.Index + offset;
            InsertAndReorder(target.Parent, sourceCategory, insertIndex);
        }

        private void SetAsBaseNode(TreeNode node)
        {
            if (node.Tag is not Category category)
                return;

            var confirmation = MessageBox.Show(
                                $"Set category {category.Value} as root category?",
                                "Move category",
                                MessageBoxButtons.OKCancel);

            if (confirmation == DialogResult.OK)
            {
                var index = Control.Nodes.Count;
                InsertAndReorder(null, category, index);
            }
        }

        private void SetAsChildNode(TreeNode source, TreeNode target)
        {
            if (source.Tag is not Category sourceCategory
                || target.Tag is not Category targetCategory)
            {
                return;
            }

            var index = target.Nodes.Count;
            InsertAndReorder(target, sourceCategory, index);
        }

        private void InsertAndReorder(
            TreeNode? parent,
            Category insertedCategory,
            int insertIndex)
        {
            var nodes = parent == null ? Control.Nodes : parent.Nodes;
            var parentId = parent == null ? Guid.Empty : ((Category)parent.Tag).Id;
            var result = new List<Category>();

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Tag is not Category category || category == insertedCategory)
                    continue;

                category.Order = i < insertIndex ? i : i + 1;
                result.Add(category);
            }

            insertedCategory.ParentId = parentId;
            insertedCategory.Order = insertIndex;
            result.Add(insertedCategory);

            UpdateManyRequest?.Invoke(result);
        }
    }
}
