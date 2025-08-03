using Comparatist.Core.Records;
using System.Diagnostics.CodeAnalysis;

namespace Comparatist.View.LanguageGrid
{
    internal class LanguageGridDragDropHelper
    {
        private readonly DataGridView _grid;
        private readonly Action<int, int> _reorder;
        private int _draggedRowIndex = -1;

        public LanguageGridDragDropHelper(DataGridView grid, Action<int, int> reorder)
        {
            _grid = grid;
            _reorder = reorder;
        }

        public void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var hit = _grid.HitTest(e.X, e.Y);
            _draggedRowIndex = hit.RowIndex >= 0 ? hit.RowIndex : -1;
        }

        public void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _draggedRowIndex < 0)
                return;

            var row = _grid.Rows[_draggedRowIndex];
            _grid.DoDragDrop(row, DragDropEffects.Move);
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
            if (targetIndex < 0 || targetIndex >= _grid.Rows.Count)
                return;

            _reorder.Invoke(sourceIndex, targetIndex);
        }

        private bool TryGetDraggedRow(DragEventArgs e, [NotNullWhen(true)] out DataGridViewRow? row)
        {
            row = e.Data?.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
            return row != null;
        }

        private int GetTargetRowIndex(DragEventArgs e)
        {
            var pt = _grid.PointToClient(new Point(e.X, e.Y));
            return _grid.HitTest(pt.X, pt.Y).RowIndex;
        }
    }
}
