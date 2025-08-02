using Comparatist.Core.Records;
using Comparatist.View.Utilities;
using System.Text;

namespace Comparatist
{
    public partial class StemEditForm : Form
    {
        private IEnumerable<Root> _allRoots;
        private List<Guid> _selectedRootIds;
        private Stem _stem;

        public StemEditForm(string header, Stem stem, IEnumerable<Root> allRoots, List<Guid> selectedRootIds)
        {
            InitializeComponent();

            Text = header;

            _stem = stem;
            _valueTextBox.Text = stem.Value;
            _translationTextBox.Text = stem.Translation;
            _commentTextBox.Text = stem.Comment;
            _nativeBox.Checked = stem.IsNative;
            _checkedBox.Checked = stem.IsChecked;
            _valueTextBox.EnableAutoReplace();
            _translationTextBox.EnableAutoReplace();
            _commentTextBox.EnableAutoReplace();

            _allRoots = allRoots;
            _selectedRootIds = selectedRootIds;
            _rootSelectionButton.Click += OnRootSelectionClicked;
            UpdateRootsTextBox();
        }

        public Stem GetResult()
        {
            return new Stem {
                Id = _stem.Id,
                Value = _valueTextBox.Text,
                Translation = _translationTextBox.Text,
                Comment = _commentTextBox.Text,
                IsNative = _nativeBox.Checked,
                IsChecked = _checkedBox.Checked,
                RootIds = _selectedRootIds
            };
        }

        private void UpdateRootsTextBox()
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
            var dialog = new RecordSelectionForm<Root>(_allRoots, _selectedRootIds);

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _selectedRootIds = dialog.SelectedIds;
                UpdateRootsTextBox();
            }
        }
    }
}