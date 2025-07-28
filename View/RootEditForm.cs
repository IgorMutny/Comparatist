namespace Comparatist
{
    public partial class RootEditForm : Form
    {
        public string ValueText
        {
            get => _valueTextBox.Text;
            set => _valueTextBox.Text = value;
        }

        public string TranslationText
        {
            get => _translationTextBox.Text;
            set => _translationTextBox.Text = value;
        }

        public string CommentText
        {
            get => _commentTextBox.Text;
            set => _commentTextBox.Text = value;
        }

        public bool NativeValue
        {
            get => _nativeBox.Checked;
            set => _nativeBox.Checked = value;
        }

        public bool CheckedValue
        {
            get => _checkedBox.Checked;
            set => _checkedBox.Checked = value;
        }

        public RootEditForm()
        {
            InitializeComponent();

            _okButton.Enabled = !string.IsNullOrWhiteSpace(_valueTextBox.Text);

            _valueTextBox.TextChanged += (_, _) =>
                 _okButton.Enabled = !string.IsNullOrWhiteSpace(_valueTextBox.Text);
        }

        public RootEditForm WithHeader(string header)
        {
            Text = header;
            return this;
        }
    }
}

