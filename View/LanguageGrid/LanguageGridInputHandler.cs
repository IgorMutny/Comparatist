using Comparatist.Data.Entities;
using Comparatist.Application.Cache;
using Comparatist.View.Common;
using Comparatist.View.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace Comparatist.View.LanguageGrid
{
    internal class LanguageGridInputHandler : InputHandler<DataGridView>
    {
        private DisposableMenu _gridMenu;
        private DisposableMenu _languageMenu;
        private int _draggedRowIndex = -1;

        public event Action<Language>? AddRequest;
        public event Action<Language>? UpdateRequest;
        public event Action<IEnumerable<Language>>? UpdateManyRequest;
        public event Action<Language>? DeleteRequest;

        public LanguageGridInputHandler(DataGridView control) : base(control)
        {
            _gridMenu = new DisposableMenu(
                ("Add language", RequestAddLanguage));

            _languageMenu = new DisposableMenu(
                ("Add language", RequestAddLanguage),
                ("Edit language", RequestEditLanguage),
                ("Delete language", RequestDeleteLanguage));

            Control.AllowUserToAddRows = false;
            Control.RowHeadersVisible = false;
            Control.AutoGenerateColumns = false;
            Control.MultiSelect = false;
            Control.AllowDrop = true;
            Control.ReadOnly = true;
            Control.Visible = false;

            Control.MouseUp += OnMouseUp;
            Control.MouseMove += OnMouseMove;
            Control.MouseDown += OnMouseDown;
            Control.DragOver += OnDragOver;
            Control.DragDrop += OnDragDrop;
        }

        public override void Dispose()
        {
            _gridMenu.Dispose();
            _languageMenu.Dispose();

            Control.MouseUp -= OnMouseUp;
            Control.MouseDown -= OnMouseDown;
            Control.MouseMove -= OnMouseMove;
            Control.DragOver -= OnDragOver;
            Control.DragDrop -= OnDragDrop;
        }

        private void RequestAddLanguage()
        {
            string? input = InputBox.Show("Add language", string.Empty);

            if (!string.IsNullOrWhiteSpace(input))
            {
                var language = new Language
                {
                    Id = Guid.Empty,
                    Value = input,
                    Order = Control.Rows.Count
                };

                AddRequest?.Invoke(language);
            }
        }

        private void RequestEditLanguage()
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

            language.Value = input;
            UpdateRequest?.Invoke(language);
        }

        private void RequestDeleteLanguage()
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
                DeleteRequest?.Invoke(language);
        }

        private void ReorderRows(int sourceIndex, int targetIndex)
        {
            var row = Control.Rows[sourceIndex];
            Control.Rows.Remove(row);
            Control.Rows.Insert(targetIndex, row);

            var movedLanguages = new List<Language>();

            foreach (DataGridViewRow movedRow in Control.Rows)
            {
                var language = movedRow.Cells[0].Tag as Language;

                if (language == null)
                    continue;

                language.Order = movedRow.Index;
                movedLanguages.Add(language);
            }

            UpdateManyRequest?.Invoke(movedLanguages);
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

        public void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var hit = Control.HitTest(e.X, e.Y);
            _draggedRowIndex = hit.RowIndex >= 0 ? hit.RowIndex : -1;
        }

        public void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _draggedRowIndex < 0)
                return;

            var row = Control.Rows[_draggedRowIndex];
            Control.DoDragDrop(row, DragDropEffects.Move);
            _draggedRowIndex = -1;
        }

        public void OnDragOver(object? sender, DragEventArgs e)
        {
            if (!TryGetDraggedRow(e, out var sourceRow))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var targetIndex = GetTargetRowIndex(e);
            e.Effect = (targetIndex != sourceRow.Index && targetIndex != -1)
                ? DragDropEffects.Move
                : DragDropEffects.None;
        }

        public void OnDragDrop(object? sender, DragEventArgs e)
        {
            if (!TryGetDraggedRow(e, out var sourceRow))
                return;

            var sourceIndex = sourceRow.Index;

            var targetIndex = GetTargetRowIndex(e);
            if (targetIndex < 0 || targetIndex >= Control.Rows.Count)
                return;

            ReorderRows(sourceIndex, targetIndex);
        }

        private bool TryGetDraggedRow(DragEventArgs e, [NotNullWhen(true)] out DataGridViewRow? row)
        {
            row = e.Data?.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
            return row != null;
        }

        private int GetTargetRowIndex(DragEventArgs e)
        {
            var pt = Control.PointToClient(new Point(e.X, e.Y));
            return Control.HitTest(pt.X, pt.Y).RowIndex;
        }
    }
}
