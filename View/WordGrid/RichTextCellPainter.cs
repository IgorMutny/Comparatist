using System.Text.RegularExpressions;

namespace Comparatist.View.WordGrid.Binders
{
	internal class RichTextCellPainter : IDisposable
	{
		private readonly DataGridView _grid;
		private readonly int _padding = 4;

		public RichTextCellPainter(DataGridView grid)
		{
			_grid = grid;
			_grid.CellPainting += OnCellPainting;
		}

		public void Dispose()
		{
			_grid.CellPainting -= OnCellPainting;
		}

		private void OnCellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
		{
			var grid = sender as DataGridView;
			var value = e.FormattedValue?.ToString();

			if(grid == null
				|| e.CellStyle == null
				|| e.Graphics == null
				|| e.RowIndex < 0
				|| e.ColumnIndex < 0
				|| string.IsNullOrEmpty(value))
			{
				return;
			}

			e.Handled = true;
			e.PaintBackground(e.CellBounds, true);

			var currentX = e.CellBounds.X + 2;
			var currentY = e.CellBounds.Y + _padding;
			var lineHeight = TextRenderer.MeasureText(e.Graphics, "A", e.CellStyle.Font).Height;
			var maxWidth = e.CellBounds.Width - 4;
			var totalSublines = 0;

			var lines = value.Split('\n', StringSplitOptions.RemoveEmptyEntries);

			foreach(var line in lines)
			{
				if(string.IsNullOrEmpty(line))
					continue;

				(var text, var style) = Parse(line);
				using var font = new Font(e.CellStyle.Font, style);
				var sublines = new List<string>();
				var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				var currentSubline = string.Empty;

				foreach(var word in words)
				{
					var testSubline = $"{currentSubline} {word}";
					var size = TextRenderer.MeasureText(e.Graphics, testSubline, font);

					if(size.Width >= maxWidth)
					{
						sublines.Add(currentSubline);
						currentSubline = word;
					}
					else
					{
						currentSubline = testSubline;
					}
				}

				sublines.Add(currentSubline);

				foreach(var subline in sublines)
				{
					TextRenderer.DrawText(
						e.Graphics,
						subline,
						font,
						new Point(currentX, currentY),
						e.CellStyle.ForeColor);

					currentY += lineHeight + _padding;
				}

				totalSublines += sublines.Count;
			}

			if(lineHeight > 0 && e.RowIndex < grid.Rows.Count)
			{
				var neededHeight = totalSublines * (lineHeight + _padding) + _padding;

				if(neededHeight > e.CellBounds.Height)
					grid.Rows[e.RowIndex].Height = neededHeight;
			}
		}

		private (string, FontStyle) Parse(string line)
		{
			var style = ExtractTag(line);
			var text = style == FontStyle.Regular ? line : line[3..];
			return (text, style);
		}

		private FontStyle ExtractTag(string line)
		{
			if(line.Length < 3)
				return FontStyle.Regular;

			var tag = line[0..3];

			return tag switch
			{
				"[b]" => FontStyle.Bold,
				"[i]" => FontStyle.Italic,
				"[m]" => FontStyle.Bold | FontStyle.Italic,
				_ => FontStyle.Regular
			};
		}
	}
}
