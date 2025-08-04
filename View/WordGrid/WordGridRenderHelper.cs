using Comparatist.Core.Records;
using Comparatist.Services.Cache;
using Comparatist.View.Utilities;

namespace Comparatist.View.WordGrid
{
    internal class WordGridRenderHelper
    {
        private DataGridView _grid;

        private Dictionary<Guid, CachedRoot> _blocks = new();
        private List<Guid> _expandedRootIds = new();
        private int _selectedRowIndex = 0;
        private int _selectedColumnIndex = 0;
        private int _firstRenderedRowIndex = 0;
        private int _firstRenderedColumnIndex = 0;

        public WordGridRenderHelper(DataGridView grid)
        {
            _grid = grid;
        }

        public void Render(IEnumerable<CachedCategory> sections, IEnumerable<Language> languages)
        {
            SaveSelection();

            _blocks.Clear();
            _grid.Columns.Clear();
            _grid.Rows.Clear();

            AddColumns(languages);
            AddSections(sections);

            RestoreSelection();
        }

        public void HandleDoubleClick(Root root, int rowIndex)
        {
            if (_expandedRootIds.Contains(root.Id))
                Collapse(root, rowIndex);
            else
                Expand(root, rowIndex);
        }

        public void OnCellPainting(DataGridViewCellPaintingEventArgs e)
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

        private void AddColumns(IEnumerable<Language> languages)
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

            _grid.Columns.Add(rowHeaderColumn);
        }

        private void AddColumn(Language language)
        {
            var column = new DataGridViewTextBoxColumn
            {
                HeaderText = language.Value,
                Width = 200,
                Tag = language
            };

            _grid.Columns.Add(column);
        }

        private void AddSections(IEnumerable<CachedCategory> sections)
        {
            foreach (var section in sections)
                AddSection(section);
        }

        private void AddSection(CachedCategory section)
        {
            int index = _grid.Rows.Add();
            var row = _grid.Rows[index];
            FillSectionHeaderCell(section.Record, row);
            AddBlocks(section.Roots.Values);

            foreach (var child in section.Children)
                AddSection(child.Value);
        }

        private void FillSectionHeaderCell(Category category, DataGridViewRow row)
        {
            row.Cells[0].Value = category.Value.ToUpper();
        }

        private void AddBlocks(IEnumerable<CachedRoot> blocks)
        {
            foreach (var block in blocks)
                AddBlock(block);
        }

        private void AddBlock(CachedRoot block)
        {
            if (!_blocks.ContainsKey(block.Record.Id))
                _blocks.Add(block.Record.Id, block);

            var isExpanded = _expandedRootIds.Contains(block.Record.Id);
            var index = _grid.Rows.Add();
            AddBlockHeaderRow(block.Record, index, isExpanded);

            if (isExpanded)
                Expand(block.Record, index);

            AddEmptyRow();
        }

        private void AddBlockHeaderRow(Root root, int rowIndex, bool isExpanded)
        {
            var row = _grid.Rows[rowIndex];
            FillBlockHeaderCell(root, row);
            FillCellsInRootRow(row, isExpanded);
        }

        private void FillBlockHeaderCell(Root root, DataGridViewRow row)
        {
            var cell = row.Cells[0];
            cell.Value = $"[b]{root.Value}[/b] {root.Translation}";
            cell.Tag = root;
            ColorizeCell(root, cell);
            ColorizeCellText(root, cell);
        }

        private void FillRowHeader(DataGridViewRow row, Stem stem)
        {
            var cell = row.Cells[0];
            cell.Value = $"→ [b]{stem.Value}[/b] {stem.Translation}";
            cell.Tag = stem;
            ColorizeCell(stem, cell);
            ColorizeCellText(stem, cell);
        }

        private void FillCell(CachedStem cachedRow, DataGridViewRow row, DataGridViewColumn column)
        {
            if (column.Tag is not Language language)
                return;

            var languageId = language.Id;
            var columnIndex = column.Index;

            if (cachedRow.WordsByLanguage.TryGetValue(languageId, out var word) && word != null)
                FillWordCell(row, columnIndex, word.Record);
            else
                FillEmptyCell(row, columnIndex, languageId, cachedRow.Record.Id);
        }

