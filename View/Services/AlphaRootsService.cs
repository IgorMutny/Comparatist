using Comparatist.Core.Infrastructure;
using Comparatist.Services.TableCache;
using Comparatist.View.Tags;
using Comparatist.View.Utities;

namespace Comparatist
{
    public class AlphaRootsService
    {
        private DataGridView _grid;
        private ContextMenuStrip _gridMenu;
        private ContextMenuStrip _rootRowMenu;
        private ContextMenuStrip _stemRowMenu;
        private ContextMenuStrip _wordMenu;
        private TableCacheService _dataCacheService;
        private HashSet<Guid> _expandedRootIds = new();

        internal AlphaRootsService(
            DataGridView grid,
            ContextMenuStrip gridMenu,
            ContextMenuStrip rootRowMenu,
            ContextMenuStrip stemRowMenu,
            ContextMenuStrip wordMenu)
        {
            _grid = grid;
            _gridMenu = gridMenu;
            _rootRowMenu = rootRowMenu;
            _stemRowMenu = stemRowMenu;
            _wordMenu = wordMenu;
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
            //_grid.CellPainting += OnCellPainting;
            _grid.ReadOnly = true;
            _grid.Visible = false;
        }

        private void OnCellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
           
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


        public void Refresh()
        {

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

            //RefreshCells();
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

    }
}
