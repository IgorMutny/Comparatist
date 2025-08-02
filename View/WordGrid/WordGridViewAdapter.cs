using Comparatist.Core.Records;
using Comparatist.Services.TableCache;
using Comparatist.View.Infrastructure;
using Comparatist.View.Utities;

namespace Comparatist.View.WordGrid
{
    internal class WordGridViewAdapter : ViewAdapter<DataGridView>
    {
        private DisposableMenu _gridMenu;
        private DisposableMenu _rootMenu;
        private DisposableMenu _stemMenu;
        private DisposableMenu _wordMenu;
        private WordGridRenderHelper _renderHelper;

        public event Action<Root>? AddRootRequest;
        public event Action<Root>? UpdateRootRequest;
        public event Action<Root>? DeleteRootRequest;
        public event Action<Stem>? AddStemRequest;
        public event Action<Stem>? UpdateStemRequest;
        public event Action<Stem>? DeleteStemRequest;
        public event Action<Word>? AddWordRequest;
        public event Action<Word>? UpdateWordRequest;
        public event Action<Word>? DeleteWordRequest;

        public WordGridViewAdapter(DataGridView grid) : base(grid)
        {
            _renderHelper = new WordGridRenderHelper(grid);

            _gridMenu = new DisposableMenu(
                ("Add root", AddRoot));

            _rootMenu = new DisposableMenu(
                ("Add root", AddRoot),
                ("Edit root", EditRoot),
                ("Delete root", DeleteRoot),
                ("Add stem", AddStem));

            _stemMenu = new DisposableMenu(
                ("Add stem", AddStem),
                ("Edit stem", EditStem),
                ("Delete stem", DeleteStem));

            _wordMenu = new DisposableMenu(
                ("Add or edit word", AddOrEditWord),
                ("Delete word", DeleteWord));

            SetupGrid();
        }

        public IEnumerable<Category> AllCategories { private get; set; } = Enumerable.Empty<Category>();
        public IEnumerable<Language> AllLanguages { private get; set; } = Enumerable.Empty<Language>();
        private IEnumerable<Root> AllRoots { get; set; } = Enumerable.Empty<Root>();

        private void SetupGrid()
        {
            Control.Dock = DockStyle.Fill;
            Control.AllowUserToAddRows = false;
            Control.RowHeadersVisible = false;
            Control.AutoGenerateColumns = false;
            Control.MultiSelect = false;
            Control.ReadOnly = true;
            Control.Visible = false;
            Control.CellPainting += OnCellPainting;
            Control.MouseUp += OnMouseUp;
            Control.MouseDoubleClick += OnDoubleClick;
        }

        public void Render(IEnumerable<CachedBlock> blocks)
        {
            AllRoots = blocks.Select(x => x.Root);
            _renderHelper.Render(blocks, AllLanguages);
        }

        protected override void Unsubscribe()
        {
            Control.CellPainting -= OnCellPainting;
            Control.MouseUp -= OnMouseUp;
            Control.MouseDoubleClick -= OnDoubleClick;
        }

        private void AddRoot()
        {
            var form = new RootEditForm("Add Root", new Root(), AllCategories);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var newRoot = form.GetResult();
                AddRootRequest?.Invoke(newRoot);
            }
        }

        private void EditRoot()
        {
            if (Control.SelectedCells.Count == 0 || Control.SelectedCells[0].Tag is not Root root)
                return;

            var form = new RootEditForm("Edit Root", root, AllCategories);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var updatedRoot = form.GetResult();
                UpdateRootRequest?.Invoke(updatedRoot);
            }
        }

        private void DeleteRoot()
        {
            if (Control.SelectedCells.Count == 0 || Control.SelectedCells[0].Tag is not Root root)
                return;

            var result = MessageBox.Show(
                $"Delete '{root.Value}'? All stems and words of this root will also be deleted!",
                "Delete root",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                var deletedRoot = (Root)root.Clone();
                DeleteRootRequest?.Invoke(deletedRoot);
            }
        }

        private void AddStem()
        {
            if (Control.SelectedCells.Count == 0)
                return;

            var selectedRootIds = new List<Guid>();

            if (Control.SelectedCells[0].Tag is Root root)
                selectedRootIds.Add(root.Id);
            else if (Control.SelectedCells[0].Tag is Stem stem)
                selectedRootIds.AddRange(stem.RootIds);
            else
                return;

            var form = new StemEditForm("Add Stem", new Stem(), AllRoots, selectedRootIds);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var newStem = form.GetResult();
                AddStemRequest?.Invoke(newStem);
            }
        }

        private void EditStem()
        {
            if (Control.SelectedCells.Count == 0 || Control.SelectedCells[0].Tag is not Stem stem)
                return;

            var form = new StemEditForm("Add Stem", stem, AllRoots, stem.RootIds);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var updatedStem = form.GetResult();
                UpdateStemRequest?.Invoke(updatedStem);
            }
        }

        private void DeleteStem()
        {
            if (Control.SelectedCells.Count == 0 || Control.SelectedCells[0].Tag is not Stem stem)
                return;

            var result = MessageBox.Show(
                $"Delete '{stem.Value}'? All words of this stem will also be removed!",
                "Delete stem",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                var deletedStem = (Stem)stem.Clone();
                DeleteStemRequest?.Invoke(deletedStem);
            }
        }

        private void AddOrEditWord()
        {
            if (Control.SelectedCells.Count == 0 || Control.SelectedCells[0].Tag is not Word word)
                return;

            var cell = Control.SelectedCells[0];
            var row = Control.Rows[cell.RowIndex];
            var column = Control.Columns[cell.ColumnIndex];

            if (row.Cells[0].Tag is not Stem stem || column.Tag is not Language language)
                return;

            var form = new WordEditForm(word, stem, language);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var updatedWord = form.GetResult();
                if (updatedWord.Id == Guid.Empty)
                    AddWordRequest?.Invoke(updatedWord);
                else
                    UpdateWordRequest?.Invoke(updatedWord);
            }
        }

        private void DeleteWord()
        {
            if (Control.SelectedCells.Count == 0 || Control.SelectedCells[0].Tag is not Word word)
                return;

            var result = MessageBox.Show(
                $"Delete '{word.Value}'?",
                "Delete word",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                var deletedWord = (Word)word.Clone();
                DeleteWordRequest?.Invoke(deletedWord);
            }
        }

        private void OnCellPainting(object? sender, DataGridViewCellPaintingEventArgs e) =>
            _renderHelper.OnCellPainting(e);

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

                if (selectedCell.Tag is Root)
                    _rootMenu.Show(Control, point);
                else if (selectedCell.Tag is Stem)
                    _stemMenu.Show(Control, point);
                else if (selectedCell.Tag is Word)
                    _wordMenu.Show(Control, point);
                else
                    _gridMenu.Show(Control, point);
            }
            else
            {
                _gridMenu.Show(Control, point);
            }
        }

        private void OnDoubleClick(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            Control.ClearSelection();
            var point = new Point(e.X, e.Y);
            var hit = Control.HitTest(e.X, e.Y);

            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                var selectedCell = Control.Rows[hit.RowIndex].Cells[hit.ColumnIndex];
                selectedCell.Selected = true;

                if (selectedCell.Tag is Root root)
                    _renderHelper.HandleDoubleClick(root, hit.RowIndex);
            }
        }
    }
}
