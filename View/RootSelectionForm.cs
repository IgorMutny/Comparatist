namespace Comparatist
{
    public partial class RootSelectionForm : Form
    {
        private List<Root> _roots;
        private List<Guid> _selectedIds;

        public List<Guid> SelectedIds
        {
            get
            {
                var result = new List<Guid>();
                foreach (var item in _checkedListBox.CheckedItems)
                {
                    if (item is Root root)
                        result.Add(root.Id);
                }
                return result;
            }
        }

        public RootSelectionForm(List<Root> roots, List<Guid> selectedIds)
        {
            InitializeComponent();

            _roots = roots;
            _selectedIds = selectedIds;

            PopulateList();
        }

        private void PopulateList()
        {
            _checkedListBox.DisplayMember = nameof(Root.Value);

            foreach (var root in _roots)
            {
                int index = _checkedListBox.Items.Add(root);

                if (_selectedIds.Contains(root.Id))
                    _checkedListBox.SetItemChecked(index, true);
            }
        }
    }
}
