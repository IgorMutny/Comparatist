using Comparatist.Core.Records;
using Comparatist.View.Utilities;

namespace Comparatist
{
    public partial class RecordSelectionForm<T> : Form where T : IRecord
    {
        private IEnumerable<T> _allRecords;
        private List<Guid> _selectedIds;

        public List<Guid> SelectedIds
        {
            get
            {
                var result = new List<Guid>();

                foreach (GuidedItem item in _checkedListBox.CheckedItems)
                    result.Add(item.Id);

                return result;
            }
        }

        public RecordSelectionForm(IEnumerable<T> allCategories, List<Guid> selectedIds)
        {
            InitializeComponent();

            _allRecords = allCategories;
            _selectedIds = selectedIds;

            _checkedListBox.Dock = DockStyle.Fill;
            _checkedListBox.CheckOnClick = true;

            PopulateList();
        }

        private void PopulateList()
        {
            foreach (var record in _allRecords)
            {
                var item = new GuidedItem
                {
                    Text = record.ToString() ?? string.Empty,
                    Id = record.Id
                };

                int index = _checkedListBox.Items.Add(item);

                if (_selectedIds.Contains(record.Id))
                    _checkedListBox.SetItemChecked(index, true);
            }
        }
    }
}