        private void FillWordCell(DataGridViewRow row, int columnIndex, Word word)
        {
            var cell = row.Cells[columnIndex];
            cell.Value = $"[b]{word.Value}[/b] {word.Translation}";
            cell.Tag = word;

            ColorizeCell(word, cell);
            ColorizeCellText(word, cell);
        }

        private void FillEmptyCell(DataGridViewRow row, int columnIndex, Guid languageId, Guid stemId)
        {
            var cell = row.Cells[columnIndex];
            cell.Value = string.Empty;
            cell.Style.BackColor = Color.LightGray;
            cell.Tag = new Word
            {
                Id = Guid.Empty,
                StemId = stemId,
                LanguageId = languageId
            };
        }

        private void AddEmptyRow()
        {
            var index = _grid.Rows.Add();
        }

        private void Expand(Root root, int rowIndex)
        {
            if (!_blocks.TryGetValue(root.Id, out var block))
                return;

            _expandedRootIds.Add(root.Id);
            int index = rowIndex + 1;

            if (block.Stems.Count == 0)
            {
                AddNoStemRow(block.Record, index);
            }
            else
            {
                var rows = block.Stems.Values.OrderBy(e => e.Record.Value).ToList();

                for (int i = rows.Count - 1; i >= 0; i--)
                    AddStemRow(rows[i], index);
            }

            FillCellsInRootRow(_grid.Rows[rowIndex], true);
        }

        private void Collapse(Root root, int rowIndex)
        {
            if (!_blocks.TryGetValue(root.Id, out var block))
                return;

            _expandedRootIds.Remove(root.Id);
            int index = rowIndex + 1;
            var rowCount = block.Stems.Count > 0 ? block.Stems.Count : 1;

            for (int i = 0; i < rowCount; i++)
                _grid.Rows.RemoveAt(index);

            FillCellsInRootRow(_grid.Rows[rowIndex], false);
        }

        private void AddStemRow(CachedStem cachedRow, int index)
        {
            _grid.Rows.Insert(index);
            var row = _grid.Rows[index];
            FillRowHeader(row, cachedRow.Record);

            foreach (DataGridViewColumn column in _grid.Columns)
                FillCell(cachedRow, row, column);
        }

        private void AddNoStemRow(Root root, int index)
        {
            _grid.Rows.Insert(index);
            var row = _grid.Rows[index];
            row.Cells[0].Value = "→ [i]no stems[/i]";
        }

        private void FillCellsInRootRow(DataGridViewRow row, bool isExpanded)
        {
            if (_grid.Columns.Count <= 1)
                return;

            for (int i = 1; i < _grid.Columns.Count; i++)
            {
                row.Cells[i].Tag = row.Cells[0].Tag;

                if (!isExpanded)
                    row.Cells[i].Value = "↓ ↓ ↓";
                else
                    row.Cells[i].Value = "↑ ↑ ↑";
            }
        }

        private void ColorizeCell(INativeRecord record, DataGridViewCell cell)
        {
            if (record.IsNative)
                cell.Style.BackColor = _grid.DefaultCellStyle.BackColor;
            else
                cell.Style.BackColor = Color.LemonChiffon;
        }
        private void ColorizeCellText(ICheckableRecord record, DataGridViewCell cell)
        {
            if (record.IsChecked)
                cell.Style.ForeColor = _grid.DefaultCellStyle.ForeColor;
            else
                cell.Style.ForeColor = Color.DarkGray;
        }

        private void SaveSelection()
        {
            try
            {
                if (_grid.SelectedCells.Count > 0)
                {
                    var cell = _grid.SelectedCells[0];
                    _selectedColumnIndex = cell.ColumnIndex;
                    _selectedRowIndex = cell.RowIndex;
                }

                _firstRenderedRowIndex = _grid.FirstDisplayedScrollingRowIndex;
                _firstRenderedColumnIndex = _grid.FirstDisplayedScrollingColumnIndex;
            }
            catch { }
        }

        private void RestoreSelection()
        {
            try
            {
                _grid.Rows[_selectedRowIndex].Cells[_selectedColumnIndex].Selected = true;
                _grid.FirstDisplayedScrollingRowIndex = _firstRenderedRowIndex;
                _grid.FirstDisplayedScrollingColumnIndex = _firstRenderedColumnIndex;
            }
            catch { }
        }
    }
}
