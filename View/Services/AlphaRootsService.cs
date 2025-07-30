using Comparatist.Core.Persistence;
using Comparatist.Services.Cache;
using Comparatist.View.Tags;

namespace Comparatist
{
    public class AlphaRootsService
    {
        private DataGridView _grid;
        private ContextMenuStrip _gridMenu;
        private ContextMenuStrip _rootRowMenu;
        private ContextMenuStrip _stemRowMenu;
        private ContextMenuStrip _wordMenu;
        private DataCacheService _dataCacheService;
        private HashSet<Guid> _expandedRootIds = new();

        public AlphaRootsService(
            DataGridView grid,
            ContextMenuStrip gridMenu,
            ContextMenuStrip rootRowMenu,
            ContextMenuStrip stemRowMenu,
            ContextMenuStrip wordMenu,
            DataCacheService dataCacheService)
        {
            _grid = grid;
            _gridMenu = gridMenu;
            _rootRowMenu = rootRowMenu;
            _stemRowMenu = stemRowMenu;
            _wordMenu = wordMenu;
            _dataCacheService = dataCacheService;
            SetupGridView();
        }

        private List<CategoryTag> AllCategories => _dataCacheService.AllCategories.ToList();
        private List<RootTag> AllRoots => _dataCacheService.AllRoots.ToList();

        private void SetupGridView()
        {
            _grid.Dock = DockStyle.Fill;
            _grid.AllowUserToAddRows = false;
            _grid.RowHeadersVisible = false;
            _grid.AutoGenerateColumns = false;
            _grid.MultiSelect = false;
            _grid.CellMouseDown += OnCellMouseDown;
            _grid.CellDoubleClick += OnCellDoubleClick;
            _grid.CellPainting += OnCellPainting;
            _grid.ReadOnly = true;
            _grid.Visible = false;
        }

        private void OnCellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewCell? cell = null;

