namespace Comparatist.View.WordGrid.Binders
{
    internal class RichTextCellPainter: IDisposable
    {
        private DataGridView _grid;

        public RichTextCellPainter(DataGridView grid)
        {
            _grid = grid;
            _grid.CellPainting += OnCellPainting;
        }

        public void Dispose()
        {
            _grid.CellPainting -= OnCellPainting;
        }

        private void OnCellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var value = e.FormattedValue?.ToString();
            if (string.IsNullOrEmpty(value)) return;

            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);

            var formatted = RichTextFormatter.Parse(value);
            var x = e.CellBounds.X + 2;
            var y = e.CellBounds.Y + 2;

            if (e.CellStyle != null && e.Graphics != null)
            {
                foreach (var (text, style) in formatted)
                {
                    using var font = new Font(e.CellStyle.Font, style);
                    var size = TextRenderer.MeasureText(text, font);
                    TextRenderer.DrawText(e.Graphics, text, font, new Point(x, y), e.CellStyle.ForeColor);
                    x += size.Width;
                }
            }
        }
    }
}
