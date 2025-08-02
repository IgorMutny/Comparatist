using Comparatist.Core.Records;
using Comparatist.View.Infrastructure;
using Comparatist.View.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace Comparatist.View.LanguageGrid
{
    internal class LanguageGridViewAdapter : ViewAdapter<DataGridView>
    {
        private DisposableMenu _gridMenu;
        private DisposableMenu _languageMenu;
        private LanguageGridDragDropHelper _dragDropHelper;

        public event Action<Language>? AddRequest;
        public event Action<Language>? UpdateRequest;
        public event Action<Language>? DeleteRequest;
        public event Action<IEnumerable<Language>>? ReorderRequest;

        public LanguageGridViewAdapter(DataGridView grid) : base(grid)
        {
            _dragDropHelper = new(Control, RequestReorder);

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
            Control.Dock = DockStyle.Fill;
            Control.AllowUserToAddRows = false;
            Control.RowHeadersVisible = false;
            Control.AutoGenerateColumns = false;
            Control.MultiSelect = false;
            Control.AllowDrop = true;
            Control.ReadOnly = true;
            Control.Visible = false;
            Control.MouseUp += OnMouseUp;
            Control.MouseMove += _dragDropHelper.OnMouseMove;
            Control.MouseDown += _dragDropHelper.OnMouseDown;
            Control.DragOver += _dragDropHelper.OnDragOver;
            Control.DragDrop += _dragDropHelper.OnDragDrop;
        }

        protected override void Unsubscribe()
        {
            Control.MouseUp -= OnMouseUp;
            Control.MouseDown -= _dragDropHelper.OnMouseDown;
            Control.MouseMove -= _dragDropHelper.OnMouseMove;
            Control.DragOver -= _dragDropHelper.OnDragOver;
            Control.DragDrop -= _dragDropHelper.OnDragDrop;
            _gridMenu.Dispose();
            _languageMenu.Dispose();
        }

        public void Render(IEnumerable<Language> languages)
        {
            Control.Rows.Clear();
            Control.Columns.Clear();

            Control.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Language",
                Width = 200
            });

            foreach (var language in languages)
                AddRow(language);
        }

        private void AddRow(Language language)
        {
            int rowIndex = Control.Rows.Add();
            var cell = Control.Rows[rowIndex].Cells[0];
            cell.Value = language.Value;
            cell.Tag = language;
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
            if (Control.SelectedCells.Count == 0)
                return;

            var cell = Control.SelectedCells[0];

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
            if (Control.SelectedCells.Count == 0)
                return;

            var cell = Control.SelectedCells[0];

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

            Control.ClearSelection();
            var point = new Point(e.X, e.Y);
            var hit = Control.HitTest(e.X, e.Y);

            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                var selectedCell = Control.Rows[hit.RowIndex].Cells[hit.ColumnIndex];
                selectedCell.Selected = true;
                _languageMenu.Show(Control, point);
            }
            else
            {
                _gridMenu.Show(Control, point);
            }
        }

        private void RequestReorder(IEnumerable<Language> languages)
        {
            ReorderRequest?.Invoke(languages);
        }
    }
}
