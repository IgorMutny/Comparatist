using Comparatist.Data.Entities;
using Comparatist.Application.Cache;

namespace Comparatist.View.WordGrid
{
    internal static class CellFormatter
    {
        private static readonly Color CheckedFrontColor = Color.Black;
        private static readonly Color UncheckedFrontColor = Color.DarkGray;
        public static readonly Color EmptyWordCellColor = Color.LightGray;
        public static readonly Color FilledWordCellColor = Color.White;

        public static void ColorizeWordCell(DataGridViewCell cell)
        {
            if (cell.ColumnIndex <= 0 || cell.OwningRow?.Cells[0].Tag is not Stem)
                throw new ArgumentException("Not a word cell");

            cell.Style.BackColor = cell.Tag != null ? FilledWordCellColor : EmptyWordCellColor;
        }

        public static void FormatCell(
            DataGridViewCell cell,
            IDisplayableCachedRecord data,
            string prefix = "")
        {
            var open = data.IsNative ? string.Empty : "《";
            var close = data.IsNative ? string.Empty : "》";
            cell.Style.ForeColor = data.IsChecked ? CheckedFrontColor : UncheckedFrontColor;
            cell.Value = $"{prefix}{open}[b]{data.Value}[/b] {data.Translation} {close}";
        }
    }
}
