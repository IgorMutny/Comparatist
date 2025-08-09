namespace Comparatist.View.WordGrid
{
    internal class WordBinderRenderer
    {
        private DataGridView _grid;
        private Dictionary<Guid, DataGridViewColumn> _columns;
        private Dictionary<StemBinder, DataGridViewRow> _stems;
        private Dictionary<WordBinder, DataGridViewCell> _words;

        public WordBinderRenderer(
            DataGridView grid,
            Dictionary<Guid, DataGridViewColumn> columns,
            Dictionary<StemBinder, DataGridViewRow> stems,
            Dictionary<WordBinder, DataGridViewCell> words)
        {
            _grid = grid;
            _columns = columns;
            _stems = stems;
            _words = words;
        }

        public void Add(WordBinder binder, StemBinder parent)
        {
            var row = _stems[parent];
            var column = _columns[binder.Word.LanguageId];
            var cell = _grid.Rows[row.Index].Cells[column.Index];
            _words.Add(binder, cell);
            Update(binder);
        }

        public void Remove(WordBinder binder)
        {
            var cell = _words[binder];
            cell.Value = null;
            cell.Tag = null;
            CellFormatter.ColorizeWordCell(cell);
            _words.Remove(binder);
        }

        public void Update(WordBinder binder)
        {
            if (!_words.TryGetValue(binder, out var cell))
                return;

            cell.Tag = binder.Word;
            CellFormatter.ColorizeWordCell(cell);
            CellFormatter.FormatCell(cell, binder.CurrentState);
        }
    }
}
