using Comparatist.Core.Records;
using Comparatist.Services.TableCache;

namespace Comparatist.View.WordGrid
{
    internal class WordGridRenderHelper
    {
        private DataGridView _grid;

        private Dictionary<Guid, CachedBlock> _blocks = new();
        private List<Guid> _expandedRootIds = new();

        public WordGridRenderHelper(DataGridView grid)
        {
            _grid = grid;
        }

        public void Render(IEnumerable<CachedBlock> blocks, IEnumerable<Language> languages)
        {
            _grid.Columns.Clear();
            _grid.Rows.Clear();
            AddColumns(languages);
            AddBlocks(blocks);
        }

        public void HandleDoubleClick(Root root, int rowIndex)
        {
            if (_expandedRootIds.Contains(root.Id))
                Collapse(root, rowIndex);
            else
                Expand(root, rowIndex);
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

        private void AddBlocks(IEnumerable<CachedBlock> blocks)
        {
            _blocks.Clear();

            foreach (var block in blocks)
                AddBlock(block);
        }

        private void AddBlock(CachedBlock block)
        {
            _blocks.Add(block.Root.Id, block);
            var isExpanded = _expandedRootIds.Contains(block.Root.Id);
            var index = _grid.Rows.Add();
            AddBlockHeaderRow(block.Root, index, isExpanded);

            if (isExpanded)
                Expand(block.Root, index);

            AddEmptyRow();
        }

        private void AddBlockHeaderRow(Root root, int rowIndex, bool isExpanded)
        {
            var row = _grid.Rows[rowIndex];
            row.Cells[0].Value = $"[b]{root.Value}[/b] {root.Translation}";
            row.Cells[0].Tag = root;
            FillCellsInRootRow(root, isExpanded, row);
        }

        private void FillRowHeader(DataGridViewRow row, Stem stem)
        {
            var cell = row.Cells[0];
            cell.Value = $"→ [b]{stem.Value}[/b] {stem.Translation}";
            cell.Tag = stem;
        }

        private void FillCell(CachedRow cachedRow, DataGridViewRow row, DataGridViewColumn column)
        {
            if (column.Tag is not Language language)
                return;

            var languageId = language.Id;
            var columnIndex = column.Index;

            if (cachedRow.Cells.TryGetValue(languageId, out var word) && word != null)
                FillWordCell(row, columnIndex, word);
            else
                FillEmptyCell(row, columnIndex, languageId, cachedRow.Stem.Id);
        }

        private void FillWordCell(DataGridViewRow row, int columnIndex, Word word)
        {
            var cell = row.Cells[columnIndex];
            cell.Value = $"[b]{word.Value}[/b] {word.Translation}";
            cell.Style.BackColor = _grid.DefaultCellStyle.BackColor;
            cell.Tag = word;
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

            if (block.Rows.Count == 0)
            {
                AddNoStemRow(block.Root, index);
            }
            else
            {
                var rows = block.Rows.Values.ToList();

                for (int i = rows.Count - 1; i >= 0; i--)
                    AddStemRow(rows[i], index);
            }

            FillCellsInRootRow(root, true, _grid.Rows[rowIndex]);
        }

        private void Collapse(Root root, int rowIndex)
        {

        }

        private void AddStemRow(CachedRow cachedRow, int index)
        {
            _grid.Rows.Insert(index);
            var row = _grid.Rows[index];
            FillRowHeader(row, cachedRow.Stem);

            foreach (DataGridViewColumn column in _grid.Columns)
                FillCell(cachedRow, row, column);
        }

        private void AddNoStemRow(Root root, int index)
        {
            _grid.Rows.Insert(index);
            var row = _grid.Rows[index];
            row.Cells[0].Value = "→ [i]no stems[/i]";
        }

        private void FillCellsInRootRow(Root root, bool isExpanded, DataGridViewRow row)
        {
            if (_grid.Columns.Count <= 1)
                return;

            for (int i = 1; i < _grid.Columns.Count; i++)
            {
                row.Cells[i].Tag = root;

                if (!isExpanded)
                    row.Cells[i].Value = "↓ ↓ ↓";
                else
                    row.Cells[i].Value = "↑ ↑ ↑";
            }
        }
    }
}
