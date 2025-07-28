using System.Text;

namespace Comparatist
{
    public partial class StemEditForm : Form
    {
        private List<Root> _allRoots;
        private List<Guid> _selectedRootIds;

        public string ValueText { get => _valueTextBox.Text; set => _valueTextBox.Text = value; }
        public string TranslationText { get => _translationTextBox.Text; set => _translationTextBox.Text = value; }
        public string CommentText { get => _commentTextBox.Text; set => _commentTextBox.Text = value; }
        public bool NativeValue { get => _nativeBox.Checked; set => _nativeBox.Checked = value; }
        public bool CheckedValue { get => _checkedBox.Checked; set => _checkedBox.Checked = value; }
        public List<Guid> SelectedRootIds => _selectedRootIds;

        public StemEditForm(string header, List<Root> allRoots, List<Guid> selectedRootIds)
        {
            InitializeComponent();

            Text = header;

            _allRoots = allRoots;
            _selectedRootIds = selectedRootIds;
            _rootSelectionButton.Click += OnRootSelectionClicked;

            UpdateRootList();
        }

        private void UpdateRootList()
        {
            var values = new List<string>();

            foreach (var rootId in _selectedRootIds)
            {
                var root = _allRoots.FirstOrDefault(x => x.Id == rootId);

                if (root != null)
                    values.Add(root.Value);
            }

            var builder = new StringBuilder();
            builder.AppendJoin(", ", values);
            _rootsTextBox.Text = builder.ToString();
        }

        private void OnRootSelectionClicked(object? sender, EventArgs e)
        {
            var dialog = new RootSelectionForm(_allRoots, _selectedRootIds);

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _selectedRootIds = dialog.SelectedIds;
                UpdateRootList();
            }
        }
    }
}