            if (_grid.SelectedCells.Count > 0)
                cell = _grid.SelectedCells[0];

            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex < 0)
                {
                    _gridMenu.Show(_grid, _grid.PointToClient(Cursor.Position));
                }
                else
                {
                    var row = _grid.Rows[e.RowIndex];

                    if (cell == null || cell.ColumnIndex == 0)
                    {
                        _grid.ClearSelection();
                        row.Selected = true;
                    }

                    if (row.Tag is RootTag)
                    {
                        _rootRowMenu.Show(_grid, _grid.PointToClient(Cursor.Position));
                    }
                    else if (row.Tag is StemTag)
                    {
                        if (e.ColumnIndex == 0)
                        {
                            _stemRowMenu.Show(_grid, _grid.PointToClient(Cursor.Position));
                        }
                        else
                        {
                            _wordMenu.Show(_grid, _grid.PointToClient(Cursor.Position));
                        }
                    }
                    else if (row.Tag is EmptyTag)
                    {
                        // do nothing
                    }
                    else
                    {
                        _gridMenu.Show(_grid, _grid.PointToClient(Cursor.Position));
                    }
                }
            }
        }

        private void OnCellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var row = _grid.Rows[e.RowIndex];

            if (row.Tag is not RootTag tag)
                return;

            if (_expandedRootIds.Contains(tag.Id))
            {
                CollapseRow(e.RowIndex);
                _expandedRootIds.Remove(tag.Id);
            }
            else
            {
                ExpandRow(e.RowIndex, tag.Stems);
                _expandedRootIds.Add(tag.Id);
            }
        }

        private void OnCellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var value = e.FormattedValue?.ToString();
            if (string.IsNullOrEmpty(value)) return;

            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);

            var formatted = RichTextFormatter.Parse(value);
            var x = e.CellBounds.X + 2;
            var y = e.CellBounds.Y + 2;

            if (e.CellStyle != null && e.Graphics != null)
            {
                foreach (var (text, style) in formatted)
                {
                    using var font = new Font(e.CellStyle.Font, style);
                    var size = TextRenderer.MeasureText(text, font);
                    TextRenderer.DrawText(e.Graphics, text, font, new Point(x, y), e.CellStyle.ForeColor);
                    x += size.Width;
                }
            }
        }

        public void Refresh()
        {
            RefreshColumns();
            RefreshRows();
            RefreshCells();
            _grid.ClearSelection();
        }

        private void RefreshColumns()
        {
            _grid.Columns.Clear();
            var languages = _dataCacheService.AllLanguages.ToList();

            var rootColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = string.Empty,
                Name = "Roots_Stems",
                Width = 200
            };

            _grid.Columns.Add(rootColumn);

            foreach (var language in languages)
            {
                var column = new DataGridViewTextBoxColumn
                {
                    HeaderText = language.Value,
                    Name = language.Id.ToString(),
                    Width = 200,
                    Tag = language
                };

                _grid.Columns.Add(column);
            }
        }

        private void RefreshRows()
        {
            _grid.Rows.Clear();

            var allTags = _dataCacheService.GetAlphabeticalTableData();

            foreach (var tag in allTags)
            {
                int rootIndex = _grid.Rows.Add();
                var rootRow = _grid.Rows[rootIndex];

                rootRow.Cells[0].Value = $"[b]{tag.Value}[/b] {tag.Translation}";
                rootRow.Tag = tag;

                var expanded = _expandedRootIds.Contains(tag.Id);
                if (expanded)
                    ExpandRow(rootIndex, tag.Stems);
            }
        }

        private void ExpandRow(int rootRowIndex, IReadOnlyList<StemTag> tags)
        {
            var rootRow = _grid.Rows[rootRowIndex];
            int insertIndex = rootRowIndex + 1;

            if (tags.Count > 0)
            {
                foreach (var stemTag in tags)
                {
                    _grid.Rows.Insert(insertIndex);
                    var row = _grid.Rows[insertIndex++];

                    row.Tag = stemTag;
                    row.Cells[0].Value = $"→ [b]{stemTag.Value}[/b] {stemTag.Translation}";
                }
            }
            else
            {
                _grid.Rows.Insert(insertIndex);
                var row = _grid.Rows[insertIndex];
                row.Tag = new EmptyTag();
                row.Cells[0].Value = "→ [i]no stems[/i]";
            }

            if (rootRow.Tag is RootTag rootTag)
                _expandedRootIds.Add(rootTag.Id);

            RefreshCells();
        }

        private void CollapseRow(int rootRowIndex)
        {
            var rootRow = _grid.Rows[rootRowIndex];
            if (rootRow.Tag is RootTag root)
                _expandedRootIds.Remove(root.Id);

            int nextIndex = rootRowIndex + 1;
            while (nextIndex < _grid.Rows.Count)
            {
                var row = _grid.Rows[nextIndex];
                if (row.Tag is StemTag || row.Tag is EmptyTag)
                    _grid.Rows.RemoveAt(nextIndex);
                else
                    break;
            }
        }

        private void RefreshCells()
        {
            var languages = _dataCacheService.AllLanguages;
            var languageIndex = languages
                .Select((lang, index) => new { lang.Id, Index = index + 1 })
                .ToDictionary(x => x.Id, x => x.Index);

            foreach (DataGridViewRow row in _grid.Rows)
            {
                if (row.Tag is not StemTag stem)
                    continue;

                foreach (var (langId, colIndex) in languageIndex)
                {
                    var cell = row.Cells[colIndex];
                    var word = stem.WordsByLanguage.FirstOrDefault(w => w.Key == langId).Value;

                    if (word is not null)
                    {
                        cell.Value = $"[b]{word.Value}[/b] {word.Translation}";
                        cell.Style.BackColor = _grid.DefaultCellStyle.BackColor;
                        cell.Tag = word;
                    }
                    else
                    {
                        cell.Value = null;
                        cell.Style.BackColor = Color.LightGray;
                        cell.Tag = new EmptyTag();
                    }
                }
            }
        }

        public void AddRoot()
        {
            var form = new RootEditForm("Add Root", new RootTag(), AllCategories);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var newRoot = form.GetResult();
                _dataCacheService.AddRoot(newRoot);
                Refresh();
            }
        }

        public void EditRoot()
        {
            if (_grid.SelectedRows.Count == 0)
                return;

            var row = _grid.SelectedRows[0];

            if (row.Tag is not RootTag tag)
                return;

            var form = new RootEditForm("Edit Root", tag, AllCategories);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var root = form.GetResult();
                _dataCacheService.UpdateRoot(root);
                Refresh();
            }
        }

        public void DeleteRoot()
        {
            if (_grid.SelectedRows.Count == 0)
                return;

            var row = _grid.SelectedRows[0];
            if (row.Tag is not RootTag tag)
                return;

            var result = MessageBox.Show(
                $"Delete '{tag.Value}'?",
                "Delete root",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _expandedRootIds.Remove(tag.Id);
                _dataCacheService.DeleteRoot(tag.Id);
                Refresh();
            }
        }

        public void AddStem()
        {
            var selectedRootIds = new List<Guid>();

            if (_grid.SelectedRows[0].Tag is RootTag rootTag)
            {
                selectedRootIds.Add(rootTag.Id);
            }
            else if (_grid.SelectedRows[0].Tag is StemTag stemTag)
            {
                foreach (var rootId in stemTag.RootIds)
                    selectedRootIds.Add(rootId);
            }

            var form = new StemEditForm("Add Stem", new StemTag(), AllRoots, selectedRootIds);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var stem = form.GetResult();
                _dataCacheService.AddStem(stem);
                Refresh();
            }
        }

        public void EditStem()
        {
            if (_grid.SelectedRows.Count == 0)
                return;

            var row = _grid.SelectedRows[0];

            if (row.Tag is not StemTag stemTag)
                return;

            var form = new StemEditForm("Add Stem", stemTag, AllRoots, stemTag.RootIds);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var stem = form.GetResult();
                _dataCacheService.UpdateStem(stem);
                Refresh();
            }
        }

        public void DeleteStem()
        {
            if (_grid.SelectedRows.Count == 0)
                return;

            var row = _grid.SelectedRows[0];
            if (row.Tag is not StemTag tag)
                return;

            var result = MessageBox.Show(
                $"Delete '{tag.Value}'?",
                "Delete stem",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _dataCacheService.DeleteStem(tag.Id);
                Refresh();
            }
        }

        public void AddOrEditWord()
        {
            if (_grid.SelectedCells.Count == 0)
                return;

            var cell = _grid.SelectedCells[0];
            var row = _grid.Rows[cell.RowIndex];
            var column = _grid.Columns[cell.ColumnIndex];

            if (row.Tag is not StemTag stemTag)
                return;

            if (column.Tag is not LanguageTag languageTag)
                return;

            if (_grid.SelectedCells[0].Tag is WordTag wordTag)
            {
                var form = new WordEditForm(wordTag, stemTag, languageTag);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    var word = form.GetResult();
                    _dataCacheService.UpdateWord(word);
                    Refresh();
                }
            }
            else
            {
                var form = new WordEditForm(new WordTag(), stemTag, languageTag);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    var word = form.GetResult();
                    _dataCacheService.AddWord(word);
                    Refresh();
                 }
            }
        }

        public void DeleteWord()
        {
            if (_grid.SelectedCells.Count == 0)
                return;

            if (_grid.SelectedCells[0].Tag is not WordTag tag)
                return;

            var result = MessageBox.Show(
                $"Delete '{tag.Value}'?",
                "Delete word",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _dataCacheService.DeleteWord(tag.Id);
                Refresh();
            }
        }
    }
}
