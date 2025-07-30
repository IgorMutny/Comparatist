using Comparatist.View.Tags;
using System.Text;

namespace Comparatist
{
    public partial class StemEditForm : Form
    {
        private List<RootTag> _allRoots;
        private List<Guid> _selectedRootIds;
        private StemTag _baseTag;

        public StemEditForm(string header,
            StemTag baseTag,
            List<RootTag> allRoots,
            IReadOnlyList<Guid> selectedRootIds)
        {
            InitializeComponent();

            Text = header;

            _baseTag = baseTag;
            _allRoots = allRoots;
            _selectedRootIds = (List<Guid>)selectedRootIds;
            _valueTextBox.Text = baseTag.Value;
            _translationTextBox.Text = baseTag.Translation;
            _commentTextBox.Text = baseTag.Comment;
            _nativeBox.Checked = baseTag.IsNative;
            _checkedBox.Checked = baseTag.IsChecked;
            _rootSelectionButton.Click += OnRootSelectionClicked;

            UpdateRootList();
        }

        public StemTag GetResult()
        {
            return new StemTag(
                id: _baseTag.Id,
                value: _valueTextBox.Text,
                translation: _translationTextBox.Text,
                comment: _commentTextBox.Text,
                isNative: _nativeBox.Checked,
                isChecked: _checkedBox.Checked,
                rootIds: _selectedRootIds
                );
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