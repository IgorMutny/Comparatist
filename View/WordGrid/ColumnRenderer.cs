using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal class ColumnRenderer
    {
        private DataGridView _grid;
        private Dictionary<Guid, DataGridViewColumn> _columns;

        public ColumnRenderer(
            DataGridView grid,
            Dictionary<Guid, DataGridViewColumn> columns)
        {
            _grid = grid;
            _columns = columns;
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

            _grid.Columns.Add(rowHeaderColumn);
        }

        private void AddColumn(CachedLanguage language)
        {
            var column = new DataGridViewTextBoxColumn
            {
                HeaderText = language.Record.Value,
                Width = 200,
                Tag = language.Record,
            };

            _grid.Columns.Add(column);
            _columns.Add(language.Record.Id, column);
        }
    }
}
