using Comparatist.Services.Cache;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.WordGrid
{
    internal class WordGridRenderer : Renderer<DataGridView>
    {
        private Dictionary<Guid, DataGridViewRow> _sections = new();

        public WordGridRenderer(DataGridView control) : base(control)
        {
            Control.Dock = DockStyle.Fill;
            Control.AllowUserToAddRows = false;
            Control.RowHeadersVisible = false;
            Control.AutoGenerateColumns = false;
            Control.MultiSelect = false;
            Control.ReadOnly = true;
            Control.Visible = false;
        }

        public void Clear()
        {
            Control.Columns.Clear();
            Control.Rows.Clear();
            _sections.Clear();
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

        public void AddSection(CachedCategory category)
        {
            var index = Control.Rows.Add();
            var row = Control.Rows[index];

            row.DefaultCellStyle.BackColor = Color.LightGreen;

            var cell = row.Cells[0];
            cell.Tag = category;
            cell.Value = $"{category.Record.Value.ToUpper()} {category.Record.Order}";

            _sections.Add(category.Record.Id, row);
        }
    }
}

