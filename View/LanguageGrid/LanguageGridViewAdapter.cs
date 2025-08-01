using Comparatist.Core.Records;
using Comparatist.View.Infrastructure;
using Comparatist.View.Utities;

namespace Comparatist.View.LanguageGrid
{
    internal class LanguageGridViewAdapter : ViewAdapter
    {
        private DataGridView _grid;
        private DisposableMenu _gridMenu;
        private DisposableMenu _languageMenu;

        public event Action<Language>? AddRequest;
        public event Action<Language>? UpdateRequest;
        public event Action<Language>? DeleteRequest;

        public LanguageGridViewAdapter(DataGridView grid)
        {
            _grid = grid;

            _gridMenu = new DisposableMenu(
                ("Add language", AddLanguage));

            _languageMenu = new DisposableMenu(
                ("Add language", AddLanguage),
                ("Edit language", EditLanguage),
                ("Delete language", DeleteLanguage));

            SetupGrid();
        }

        private void SetupGrid()
        {
            _grid.Dock = DockStyle.Fill;
            _grid.AllowUserToAddRows = false;
            _grid.RowHeadersVisible = false;
            _grid.AutoGenerateColumns = false;
            _grid.MultiSelect = false;
            _grid.ReadOnly = true;
            _grid.Visible = false;
            _grid.MouseUp += OnMouseUp;
        }

        protected override void Unsubscribe()
        {
            _grid.MouseUp -= OnMouseUp;
            _gridMenu.Dispose();
            _languageMenu.Dispose();
        }

        public void Render(IEnumerable<Language> languages)
        {
            _grid.Rows.Clear();
            _grid.Columns.Clear();

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Language",
                Width = 200
            });

            foreach (var language in languages)
            {
                int rowIndex = _grid.Rows.Add();
                var cell = _grid.Rows[rowIndex].Cells[0];
                cell.Value = language.Value;
                cell.Tag = language;
            }
        }

        private void AddLanguage()
        {
            string? input = InputBox.Show("Add language", string.Empty);

            if (!string.IsNullOrWhiteSpace(input))
            {
                var language = new Language { Id = Guid.Empty, Value = input };
                AddRequest?.Invoke(language);
            }
        }

        private void EditLanguage()
        {
            if (_grid.SelectedCells.Count == 0)
                return;

            var cell = _grid.SelectedCells[0];

            if (cell.Tag is not Language language)
                return;

            string? input = InputBox.Show(
                $"New name for {language.Value}",
                "Edit language",
                language.Value);

            if (string.IsNullOrWhiteSpace(input))
                return;

            language = (Language)language.Clone();
            language.Value = input;
            UpdateRequest?.Invoke(language);
        }

        private void DeleteLanguage()
        {
            if (_grid.SelectedCells.Count == 0)
                return;

            var cell = _grid.SelectedCells[0];

            if (cell.Tag is not Language language)
                return;

            var result = MessageBox.Show(
                $"Delete {language.Value}? All words of this language will also be deleted!",
                "Delete language",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                language = (Language)language.Clone();
                DeleteRequest?.Invoke(language);
            }
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            _grid.ClearSelection();
            var point = new Point(e.X, e.Y);
            var hit = _grid.HitTest(e.X, e.Y);

            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                var selectedCell = _grid.Rows[hit.RowIndex].Cells[hit.ColumnIndex];
                selectedCell.Selected = true;
                _languageMenu.Show(_grid, point);
            }
            else
            {
                _gridMenu.Show(_grid, point);
            }
        }
    }
}
