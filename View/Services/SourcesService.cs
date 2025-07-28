namespace Comparatist
{
    public class SourcesService
    {
        private readonly DataGridView _grid;
        private readonly IRepository<Source> _repository;

        public SourcesService(DataGridView grid, IRepository<Source> repository)
        {
            _grid = grid;
            _repository = repository;

            SetupGridView();
        }

        private void SetupGridView()
        {
            _grid.Columns.Clear();

            var valueColumn = new DataGridViewTextBoxColumn
            {
                Name = "Value",
                HeaderText = "Source",
                DataPropertyName = "Value",
                Width = 200
            };

            _grid.Columns.Add(valueColumn);

            _grid.Dock = DockStyle.Fill;
            _grid.AllowUserToAddRows = false;
            _grid.RowHeadersVisible = false;
            _grid.AutoGenerateColumns = false;
            _grid.MultiSelect = false;
            _grid.CellMouseDown += OnCellMouseDown;
            _grid.ReadOnly = true;
            _grid.Visible = false;
        }

        private void OnCellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                _grid.Rows[e.RowIndex].Selected = true;
        }

        public void Refresh()
        {
            _grid.Rows.Clear();

            foreach (var entry in _repository.GetAll())
            {
                int rowIndex = _grid.Rows.Add(entry.Value);
                _grid.Rows[rowIndex].Tag = entry.Id;
            }
        }

        public void Add()
        {
            string? input = InputBox.Show("Add source", string.Empty);

            if (!string.IsNullOrWhiteSpace(input))
            {
                _repository.Add(new Source { Value = input });
                Refresh();
            }
        }

        public void Edit()
        {
            if (_grid.SelectedRows.Count == 0)
                return;

            var row = _grid.SelectedRows[0];
            if (row.Tag is not Guid id || !_repository.TryGet(id, out var source))
                return;

            string? input = InputBox.Show("Edit source", $"New name for {source.Value}", source.Value);
            if (string.IsNullOrWhiteSpace(input))
                return;

            source.Value = input;
            Refresh();
        }

        public void Delete()
        {
            if (_grid.SelectedRows.Count == 0)
                return;

            var row = _grid.SelectedRows[0];
            if (row.Tag is not Guid id || !_repository.TryGet(id, out var source))
                return;

            var result = MessageBox.Show(
                $"Delete {source.Value}?",
                "Delete source",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _repository.Delete(id);
                Refresh();
            }
        }
    }
}
