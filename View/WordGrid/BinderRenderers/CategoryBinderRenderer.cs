using Comparatist.View.WordGrid.Binders;

namespace Comparatist.View.WordGrid.BinderRenderers
{
    internal class CategoryBinderRenderer
    {
        private DataGridView _grid;
        private Dictionary<CategoryBinder, DataGridViewRow> _categories;

        public CategoryBinderRenderer(
            DataGridView grid,
            Dictionary<CategoryBinder, DataGridViewRow> categories)
        {
            _grid = grid;
            _categories = categories;
        }

        public void Add(CategoryBinder binder)
        {
            var index = _grid.Rows.Add();
            var row = _grid.Rows[index];
            row.DefaultCellStyle.BackColor = Color.LightGreen;

            var cell = row.Cells[0];
            cell.Tag = binder.Category;
            cell.Value = $"[b]{binder.Category.Value.ToUpper()}";

            _categories.Add(binder, row);
        }
    }
}
