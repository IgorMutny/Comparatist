using Comparatist.View.WordGrid.Binders;

namespace Comparatist.View.WordGrid.BinderRenderers
{
    internal class StemBinderRenderer
    {
        private DataGridView _grid;
        private Dictionary<RootBinder, DataGridViewRow> _roots;
        private Dictionary<StemBinder, DataGridViewRow> _stems;

        public StemBinderRenderer(
            DataGridView grid,
            Dictionary<RootBinder, DataGridViewRow> roots,
            Dictionary<StemBinder, DataGridViewRow> stems)
        {
            _grid = grid;
            _roots = roots;
            _stems = stems;
        }

        public void Add(StemBinder binder, RootBinder parent)
        {
            var parentRow = _roots[parent];
            var index = parentRow.Index + 1;
            _grid.Rows.Insert(index);
            var row = _grid.Rows[index];
            _stems.Add(binder, row);

            Update(binder);

            for (int i = 1; i < row.Cells.Count; i++)
                CellFormatter.ColorizeWordCell(row.Cells[i]);
        }

        public void Move(StemBinder binder, StemBinder? previousBinder)
        {
            var previousRow = previousBinder == null
                ? _roots[binder.Parent]
                : _stems[previousBinder];

            var row = _stems[binder];

            if (previousRow.Index + 1 == row.Index)
                return;

            _grid.Rows.Remove(row);
            var index = previousRow.Index + 1;
            _grid.Rows.Insert(index, row);
        }

        public void Remove(StemBinder binder)
        {
            if (!_stems.TryGetValue(binder, out var row))
                return;

            _grid.Rows.Remove(row);
            _stems.Remove(binder);
        }

        public void Update(StemBinder binder)
        {
            if (!_stems.TryGetValue(binder, out var row))
                return;

            var cell = row.Cells[0];
            cell.Tag = binder.Stem;
            CellFormatter.FormatCell(cell, binder.CurrentState, "    •");
        }
    }
}
