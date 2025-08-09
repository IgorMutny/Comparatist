using Comparatist.Services.Cache;
using Comparatist.View.Infrastructure;
using Comparatist.View.Utilities;

namespace Comparatist.View.WordGrid
{
    internal class WordGridRenderer : Renderer<DataGridView>
    {
        private readonly static Color FilledWordCellColor = Color.White;
        private readonly static Color EmptyWordCellColor = Color.LightGray;
        private readonly static Color CheckedFrontColor = Color.Black;
        private readonly static Color UncheckedFrontColor = Color.DarkGray;

        private Dictionary<CategoryBinder, DataGridViewRow> _categories = new();
        private Dictionary<RootBinder, DataGridViewRow> _roots = new();
        private Dictionary<StemBinder, DataGridViewRow> _stems = new();
        private Dictionary<WordBinder, DataGridViewCell> _words = new();
        private Dictionary<Guid, DataGridViewColumn> _columns = new();

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
            _words.Clear();
            _columns.Clear();
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
                Tag = language.Record,
            };

            Control.Columns.Add(column);
            _columns.Add(language.Record.Id, column);
        }

        public void Add<T>(T binder)
            where T : class, IBinder
        {
            switch (binder)
            {
                case (CategoryBinder categoryBinder):
                    AddCategory(categoryBinder);
                    break;
                default: throw new NotSupportedException();
            }
        }

        public void Add<T1, T2>(T1 binder, T2 parent)
            where T1 : class, IBinder where T2 : class, IBinder
        {
            switch (binder, parent)
            {
                case (RootBinder rootBinder, CategoryBinder categoryBinder):
                    AddRoot(rootBinder, categoryBinder);
                    break;
                case (StemBinder stemBinder, RootBinder rootBinder):
                    AddStem(stemBinder, rootBinder);
                    break;
                case (WordBinder wordBinder, StemBinder stemBinder):
                    AddWord(wordBinder, stemBinder);
                    break;
                default: throw new NotSupportedException();
            }
        }

        private void AddCategory(CategoryBinder binder)
        {
            var index = Control.Rows.Add();
            var row = Control.Rows[index];
            row.DefaultCellStyle.BackColor = Color.LightGreen;

            var cell = row.Cells[0];
            cell.Tag = binder.Category;
            cell.Value = $"[b]{binder.Category.Value.ToUpper()}[/b]";

            _categories.Add(binder, row);
        }

        private void AddRoot(RootBinder binder, CategoryBinder parent)
        {
            var parentRow = _categories[parent];
            var index = parentRow.Index + 1;
            Control.Rows.Insert(index);
            var row = Control.Rows[index];
            _roots.Add(binder, row);

            UpdateRoot(binder);
        }

        private void AddStem(StemBinder binder, RootBinder parent)
        {
            var parentRow = _roots[parent];
            var index = parentRow.Index + 1;
            Control.Rows.Insert(index);
            var row = Control.Rows[index];
            _stems.Add(binder, row);

            for (int i = 1; i < row.Cells.Count; i++)
                row.Cells[i].Style.BackColor = EmptyWordCellColor;

            UpdateStem(binder);
        }

        private void AddWord(WordBinder binder, StemBinder parent)
        {
            var row = _stems[parent];
            var column = _columns[binder.Word.LanguageId];
            var cell = Control.Rows[row.Index].Cells[column.Index];
            cell.Style.BackColor = FilledWordCellColor;
            _words.Add(binder, cell);
            UpdateWord(binder);
        }

        public void Update<T>(T binder)
            where T : class, IBinder
        {
            switch (binder)
            {
                case (CategoryBinder categoryBinder):
                    break;
                case (RootBinder rootBinder):
                    UpdateRoot(rootBinder);
                    break;
                case (StemBinder stemBinder):
                    UpdateStem(stemBinder);
                    break;
                case (WordBinder wordBinder):
                    UpdateWord(wordBinder);
                    break;
                default: throw new NotSupportedException();
            }
        }

        private void UpdateRoot(RootBinder binder)
        {
            if (!_roots.TryGetValue(binder, out var row))
                return;

            var cell = row.Cells[0];
            cell.Tag = binder.Root;
            cell.Style.ForeColor = binder.Root.IsChecked ? CheckedFrontColor : UncheckedFrontColor;
            var open = binder.Root.IsNative ? string.Empty : "《";
            var close = binder.Root.IsNative ? string.Empty : "》";
            cell.Value = $"{binder.ExpandedMark} {open}[b]{binder.Root.Value}[/b] {binder.Root.Translation} {close}";
        }

        private void UpdateStem(StemBinder binder)
        {
            if (!_stems.TryGetValue(binder, out var row))
                return;

            var cell = row.Cells[0];
            cell.Tag = binder.Stem;
            var open = binder.Stem.IsNative ? string.Empty : "《";
            var close = binder.Stem.IsNative ? string.Empty : "》";
            cell.Style.ForeColor = binder.Stem.IsChecked ? CheckedFrontColor : UncheckedFrontColor;
            cell.Value = $"    • {open}[b]{binder.Stem.Value}[/b] {binder.Stem.Translation} {close}";
        }

        private void UpdateWord(WordBinder binder)
        {
            if (!_words.TryGetValue(binder, out var cell))
                return;

            cell.Tag = binder.Word;
            var open = binder.Word.IsNative ? string.Empty : "《";
            var close = binder.Word.IsNative ? string.Empty : "》";
            cell.Style.ForeColor = binder.Word.IsChecked ? CheckedFrontColor : UncheckedFrontColor;
            cell.Value = $"{open}[b]{binder.Word.Value}[/b] {binder.Word.Translation} {close}";
        }

        public void Move<T>(T binder, T? previousBinder) where T : class, IBinder
        {
            switch (binder, previousBinder)
            {
                case (RootBinder rootBinder, RootBinder previousRootBinder):
                    MoveRoot(rootBinder, previousRootBinder);
                    break;
                case (RootBinder rootBinder, null):
                    MoveRoot(rootBinder, null);
                    break;
                case (StemBinder stemBinder, StemBinder previousStemBinder):
                    MoveStem(stemBinder, previousStemBinder);
                    break;
                case (StemBinder stemBinder, null):
                    MoveStem(stemBinder, null);
                    break;
                default: throw new NotSupportedException();
            }
        }

        private void MoveRoot(RootBinder binder, RootBinder? previousBinder)
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

        private void MoveStem(StemBinder binder, StemBinder? previousBinder)
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

        public void Remove<T>(T binder) where T : IBinder
        {
            switch (binder)
            {
                case RootBinder rootBinder: RemoveRoot(rootBinder); break;
                case StemBinder stemBinder: RemoveStem(stemBinder); break;
                case WordBinder wordBinder: RemoveWord(wordBinder); break;
                default: throw new NotSupportedException();
            }
        }

        private void RemoveRoot(RootBinder binder)
        {
            if (!_roots.TryGetValue(binder, out var row))
                return;

            Control.Rows.Remove(row);
            _roots.Remove(binder);
        }

        private void RemoveStem(StemBinder binder)
        {
            if (!_stems.TryGetValue(binder, out var row))
                return;

            Control.Rows.Remove(row);
            _stems.Remove(binder);
        }

        private void RemoveWord(WordBinder binder)
        {
            var cell = _words[binder];
            cell.Value = null;
            cell.Tag = null;
            cell.Style.BackColor = EmptyWordCellColor;
            _words.Remove(binder);
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

