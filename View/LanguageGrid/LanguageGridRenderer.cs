using Comparatist.Application.Cache;
using Comparatist.View.Common;

namespace Comparatist.View.LanguageGrid
{
    internal class LanguageGridRenderer : Renderer<DataGridView>
    {
        private Dictionary<Guid, DataGridViewRow> _cachedRows = new();

        public LanguageGridRenderer(DataGridView control) : base(control) { }

        public void Add(CachedLanguage language)
        {
            int rowIndex = Control.Rows.Add();
            var row = Control.Rows[rowIndex];
            var cell = row.Cells[0];
            cell.Value = language.Record.Value;
            cell.Tag = (CachedLanguage)language.Clone();
            _cachedRows.Add(language.Record.Id, row);
        }

        public void Remove(Guid id)
        {
            var row = _cachedRows[id];
            Control.Rows.Remove(row);
            _cachedRows.Remove(id);
        }

        public void Update(Guid id, CachedLanguage language)
        {
            var row = _cachedRows[id];
            var cell = row.Cells[0];
            cell.Value = language.Record.Value;
        }

        public void Move(Guid id, Guid before)
        {
            var row = _cachedRows[id];
            Control.Rows.Remove(row);
            int position = Control.Rows.Count;

            if (before != Guid.Empty)
            {
                var nextRow = _cachedRows[before];
                position = Control.Rows.IndexOf(nextRow);
            }

            Control.Rows.Insert(position, row);
        }

        public void Reset()
        {
            Control.Rows.Clear();
            Control.Columns.Clear();
            _cachedRows.Clear();

            Control.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Language",
                Width = 200
            });
        }

    }
}
