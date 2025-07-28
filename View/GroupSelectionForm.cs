namespace Comparatist
{
    public partial class GroupSelectionForm : Form
    {
        private List<SemanticGroup> _groups;
        private List<Guid> _selectedIds;

        public List<Guid> SelectedIds
        {
            get
            {
                var result = new List<Guid>();
                foreach (var item in _checkedListBox.CheckedItems)
                {
                    if (item is SemanticGroup group)
                        result.Add(group.Id);
                }
                return result;
            }
        }

        public GroupSelectionForm(List<SemanticGroup> groups, List<Guid> selectedIds)
        {
            InitializeComponent();

            _groups = groups;
            _selectedIds = selectedIds;

            _checkedListBox.Dock = DockStyle.Fill;
            _checkedListBox.CheckOnClick = true;

            PopulateList();
        }

        private void PopulateList()
        {
            _checkedListBox.DisplayMember = nameof(SemanticGroup.Value);

            foreach (var group in _groups)
            {
                int index = _checkedListBox.Items.Add(group);

                if (_selectedIds.Contains(group.Id))
                    _checkedListBox.SetItemChecked(index, true);
            }
        }
    }
}
