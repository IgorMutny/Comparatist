using Comparatist.Data.Entities;
using Comparatist.Application.Cache;
using Comparatist.View.Common;
using Comparatist.View.Utilities;
using System.Diagnostics.CodeAnalysis;
using Comparatist.View.Forms;

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
        public event Action<Root>? ExpandOrCollapseRequest;

        public WordGridInputHandler(DataGridView control) : base(control)
        {
            Control.AllowUserToAddRows = false;
            Control.RowHeadersVisible = false;
            Control.AutoGenerateColumns = false;
            Control.MultiSelect = false;
            Control.ReadOnly = true;
            Control.Visible = false;

            _gridMenu = new DisposableMenu(
                ("Add root", AddRoot));

            _rootMenu = new DisposableMenu(
                ("Expand / collapse", ExpandOrCollapse),
                ("Add root", AddRoot),
                ("Edit root", EditRoot),
                ("Delete root", DeleteRoot),
                ("Add stem", AddStem));

            _stemMenu = new DisposableMenu(
                ("Add stem", AddStem),
                ("Edit stem", EditStem),
                ("Delete stem", DeleteStem),
                ("Add word set", AddWordSet));

            _wordMenu = new DisposableMenu(
                ("Add or edit word", AddOrEditWord),
                ("Delete word", DeleteWord));

            Control.MouseUp += OnMouseUp;
            Control.MouseDoubleClick += OnDoubleClick;
        }

        public List<Category> AllCategories { private get; set; } = new();
        public List<Root> AllRoots { private get; set; } = new();

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

                var stemRequired = MessageBox.Show(
                    $"Add default stem for {newRoot.Value}?",
                    "Add default stem",
                    MessageBoxButtons.YesNo);

                if (stemRequired == DialogResult.Yes)
                {
                    var stem = new Stem
                    {
                        Value = newRoot.Value,
                        Translation = newRoot.Translation,
                        RootIds = { newRoot.Id }
                    };

                    AddStemRequest?.Invoke(stem);
                }
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
                ExpandOrCollapseRequest?.Invoke(updatedRoot);
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

            if (TryGetSelected(out Root root))
                selectedRootIds.Add(root.Id);
            else if (TryGetSelected(out Stem stem))
                selectedRootIds.AddRange(stem.RootIds);
            else
                return;

            var form = new StemEditForm(
                "Add stem",
                new Stem(),
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
                AddWord();
            else
                EditWord(word);
        }

        private void AddWord()
        {
            var cell = Control.SelectedCells[0];
            var row = Control.Rows[cell.RowIndex];
            var column = Control.Columns[cell.ColumnIndex];

            if (row.Cells[0].Tag is not Stem stem || column.Tag is not Language language)
                return;

            var word = new Word();

            var form = new WordEditForm(word, stem, language);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var updatedWord = form.GetResult();
                AddWordRequest?.Invoke(updatedWord);
            }
        }

        private void EditWord(Word word)
        {
            var cell = Control.SelectedCells[0];
            var row = Control.Rows[cell.RowIndex];
            var column = Control.Columns[cell.ColumnIndex];

            if (row.Cells[0].Tag is not Stem stem || column.Tag is not Language language)
                return;

            var form = new WordEditForm(word, stem, language);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var updatedWord = form.GetResult();
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


        private void AddWordSet()
        {
            if (Control.SelectedCells.Count == 0)
                return;

            var row = Control.Rows[Control.SelectedCells[0].RowIndex];

            if (row.Cells[0].Tag is not Stem stem)
                return;

            var languages = new List<Language>();
            var existingWords = new Dictionary<Guid, string>();
            CollectAddWordSetFormData(row, languages, existingWords);

            var dialog = new AddWordSetForm(stem.Value, languages, existingWords);

            if (dialog.ShowDialog() == DialogResult.OK && dialog.Values != null)
                AddWordsFromSet(stem, dialog.Values);
        }

        private void AddWordsFromSet(Stem stem, Dictionary<Guid, string> result)
        {
            foreach (var pair in result)
            {
                var word = new Word
                {
                    Value = pair.Value,
                    Translation = stem.Translation,
                    IsChecked = false,
                    StemId = stem.Id,
                    LanguageId = pair.Key,
                };

                AddWordRequest?.Invoke(word);
            }
        }

        private void CollectAddWordSetFormData(DataGridViewRow row, List<Language> languages, Dictionary<Guid, string> existingWords)
        {
            for (int i = 1; i < Control.Columns.Count; i++)
            {
                if (Control.Columns[i].Tag is not Language language)
                    continue;

                languages.Add(language);
                var cell = row.Cells[i];

                if (cell.Tag is Word word)
                    existingWords.Add(language.Id, word.Value);
            }
        }

        private void ExpandOrCollapse()
        {
            if (TryGetSelected(out Root root))
                ExpandOrCollapseRequest?.Invoke(root);
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

            SelectCellUnderCursor(e, out var cell);
            TryGetSelected(out IRecord tag);
            var menu = GetMenuFor(cell, tag);
            var point = new Point(e.X, e.Y);
            menu?.Show(Control, point);
        }

        private DisposableMenu? GetMenuFor(DataGridViewCell? cell, object? tag)
        {
            if (tag is null
                && cell is not null
                && Control.Rows[cell.RowIndex].Cells[0].Tag is Stem)
            {
                return _wordMenu;
            }

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

            if (TryGetSelected(out Root root))
                ExpandOrCollapseRequest?.Invoke(root);
            else if (Control.SelectedCells.Count > 0 && Control.Rows[Control.SelectedCells[0].RowIndex].Cells[0].Tag is Stem)
                AddOrEditWord();
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

        private void SelectCellUnderCursor(MouseEventArgs e, out DataGridViewCell? cell)
        {
            SelectCellUnderCursor(e);

            cell = null;

            if (Control.SelectedCells.Count > 0)
                cell = Control.SelectedCells[0];
        }
    }
}
