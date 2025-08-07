using Comparatist.Services.Cache;
using Comparatist.View.Infrastructure;
using Comparatist.View.Utilities;

namespace Comparatist.View.WordGrid
{
    internal class WordGridRenderer : Renderer<DataGridView>
    {
        private Dictionary<SectionBinder, DataGridViewRow> _sections = new();
        private Dictionary<BlockBinder, DataGridViewRow> _blocks = new();

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
            _sections.Clear();
            _blocks.Clear();
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

        public void AddSection(SectionBinder binder)
        {
            var index = Control.Rows.Add();
            var row = Control.Rows[index];
            row.DefaultCellStyle.BackColor = Color.LightGreen;

            var cell = row.Cells[0];
            cell.Tag = binder.Category;
            cell.Value = binder.Category.Value.ToUpper();

            _sections.Add(binder, row);
        }

        public void AddBlock(BlockBinder binder, SectionBinder parent)
        {
            var parentRow = _sections[parent];
            var index = parentRow.Index + 1;
            Control.Rows.Insert(index);
            var row = Control.Rows[index];
            _blocks.Add(binder, row);

            UpdateBlock(binder);
        }

        public void UpdateBlock(BlockBinder binder)
        {
            if (!_blocks.TryGetValue(binder, out var block))
                return;

            var cell = block.Cells[0];
            cell.Tag = binder.Root;
            cell.Value = $"[b]{binder.Root.Value}[/b] {binder.Root.Translation}";
        }

        public void MoveBlock(BlockBinder binder, BlockBinder? previousBinder)
        {
            var previousRow = previousBinder == null
                ? _sections[binder.Parent]
                : _blocks[previousBinder];

            var row = _blocks[binder];

            if (previousRow.Index + 1 == row.Index)
                return;

            Control.Rows.Remove(row);
            var index = previousRow.Index + 1;
            Control.Rows.Insert(index, row);
        }

        public void RemoveBlock(BlockBinder binder)
        {
            if (!_blocks.TryGetValue(binder, out var row))
                return;

            Control.Rows.Remove(row);
            _blocks.Remove(binder);
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

