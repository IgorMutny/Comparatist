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
            _grid.AutoGenerateColumns = false;
            _grid.MultiSelect = false;
            _grid.CellMouseDown += OnCellMouseDown;
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

                // Скрываем столбцы языков для root строки
                //for (int i = 1; i < _rootsGridView.Columns.Count; i++)
                {
                    //row.Cells[i].Value = null;
                }

                row.Tag = new RootRowTag { Id = root.Id, Expanded = false };
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
    }
}
