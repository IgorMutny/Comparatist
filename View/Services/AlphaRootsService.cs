namespace Comparatist.View.Services
{
    public class AlphaRootsService
    {
        private IDatabase _db;
        private DataGridView _grid;
        private readonly ContextMenuStrip _gridMenu;
        private readonly ContextMenuStrip _rootRowMenu;

        public AlphaRootsService(
            IDatabase db,
            DataGridView grid,
            ContextMenuStrip gridMenu,
            ContextMenuStrip rootRowMenu)
        {
            _db = db;
            _grid = grid;
            _gridMenu = gridMenu;
            _rootRowMenu = rootRowMenu;
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

            if (rootRow.Tag is not RootRowTag)
                return;

            var expandedRowIndex = rootRowIndex + 1;
            _grid.Rows.Insert(expandedRowIndex);

            var expandedRow = _grid.Rows[expandedRowIndex];

            // fake
            expandedRow.Cells[0].Value = "[Expanded content]";

            for (int i = 1; i < _grid.Columns.Count; i++)
            {
                expandedRow.Cells[i].Value = "Expanded content";
            }

            expandedRow.Tag = null;
        }

        private void CollapseRow(int rootRowIndex)
        {
            var nextIndex = rootRowIndex + 1;
            if (nextIndex < _grid.Rows.Count)
            {
                var nextRow = _grid.Rows[nextIndex];

                if (nextRow.Tag == null) // fake
                {
                    _grid.Rows.RemoveAt(nextIndex);
                }
            }
        }

        public void AddRoot()
        {
            var form = new RootEditForm().WithHeader("Add Root");

            if (form.ShowDialog() == DialogResult.OK)
            {
                var newRoot = new Root
                {
                    Value = form.ValueText,
                    Translation = form.TranslationText,
                    Comment = form.CommentText,
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

            var form = new RootEditForm().WithHeader("Edit Root");
            form.ValueText = root.Value;
            form.TranslationText = root.Translation;
            form.CommentText = root.Comment;
            form.CheckedValue = root.Checked;

            if (form.ShowDialog() == DialogResult.OK)
            {
                root.Value = form.ValueText;
                root.Translation = form.TranslationText;
                root.Comment = form.CommentText;
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
    }
}
