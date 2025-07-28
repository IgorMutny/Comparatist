namespace Comparatist
{
    public class AlphaRootsService
    {
        private IDatabase _db;
        private DataGridView _grid;
        private ContextMenuStrip _gridMenu;
        private ContextMenuStrip _rootRowMenu;
        private ContextMenuStrip _stemRowMenu;

        public AlphaRootsService(
            IDatabase db,
            DataGridView grid,
            ContextMenuStrip gridMenu,
            ContextMenuStrip rootRowMenu,
            ContextMenuStrip stemRowMenu)
        {
            _db = db;
            _grid = grid;
            _gridMenu = gridMenu;
            _rootRowMenu = rootRowMenu;
            _stemRowMenu = stemRowMenu;
            SetupGridView();
        }

        private void SetupGridView()
        {
            _grid.Dock = DockStyle.Fill;
            _grid.AllowUserToAddRows = false;
            _grid.RowHeadersVisible = false;
            _grid.AutoGenerateColumns = false;
            _grid.MultiSelect = false;
            _grid.CellMouseDown += OnCellMouseDown;
            _grid.CellDoubleClick += OnCellDoubleClick;
            _grid.ReadOnly = true;
            _grid.Visible = false;
        }

        private void OnCellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _grid.ClearSelection();

                if (e.RowIndex < 0)
                {
                    _gridMenu.Show(_grid, _grid.PointToClient(Cursor.Position));
                }
                else
                {
                    var row = _grid.Rows[e.RowIndex];
                    row.Selected = true;

                    if (row.Tag is RootRowTag)
                    {
                        _rootRowMenu.Show(_grid, _grid.PointToClient(Cursor.Position));
                    }
                    else if (row.Tag is StemRowTag)
                    {
                        _stemRowMenu.Show(_grid, _grid.PointToClient(Cursor.Position));
                    }
                    else if (row.Tag is EmptyRowTag)
                    {
                        //do nothing
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

            if (row.Tag is not RootRowTag tag)
                return;

            if (tag.Expanded)
                CollapseRow(e.RowIndex);
            else
                ExpandRow(e.RowIndex);

            tag.Expanded = !tag.Expanded;
        }

        public void Refresh()
        {
            RefreshColumns();
            RefreshRows();
        }

        private void RefreshColumns()
        {
            _grid.Columns.Clear();
            var languages = _db.Languages.GetAll();

            var rootColumn = new DataGridViewTextBoxColumn();
            rootColumn.HeaderText = string.Empty;
            rootColumn.Name = "Roots";
            rootColumn.Width = 200;
            _grid.Columns.Add(rootColumn);

            foreach (var language in languages)
            {
                var column = new DataGridViewTextBoxColumn();
                column.HeaderText = language.Value;
                column.Name = language.Id.ToString();
                column.Width = 200;
                _grid.Columns.Add(column);
            }
        }

        private void RefreshRows()
        {
            _grid.Rows.Clear();
            var sortedRoots = _db.Roots.GetAll().OrderBy(r => r.Value).ToList();

            foreach (var root in sortedRoots)
            {
                int rowIndex = _grid.Rows.Add();

                var row = _grid.Rows[rowIndex];
                row.Cells[0].Value = root.Value;

                row.Tag = new RootRowTag { Id = root.Id, Expanded = false };
            }
        }

        private void ExpandRow(int rootRowIndex)
        {
            var rootRow = _grid.Rows[rootRowIndex];

            if (rootRow.Tag is not RootRowTag rootTag)
                return;

            var stems = _db.Stems
                .Filter(s => s.RootIds.Contains(rootTag.Id))
                .OrderBy(s => s.Value)
                .ToList();

            int insertIndex = rootRowIndex + 1;

            if (stems.Count > 0)
            {
                foreach (var stem in stems)
                {
                    int newIndex = insertIndex++;
                    _grid.Rows.Insert(newIndex);

                    var row = _grid.Rows[newIndex];
                    row.Tag = new StemRowTag { Id = stem.Id };

                    row.Cells[0].Value = $"{stem.Value} — {stem.Translation}";
                }
            }
            else
            {
                _grid.Rows.Insert(insertIndex);
                var row = _grid.Rows[insertIndex];
                row.Tag = new EmptyRowTag();
                row.Cells[0].Value = "---no stems---";

            }
        }

        private void CollapseRow(int rootRowIndex)
        {
            var nextIndex = rootRowIndex + 1;

            while (nextIndex < _grid.Rows.Count)
            {
                var row = _grid.Rows[nextIndex];

                if (row.Tag is StemRowTag || row.Tag is EmptyRowTag)
                    _grid.Rows.RemoveAt(nextIndex);
                else
                    break;
            }
        }

        public void AddRoot()
        {
            var form = new RootEditForm("Add Root");

            if (form.ShowDialog() == DialogResult.OK)
            {
                var newRoot = new Root
                {
                    Value = form.ValueText,
                    Translation = form.TranslationText,
                    Comment = form.CommentText,
                    Native = form.NativeValue,
                    Checked = form.CheckedValue
                };
                _db.Roots.Add(newRoot);
                Refresh();
            }
        }

        public void EditRoot()
        {
            if (_grid.SelectedRows.Count == 0)
                return;

            var row = _grid.SelectedRows[0];

            if (row.Tag is not RootRowTag rowTag || !_db.Roots.TryGet(rowTag.Id, out var root))
                return;

            var form = new RootEditForm("Edit Root");
            form.ValueText = root.Value;
            form.TranslationText = root.Translation;
            form.CommentText = root.Comment;
            form.NativeValue = root.Native;
            form.CheckedValue = root.Checked;

            if (form.ShowDialog() == DialogResult.OK)
            {
                root.Value = form.ValueText;
                root.Translation = form.TranslationText;
                root.Comment = form.CommentText;
                root.Native = form.NativeValue;
                root.Checked = form.CheckedValue;
                Refresh();
            }
        }

        public void DeleteRoot()
        {
            if (_grid.SelectedRows.Count == 0)
                return;

            var row = _grid.SelectedRows[0];
            if (row.Tag is not RootRowTag rowTag || !_db.Roots.TryGet(rowTag.Id, out var root))
                return;

            var result = MessageBox.Show(
                $"Delete '{root.Value}'?",
                "Delete root",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _db.Roots.Delete(rowTag.Id);
                Refresh();
            }
        }

        public void AddStem()
        {
            var form = new StemEditForm("Add Stem", _db.Roots.GetAll().ToList(), new List<Guid>());

            if (form.ShowDialog() == DialogResult.OK)
            {
                var newStem = new Stem
                {
                    Value = form.ValueText,
                    Translation = form.TranslationText,
                    Comment = form.CommentText,
                    RootIds = form.SelectedRootIds,
                    Native = form.NativeValue,
                    Checked = form.CheckedValue
                };

                _db.Stems.Add(newStem);
                Refresh();
            }
        }

        public void EditStem()
        {
            if (_grid.SelectedRows.Count == 0)
                return;

            var row = _grid.SelectedRows[0];

            if (row.Tag is not StemRowTag rowTag || !_db.Stems.TryGet(rowTag.Id, out var stem))
                return;

            var form = new StemEditForm("Edit stem", _db.Roots.GetAll().ToList(), stem.RootIds);
            form.ValueText = stem.Value;
            form.TranslationText = stem.Translation;
            form.CommentText = stem.Comment;
            form.NativeValue = stem.Native;
            form.CheckedValue = stem.Checked;

            if (form.ShowDialog() == DialogResult.OK)
            {
                stem.Value = form.ValueText;
                stem.Translation = form.TranslationText;
                stem.Comment = form.CommentText;
                stem.RootIds = form.SelectedRootIds;
                stem.Native = form.NativeValue;
                stem.Checked = form.CheckedValue;
                Refresh();
            }
        }

        public void DeleteStem()
        {
            if (_grid.SelectedRows.Count == 0)
                return;

            var row = _grid.SelectedRows[0];
            if (row.Tag is not StemRowTag rowTag || !_db.Stems.TryGet(rowTag.Id, out var stem))
                return;

            var result = MessageBox.Show(
                $"Delete '{stem.Value}'?",
                "Delete stem",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _db.Stems.Delete(rowTag.Id);
                Refresh();
            }
        }
    }
}
