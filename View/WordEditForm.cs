namespace Comparatist
{
    public partial class WordEditForm : Form
    {
        public string ValueText { get => _valueTextBox.Text; set => _valueTextBox.Text = value; }
        public string TranslationText { get => _translationTextBox.Text; set => _translationTextBox.Text = value; }
        public string CommentText { get => _commentTextBox.Text; set => _commentTextBox.Text = value; }
        public string StemText { get => _stemTextBox.Text; set => _stemTextBox.Text = value; }
        public string LanguageText { get => _languageTextBox.Text; set => _languageTextBox.Text = value; }
        public bool NativeValue { get => _nativeBox.Checked; set => _nativeBox.Checked = value; }
        public bool CheckedValue { get => _checkedBox.Checked; set => _checkedBox.Checked = value; }

        public WordEditForm(string stemValue, string languageValue)
        {
            InitializeComponent();

            StemText = stemValue;
            LanguageText = languageValue;
        }
    }
}
