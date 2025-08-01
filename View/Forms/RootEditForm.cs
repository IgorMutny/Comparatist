using Comparatist.View.Tags;
using System.Text;

namespace Comparatist
{
    public partial class RootEditForm : Form
    {
        private List<CategoryTag> _allCategories;
        private List<Guid> _selectedCategoryIds;
        private RootTag _baseTag;

        public RootEditForm(string header, RootTag baseTag, List<CategoryTag> allCategories)
        {
            InitializeComponent();

            Text = header;

            _baseTag = baseTag;
            _allCategories = allCategories;
            _selectedCategoryIds = (List<Guid>)baseTag.CategoryIds;
            _valueTextBox.Text = baseTag.Value;
            _translationTextBox.Text = baseTag.Translation;
            _commentTextBox.Text = baseTag.Comment;
            _nativeBox.Checked = baseTag.IsNative;
            _checkedBox.Checked = baseTag.IsChecked;
            _groupSelectionButton.Click += OnCategorySelectionClicked;

            UpdateGroupList();
        }

        public RootTag GetResult()
        {
            return new RootTag(
                id: _baseTag.Id,
                value: _valueTextBox.Text,
                translation: _translationTextBox.Text,
                comment: _commentTextBox.Text,
                isNative: _nativeBox.Checked,
                isChecked: _checkedBox.Checked,
                categoryIds: _selectedCategoryIds
                );
        }

        private void UpdateGroupList()
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
            var dialog = new CategorySelectionForm(_allCategories, _selectedCategoryIds);

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _selectedCategoryIds = dialog.SelectedIds;
                UpdateGroupList();
            }
        }
    }
}