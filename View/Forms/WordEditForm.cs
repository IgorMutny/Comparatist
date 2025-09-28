using Comparatist.Data.Entities;
using Comparatist.View.Utilities;

namespace Comparatist
{
    public partial class WordEditForm : Form
    {
        private Word _word;
        private Stem _stem;
        private Language _language;

        public WordEditForm(Word word, Stem stem, Language language)
        {
            InitializeComponent();

            _word = word;
            _stem = stem;
            _language = language;

            _valueTextBox.Text = word.Value;
            _translationTextBox.Text = word.Translation;
            _commentTextBox.Text = word.Comment;
            _stemTextBox.Text = stem.Value;
            _languageTextBox.Text = language.Value;
            _checkedBox.Checked = word.IsChecked;
            _valueTextBox.EnableAutoReplace();
            _translationTextBox.EnableAutoReplace();
            _commentTextBox.EnableAutoReplace();

            _valueTextBox.TextChanged += (_, _)
                => _okButton.Enabled = !string.IsNullOrWhiteSpace(_valueTextBox.Text);
        }

        public Word GetResult()
        {
            return new Word
            {
                Id = _word.Id,
                Value = _valueTextBox.Text,
                Translation = _translationTextBox.Text,
                Comment = _commentTextBox.Text,
                IsChecked = _checkedBox.Checked,
                StemId = _stem.Id,
                LanguageId = _language.Id
            };
        }
    }
}
