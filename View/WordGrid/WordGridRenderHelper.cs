using Comparatist.Core.Records;
using Comparatist.Services.TableCache;

namespace Comparatist.View.WordGrid
{
    internal class WordGridRenderHelper
    {
        private DataGridView _grid;

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
            foreach (var block in blocks)
                AddBlock(block);
        }

        private void AddBlock(CachedBlock block)
        {
            AddBlockHeaderRow(block.Root);

            if (block.Rows.Count == 0)
                AddNoStemRow(block.Root);
            else
                foreach (var row in block.Rows.Values)
                    AddStemRow(row);

            AddEmptyRow(block.Root);
        }

        private void AddBlockHeaderRow(Root root)
        {
            int index = _grid.Rows.Add();
            var row = _grid.Rows[index];
            row.Cells[0].Value = $"[b]{root.Value}[/b] {root.Translation}";

            for (int i = 0; i < _grid.Columns.Count; i++)
                row.Cells[i].Tag = root;
        }

        private void AddStemRow(CachedRow cachedRow)
        {
            int index = _grid.Rows.Add();
            var row = _grid.Rows[index];
            FillRowHeader(row, cachedRow.Stem);

            foreach (DataGridViewColumn column in _grid.Columns)
                FillCell(cachedRow, row, column);
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

        private void AddNoStemRow(Root root)
        {
            var index = _grid.Rows.Add();
            var row = _grid.Rows[index];
            row.Cells[0].Value = "→ [i]no stems[/i]";

            foreach (DataGridViewCell cell in _grid.Rows[index].Cells)
                cell.Tag = root;
        }

        private void AddEmptyRow(Root root)
        {
            var index = _grid.Rows.Add();

            foreach (DataGridViewCell cell in _grid.Rows[index].Cells)
                cell.Tag = root;
        }
    }
}
