using Comparatist.View.WordGrid.Binders;

namespace Comparatist.View.WordGrid.BinderRenderers
{
    internal class RootBinderRenderer
    {
        private DataGridView _grid;
        private Dictionary<CategoryBinder, DataGridViewRow> _categories;
        private Dictionary<RootBinder, DataGridViewRow> _roots;

        public RootBinderRenderer(
            DataGridView grid,
            Dictionary<CategoryBinder, DataGridViewRow> categories,
            Dictionary<RootBinder, DataGridViewRow> roots)
        {
            _grid = grid;
            _categories = categories;
            _roots = roots;
        }

        public void Add(RootBinder binder, CategoryBinder parent)
        {
            var parentRow = _categories[parent];
            var index = parentRow.Index + 1;
            _grid.Rows.Insert(index);
            var row = _grid.Rows[index];
            _roots.Add(binder, row);

            Update(binder);
        }

        public void Move(RootBinder binder, RootBinder? previousBinder)
        {
            if (previousBinder == null)
                InsertAfterParent(binder, binder.Parent);
            else
                InsertAfterRoot(binder, previousBinder);
        }

        private void InsertAfterParent(RootBinder binder, CategoryBinder category)
        {
            var previousRow = _categories[binder.Parent];
            var row = _roots[binder];

            if (previousRow.Index + 1 == row.Index)
                return;

            _grid.Rows.Remove(row);
            var index = previousRow.Index + 1;
            _grid.Rows.Insert(index, row);
        }

        private void InsertAfterRoot(RootBinder binder, RootBinder previousBinder)
        {
            var previousRow = _roots[previousBinder];
            var row = _roots[binder];

            var offset = previousBinder.IsExpanded ? previousBinder.GetStemCount() + 1 : 1;

            if (previousRow.Index + offset == row.Index)
                return;

            _grid.Rows.Remove(row);
            var index = previousRow.Index + offset;
            _grid.Rows.Insert(index, row);
        }

        public void Remove(RootBinder binder)
        {
            if (!_roots.TryGetValue(binder, out var row))
                return;

            _grid.Rows.Remove(row);
            _roots.Remove(binder);
        }

        public void Update(RootBinder binder)
        {
            if (!_roots.TryGetValue(binder, out var row))
                return;

            var cell = row.Cells[0];
            cell.Tag = binder.Root;
            CellFormatter.FormatCell(cell, binder.CurrentState, binder.ExpandedMark);
        }
    }
}
