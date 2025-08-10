using Comparatist.View.Common;

namespace Comparatist.View.CategoryTree
{
    internal class CategoryTreeRenderer : Renderer<TreeView>
    {
        private Dictionary<CategoryNodeBinder, TreeNode> _categories = new();

        public CategoryTreeRenderer(TreeView tree) : base(tree) { }

        public void Reset()
        {
            _categories.Clear();
            Control.Nodes.Clear();
        }

        public void Add(CategoryNodeBinder binder, CategoryNodeBinder? parent)
        {
            if (parent == null)
            {
                var node = Control.Nodes.Add($"{binder.Category.Value} --- {binder.Order}");
                node.Tag = binder.Category;
                _categories.Add(binder, node);
            }
            else
            {
                if (!_categories.TryGetValue(parent, out var parentNode))
                    return;

                var node = new TreeNode($"{binder.Category.Value} --- {binder.Order}");
                parentNode.Nodes.Insert(binder.Category.Order, node);
                node.Tag = binder.Category;
                _categories.Add(binder, node);
            }
        }

        public void Remove(CategoryNodeBinder binder)
        {
            var node = _categories[binder];
            node.Remove();
            _categories.Remove(binder);
        }

        public void Update(CategoryNodeBinder binder)
        {
            if (!_categories.TryGetValue(binder, out var node))
                return;

            node.Tag = binder.Category;
            node.Text = $"{binder.Category.Value} --- {binder.Order}";
        }

        public void Move(CategoryNodeBinder binder, CategoryNodeBinder? previousBinder)
        {
            var node = _categories[binder];
            var parentNode = node.Parent;
            node.Remove();
            var index = previousBinder != null ? _categories[previousBinder].Index + 1 : 0;

            if (parentNode != null)
                parentNode.Nodes.Insert(index, node);
            else
                Control.Nodes.Insert(index, node);
        }
    }
}
