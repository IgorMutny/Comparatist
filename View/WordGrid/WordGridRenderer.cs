using Comparatist.Services.Cache;
using Comparatist.View.Infrastructure;
using Comparatist.View.Utilities;

namespace Comparatist.View.WordGrid
{
    internal class WordGridRenderer : Renderer<DataGridView>
    {
        private Dictionary<CategoryBinder, DataGridViewRow> _categories = new();
        private Dictionary<RootBinder, DataGridViewRow> _roots = new();
        private Dictionary<StemBinder, DataGridViewRow> _stems = new();

        public WordGridRenderer(DataGridView control) : base(control)
        {
            Control.Dock = DockStyle.Fill;
            Control.AllowUserToAddRows = false;
            Control.RowHeadersVisible = false;
            Control.AutoGenerateColumns = false;
            Control.MultiSelect = false;
            Control.ReadOnly = true;
            Control.Visible = false;

            Control.CellPainting += OnCellPainting;
        }

        public override void Dispose()
        {
            Control.CellPainting -= OnCellPainting;
        }

        public void Reset()
        {
            Control.Columns.Clear();
            Control.Rows.Clear();
            _categories.Clear();
            _roots.Clear();
            _stems.Clear();
        }

        public void CreateColumns(List<CachedLanguage> languages)
        {
            AddRowHeaderColumn();

            foreach (var language in languages)
                AddColumn(language);
        }

        private void AddRowHeaderColumn()
        {
            var rowHeaderColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = string.Empty,
                Width = 200
            };

            Control.Columns.Add(rowHeaderColumn);
        }

        private void AddColumn(CachedLanguage language)
        {
            var column = new DataGridViewTextBoxColumn
            {
                HeaderText = language.Record.Value,
                Width = 200,
                Tag = language
            };

            Control.Columns.Add(column);
        }

        public void AddCategory(CategoryBinder binder)
        {
            var index = Control.Rows.Add();
            var row = Control.Rows[index];
            row.DefaultCellStyle.BackColor = Color.LightGreen;

            var cell = row.Cells[0];
            cell.Tag = binder.Category;
            cell.Value = binder.Category.Value.ToUpper();

            _categories.Add(binder, row);
        }

        public void AddRoot(RootBinder binder, CategoryBinder parent)
        {
            var parentRow = _categories[parent];
            var index = parentRow.Index + 1;
            Control.Rows.Insert(index);
            var row = Control.Rows[index];
            _roots.Add(binder, row);

            UpdateRoot(binder);
        }

        public void UpdateRoot(RootBinder binder)
        {
            if (!_roots.TryGetValue(binder, out var row))
                return;

            var cell = row.Cells[0];
            cell.Tag = binder.Root;
            cell.Value = $"[b]{binder.Root.Value}[/b] {binder.Root.Translation}";
        }

        public void MoveRoot(RootBinder binder, RootBinder? previousBinder)
        {
            var previousRow = previousBinder == null
                ? _categories[binder.Parent]
                : _roots[previousBinder];

            var row = _roots[binder];

            if (previousRow.Index + 1 == row.Index)
                return;

            Control.Rows.Remove(row);
            var index = previousRow.Index + 1;
            Control.Rows.Insert(index, row);
        }

        public void RemoveRoot(RootBinder binder)
        {
            if (!_roots.TryGetValue(binder, out var row))
                return;

            Control.Rows.Remove(row);
            _roots.Remove(binder);
        }

        public void AddStem(StemBinder binder, RootBinder parent)
        {
            var parentRow = _roots[parent];
            var index = parentRow.Index + 1;
            Control.Rows.Insert(index);
            var row = Control.Rows[index];
            _stems.Add(binder, row);
            UpdateStem(binder);
        }

        public void UpdateStem(StemBinder binder)
        {
            if (!_stems.TryGetValue(binder, out var block))
                return;

            var cell = block.Cells[0];
            cell.Tag = binder.Stem;
            cell.Value = $"→ [b]{binder.Stem.Value}[/b] {binder.Stem.Translation}";
        }

        public void MoveStem(StemBinder binder, StemBinder? previousBinder)
        {
            var previousRow = previousBinder == null
                ? _roots[binder.Parent]
                : _stems[previousBinder];

            var row = _stems[binder];

            if (previousRow.Index + 1 == row.Index)
                return;

            Control.Rows.Remove(row);
            var index = previousRow.Index + 1;
            Control.Rows.Insert(index, row);
        }

        public void RemoveStem(StemBinder binder)
        {
            if (!_stems.TryGetValue(binder, out var row))
                return;

            Control.Rows.Remove(row);
            _stems.Remove(binder);
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

