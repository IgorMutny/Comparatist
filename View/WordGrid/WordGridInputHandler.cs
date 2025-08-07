using Comparatist.Core.Records;
using Comparatist.Services.Cache;
using Comparatist.View.Infrastructure;
using Comparatist.View.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace Comparatist.View.WordGrid
{
    internal class WordGridInputHandler : InputHandler<DataGridView>
    {
        private DisposableMenu _gridMenu;
        private DisposableMenu _rootMenu;
        private DisposableMenu _stemMenu;
        private DisposableMenu _wordMenu;

        public event Action<Root>? AddRootRequest;
        public event Action<Root>? UpdateRootRequest;
        public event Action<Root>? DeleteRootRequest;
        public event Action<Stem>? AddStemRequest;
        public event Action<Stem>? UpdateStemRequest;
        public event Action<Stem>? DeleteStemRequest;
        public event Action<Word>? AddWordRequest;
        public event Action<Word>? UpdateWordRequest;
        public event Action<Word>? DeleteWordRequest;

        public WordGridInputHandler(DataGridView control) : base(control)
        {
            Control.Dock = DockStyle.Fill;
            Control.AllowUserToAddRows = false;
            Control.RowHeadersVisible = false;
            Control.AutoGenerateColumns = false;
            Control.MultiSelect = false;
            Control.ReadOnly = true;
            Control.Visible = false;

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

            Control.MouseUp += OnMouseUp;
            Control.MouseDoubleClick += OnDoubleClick;
        }

        public IEnumerable<Category> AllCategories { private get; set; } = Enumerable.Empty<Category>();
        public IEnumerable<Root> AllRoots { private get; set; } = Enumerable.Empty<Root>();

        public override void Dispose()
        {
            Control.MouseUp -= OnMouseUp;
            Control.MouseDoubleClick -= OnDoubleClick;
        }

        private void AddRoot()
        {
            var form = new RootEditForm("Add root", new Root(), AllCategories);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var newRoot = form.GetResult();
                AddRootRequest?.Invoke(newRoot);
            }
        }

        private void EditRoot()
        {
            if (!TryGetSelected(out Root root))
                return;

            var form = new RootEditForm("Edit root", root, AllCategories);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var updatedRoot = form.GetResult();
                UpdateRootRequest?.Invoke(updatedRoot);
            }
        }

        private void DeleteRoot()
        {
            if (!TryGetSelected(out Root root))
                return;

            var result = MessageBox.Show(
                $"Delete '{root.Value}'? All stems and words of this root will also be deleted!",
                "Delete root",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
                DeleteRootRequest?.Invoke(root);
        }

        private void AddStem()
        {
            var selectedRootIds = new List<Guid>();
            bool isNative;

            if (TryGetSelected(out Root root))
            {
                selectedRootIds.Add(root.Id);
                isNative = root.IsNative;
            }
            else if (TryGetSelected(out Stem stem))
            {
                selectedRootIds.AddRange(stem.RootIds);
                isNative = stem.IsNative;
            }
            else
            {
                return;
            }

            var form = new StemEditForm(
                "Add stem",
                new Stem { IsNative = isNative },
                AllRoots,
                selectedRootIds);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var newStem = form.GetResult();
                AddStemRequest?.Invoke(newStem);
            }
        }

        private void EditStem()
        {
            if (!TryGetSelected(out Stem stem))
                return;

            var form = new StemEditForm("Edit stem", stem, AllRoots, stem.RootIds);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var updatedStem = form.GetResult();
                UpdateStemRequest?.Invoke(updatedStem);
            }
        }

        private void DeleteStem()
        {
            if (!TryGetSelected(out Stem stem))
                return;

            var result = MessageBox.Show(
                $"Delete '{stem.Value}'? All words of this stem will also be removed!",
                "Delete stem",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
                DeleteStemRequest?.Invoke(stem);
        }

        private void AddOrEditWord()
        {
            if (!TryGetSelected(out Word word))
                return;

            var cell = Control.SelectedCells[0];
            var row = Control.Rows[cell.RowIndex];
            var column = Control.Columns[cell.ColumnIndex];

            if (row.Cells[0].Tag is not Stem stem || column.Tag is not Language language)
                return;

            word.IsNative = stem.IsNative;
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
            if (!TryGetSelected(out Word word))
                return;

            var result = MessageBox.Show(
                $"Delete '{word.Value}'?",
                "Delete word",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
                DeleteWordRequest?.Invoke(word);
        }

        private bool TryGetSelected<T>([NotNullWhen(true)] out T result) where T : class, IRecord
        {
            result = default!;

            if (Control.SelectedCells.Count == 0)
                return false;

            if (Control.SelectedCells[0].Tag is not T tag)
                return false;

            result = tag;
            return true;
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            SelectCellUnderCursor(e);
            TryGetSelected(out IRecord tag);
            var menu = GetMenuFor(tag);
            var point = new Point(e.X, e.Y);
            menu?.Show(Control, point);
        }

        private DisposableMenu? GetMenuFor(object? tag)
        {
            return tag switch
            {
                Root => _rootMenu,
                Stem => _stemMenu,
                Word => _wordMenu,
                _ => _gridMenu
            };
        }

        private void OnDoubleClick(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            SelectCellUnderCursor(e);

            //if (TryGetSelected(out Root root))
                //_renderHelper.HandleDoubleClick(root, Control.SelectedCells[0].RowIndex);
        }

        private void SelectCellUnderCursor(MouseEventArgs e)
        {
            Control.ClearSelection();
            var point = new Point(e.X, e.Y);
            var hit = Control.HitTest(e.X, e.Y);

            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                var selectedCell = Control.Rows[hit.RowIndex].Cells[hit.ColumnIndex];
                selectedCell.Selected = true;
            }
        }
    }
}
