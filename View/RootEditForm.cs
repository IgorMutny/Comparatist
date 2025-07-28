using System.Text;

namespace Comparatist
{
    public partial class RootEditForm : Form
    {
        private List<SemanticGroup> _allGroups;
        private List<Guid> _selectedGroupIds;

        public string ValueText { get => _valueTextBox.Text; set => _valueTextBox.Text = value; }
        public string TranslationText { get => _translationTextBox.Text; set => _translationTextBox.Text = value; }
        public string CommentText { get => _commentTextBox.Text; set => _commentTextBox.Text = value; }
        public bool NativeValue { get => _nativeBox.Checked; set => _nativeBox.Checked = value; }
        public bool CheckedValue { get => _checkedBox.Checked; set => _checkedBox.Checked = value; }
        public List<Guid> SelectedGroupIds => _selectedGroupIds;

        public RootEditForm(string header, List<SemanticGroup> allGroups, List<Guid> selectedIds)
        {
            InitializeComponent();

            Text = header;

            _allGroups = allGroups;
            _selectedGroupIds = selectedIds;
            _groupSelectionButton.Click += OnGroupSelectionClicked;

            UpdateGroupList();
        }

        private void UpdateGroupList()
        {
            var values = new List<string>();

            foreach (var groupId in _selectedGroupIds)
            {
                var group = _allGroups.FirstOrDefault(x => x.Id == groupId);

                if (group != null)
                    values.Add(group.Value);
            }

            var builder = new StringBuilder();
            builder.AppendJoin(", ", values);
            _groupsTextBox.Text = builder.ToString();
        }

        private void OnGroupSelectionClicked(object? sender, EventArgs e)
        {
            var dialog = new GroupSelectionForm(_allGroups, _selectedGroupIds);

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _selectedGroupIds = dialog.SelectedIds;
                UpdateGroupList();
            }
        }
    }
}