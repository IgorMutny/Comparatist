namespace Comparatist
{
    public class LanguagesService
    {
        private readonly DataGridView _grid;
        private readonly IRepository<Language> _repository;

        public LanguagesService(DataGridView grid, IRepository<Language> repository)
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
                HeaderText = "Language",
                DataPropertyName = "Value",
                Width = 200
            };

            _grid.Columns.Add(valueColumn);
            _grid.AutoGenerateColumns = false;
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
            string? input = InputBox.Show("Add language", string.Empty);

            if (!string.IsNullOrWhiteSpace(input))
            {
                _repository.Add(new Language { Value = input });
                Refresh();
            }
        }

        public void Edit()
        {
            if (_grid.SelectedRows.Count == 0)
                return;

            var row = _grid.SelectedRows[0];
            if (row.Tag is not Guid id || !_repository.TryGet(id, out var language))
                return;

            string? input = InputBox.Show("Edit language", $"New name for {language.Value}", language.Value);
            if (string.IsNullOrWhiteSpace(input))
                return;

            language.Value = input;
            Refresh();
        }

        public void Delete()
        {
            if (_grid.SelectedRows.Count == 0)
                return;

            var row = _grid.SelectedRows[0];
            if (row.Tag is not Guid id || !_repository.TryGet(id, out var language))
                return;

            var result = MessageBox.Show(
                $"Delete {language.Value}?",
                "Delete language",
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
