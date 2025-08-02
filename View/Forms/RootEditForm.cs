using Comparatist.Core.Records;
using System.Text;

namespace Comparatist
{
    public partial class RootEditForm : Form
    {
        private IEnumerable<Category> _allCategories;
        private List<Guid> _selectedCategoryIds;
        private Root _root;

        public RootEditForm(string header, Root root, IEnumerable<Category> allCategories)
        {
            InitializeComponent();

            Text = header;

            _root = root;
            _valueTextBox.Text = root.Value;
            _translationTextBox.Text = root.Translation;
            _commentTextBox.Text = root.Comment;
            _nativeBox.Checked = root.IsNative;
            _checkedBox.Checked = root.IsChecked;

            _allCategories = allCategories;
            _selectedCategoryIds = root.CategoryIds;
            _groupSelectionButton.Click += OnCategorySelectionClicked;
            UpdateCategoriesTextBox();
        }

        public Root GetResult()
        {
            return new Root
            {
                Id = _root.Id,
                Value = _valueTextBox.Text,
                Translation = _translationTextBox.Text,
                Comment = _commentTextBox.Text,
                IsNative = _nativeBox.Checked,
                IsChecked = _checkedBox.Checked,
                CategoryIds = _selectedCategoryIds
            };
        }

        private void UpdateCategoriesTextBox()
        {
            var values = new List<string>();

            foreach (var id in _selectedCategoryIds)
            {
                var category = _allCategories.FirstOrDefault(x => x.Id == id);

                if (category != null)
                    values.Add(category.Value);
            }

            var builder = new StringBuilder();
            builder.AppendJoin(", ", values);
            _groupsTextBox.Text = builder.ToString();
        }

        private void OnCategorySelectionClicked(object? sender, EventArgs e)
        {
            var dialog = new RecordSelectionForm<Category>(_allCategories, _selectedCategoryIds);

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _selectedCategoryIds = dialog.SelectedIds;
                UpdateCategoriesTextBox();
            }
        }
    }
}