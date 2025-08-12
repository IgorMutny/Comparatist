using Comparatist.Application.Cache;
using Comparatist.View.Common;

namespace Comparatist.View.LanguageGrid
{
    internal class LanguageGridRenderer : Renderer<DataGridView>
    {
        private Dictionary<LanguageBinder, DataGridViewRow> _languages = new();

        public LanguageGridRenderer(DataGridView control) : base(control) { }

        public void Add(LanguageBinder binder)
        {
            int rowIndex = Control.Rows.Add();
            var row = Control.Rows[rowIndex];
            _languages.Add(binder, row);

            Update(binder);
        }

        public void Remove(LanguageBinder binder)
        {
            if (!_languages.TryGetValue(binder, out var row))
                return;

            Control.Rows.Remove(row);
            _languages.Remove(binder);
        }

        public void Update(LanguageBinder binder)
        {
            if (!_languages.TryGetValue(binder, out var row))
                return;

            var cell = row.Cells[0];
            cell.Tag = binder.Language;
            cell.Value = binder.Language.Value;
        }

        public void Move(LanguageBinder currentBinder, LanguageBinder? previousBinder)
        {
            if (!_languages.TryGetValue(currentBinder, out var row))
                return;

            Control.Rows.Remove(row);

            int index = previousBinder != null 
                ? _languages[previousBinder].Index + 1
                : 0;

            Control.Rows.Insert(index, row);
        }

        public void Reset()
        {
            Control.Rows.Clear();
            Control.Columns.Clear();
            _languages.Clear();

            Control.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Language",
                Width = 200
            });
        }

    }
}
