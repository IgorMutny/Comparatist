using Comparatist.Core.Records;
using Comparatist.View.Tags;

namespace Comparatist
{
    public partial class CategorySelectionForm : Form
    {
        private List<CategoryTag> _categories;
        private List<Guid> _selectedIds;

        public List<Guid> SelectedIds
        {
            get
            {
                var result = new List<Guid>();
                foreach (var item in _checkedListBox.CheckedItems)
                {
                    if (item is Category group)
                        result.Add(group.Id);
                }
                return result;
            }
        }

        public CategorySelectionForm(List<CategoryTag> categories, List<Guid> selectedIds)
        {
            InitializeComponent();

            _categories = categories;
            _selectedIds = selectedIds;

            _checkedListBox.Dock = DockStyle.Fill;
            _checkedListBox.CheckOnClick = true;

            PopulateList();
        }

        private void PopulateList()
        {
            _checkedListBox.DisplayMember = nameof(Category.Value);

            foreach (var group in _categories)
            {
                int index = _checkedListBox.Items.Add(group);

                if (_selectedIds.Contains(group.Id))
                    _checkedListBox.SetItemChecked(index, true);
            }
        }
    }
}
