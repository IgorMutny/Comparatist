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
            var row = _languages[binder];
            Control.Rows.Remove(row);
            _languages.Remove(binder);
        }

        public void Update(LanguageBinder binder)
        {
            var row = _languages[binder];
            var cell = row.Cells[0];
            cell.Tag = binder.Language;
            cell.Value = binder.Language.Value;
        }

        public void Move(LanguageBinder currentBinder, LanguageBinder? previousBinder)
        {
            var row = _languages[currentBinder];
            Control.Rows.Remove(row);

            int position = previousBinder != null 
                ? _languages[previousBinder].Index
                : 0;

            Control.Rows.Insert(position, row);
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
