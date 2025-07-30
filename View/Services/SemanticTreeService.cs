using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist
{
    public class SemanticTreeService
    {
        private TreeView _tree;
        private IRepository<Category> _repository;
        private ContextMenuStrip _treeMenu;
        private ContextMenuStrip _nodeMenu;
        private bool _isMoveMode = false;
        private TreeNode? _nodeToMove;
        private Action<object, DragDropEffects> _doDragDrop;

        public SemanticTreeService(
            TreeView tree,
            IRepository<Category> repository,
            ContextMenuStrip treeMenu,
            ContextMenuStrip nodeMenu,
            Action<object, DragDropEffects> doDragDrop)
        {
            _tree = tree;
            _repository = repository;
            _treeMenu = treeMenu;
            _nodeMenu = nodeMenu;
            _doDragDrop = doDragDrop;
            SetupTree();
        }

        private void SetupTree()
        {
            _tree.AllowDrop = true;
            _tree.ItemDrag += DragGroup;
            _tree.MouseUp += OnTreeViewMouseUp;
            _tree.DragDrop += DragDropGroup;
            _tree.DragEnter += EnterDragGroup;
            _tree.DragOver += DragOverGroup;
        }

        public void Refresh()
        {
            _tree.BeginUpdate();
            _tree.Nodes.Clear();

            var nodeMap = new Dictionary<Category, TreeNode>();

            foreach (var record in _repository.GetAll())
            {
                var node = new TreeNode(record.Value) { Tag = record.Id };
                nodeMap[record] = node;
            }

            foreach (var pair in nodeMap)
            {
                var record = pair.Key;
                var parentId = record.ParentId;

                if (parentId == null)
                {
                    _tree.Nodes.Add(pair.Value);
                }
                else if (_repository.TryGet(parentId.Value, out var parentGroup)
                    && nodeMap.TryGetValue(parentGroup, out var parentNode))
                {
                    parentNode.Nodes.Add(pair.Value);
                }
            }

            _tree.EndUpdate();
            _tree.ExpandAll();
        }
        public void AddHead()
        {
            var input = InputBox.Show("New semantic group", "Will be a root group");

            if (string.IsNullOrWhiteSpace(input))
                return;

            var newGroup = new Category
            {
                Value = input,
                ParentId = null
            };

            _repository.Add(newGroup);
            Refresh();
        }

        public void AddChild()
        {
            if (_tree.SelectedNode?.Tag is not Guid parentId ||
                !_repository.TryGet(parentId, out var parent))
                return;

            var input = InputBox.Show("New semantic group", $"As child of {parent.Value}");

            if (string.IsNullOrWhiteSpace(input))
                return;

            var newGroup = new Category
            {
                Value = input,
                ParentId = parentId
            };

            _repository.Add(newGroup);
            Refresh();
        }

        public void Edit()
        {
            if (_tree.SelectedNode?.Tag is not Guid id ||
                !_repository.TryGet(id, out var group))
                return;

            var input = InputBox.Show("Edit group", $"New name for {group.Value}", group.Value);

            if (string.IsNullOrWhiteSpace(input))
                return;

            group.Value = input;
            Refresh();
        }

        public void Move()
        {
            if (_tree.SelectedNode == null)
                return;

            _isMoveMode = true;
            _nodeToMove = _tree.SelectedNode;
        }

        public void Delete()
        {
            if (_tree.SelectedNode?.Tag is not Guid id ||
                !_repository.TryGet(id, out var group))
                return;

            var result = MessageBox.Show(
                $"Delete {group.Value}",
                "Delete semantic group?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                //_repository.Delete(id);
                Refresh();
            }
        }
        private void OnTreeViewMouseUp(object? sender, EventArgs e)
        {
            var mouseEvent = (MouseEventArgs)e;

            if (mouseEvent.Button != MouseButtons.Right)
                return;

            TreeNode clickedNode = _tree.GetNodeAt(mouseEvent.X, mouseEvent.Y);

            if (clickedNode == null)
            {
                _tree.SelectedNode = null;
                _treeMenu.Show(_tree, mouseEvent.Location);
            }
            else
            {
                _tree.SelectedNode = clickedNode;
                _nodeMenu.Show(_tree, mouseEvent.Location);
            }
        }

        private void DragGroup(object? sender, ItemDragEventArgs e)
        {
            if (_isMoveMode && e.Item is TreeNode node)
                _doDragDrop(node, DragDropEffects.Move);
        }

        private void EnterDragGroup(object? sender, DragEventArgs e)
        {
            if (e.Data != null && _isMoveMode && e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
        }

        private void DragOverGroup(object? sender, DragEventArgs e)
        {
            if (!_isMoveMode)
                return;

            Point pt = _tree.PointToClient(new Point(e.X, e.Y));
            _tree.SelectedNode = _tree.GetNodeAt(pt);
        }

        private void DragDropGroup(object? sender, DragEventArgs e)
        {
            if (!_isMoveMode || _nodeToMove?.Tag is not Guid sourceId)
                return;

            Point point = _tree.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = _tree.GetNodeAt(point);

            if (targetNode == _nodeToMove)
                return;

            if (targetNode != null && targetNode.Bounds.Contains(point))
                MoveToParentNode(sourceId, targetNode);
            else
                MoveToRoot(sourceId);

            _isMoveMode = false;
            _nodeToMove = null;
        }

        private void MoveToRoot(Guid sourceId)
        {
            if (_nodeToMove == null)
                return;

            var confirm = MessageBox.Show(
                $"Move \"{_nodeToMove.Text}\" to the root?",
                "Move semantic group?",
                MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes && _repository.TryGet(sourceId, out var group))
            {
                group.ParentId = null;
                Refresh();
            }
        }

        private void MoveToParentNode(Guid sourceId, TreeNode targetNode)
        {
            if (_nodeToMove == null)
                return;

            var confirm = MessageBox.Show(
                $"Move \"{_nodeToMove.Text}\" into \"{targetNode.Text}\"?",
                "Move semantic group?",
                MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes
                && targetNode.Tag is Guid targetId
                && _repository.TryGet(sourceId, out var group))
            {
                if (IsDescendant(targetId, sourceId))
                {
                    MessageBox.Show("Moving forbidden", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _isMoveMode = false;
                    _nodeToMove = null;
                    return;
                }

                group.ParentId = targetId;
                Refresh();
            }
        }

        private bool IsDescendant(Guid childId, Guid parentId)
        {
            Guid? currentId = childId;

            while (currentId.HasValue)
            {
                if (currentId == parentId)
                    return true;

                if (_repository.TryGet(currentId.Value, out var currentGroup))
                    currentId = currentGroup.ParentId;
                else
                    break;
            }

            return false;
        }
    }
}
