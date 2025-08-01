using Comparatist.View.Tags;

namespace Comparatist
{
    public partial class WordEditForm : Form
    {
        private WordTag _baseTag;
        private StemTag _stemTag;
        private LanguageTag _languageTag;

        public WordEditForm(WordTag baseTag, StemTag stemTag, LanguageTag languageTag)
        {
            InitializeComponent();

            _baseTag = baseTag;
            _stemTag = stemTag;
            _languageTag = languageTag;
            _valueTextBox.Text = baseTag.Value;
            _translationTextBox.Text = baseTag.Translation;
            _commentTextBox.Text = baseTag.Comment;
            _stemTextBox.Text = stemTag.Value;
            _languageTextBox.Text = languageTag.Value;
            _checkedBox.Checked = baseTag.IsChecked;
            _nativeBox.Checked = baseTag.IsNative;
        }

        public WordTag GetResult()
        {
            return new WordTag(
                id: _baseTag.Id,
                value: _valueTextBox.Text,
                translation: _translationTextBox.Text,
                comment: _commentTextBox.Text,
                isNative: _nativeBox.Checked,
                isChecked: _checkedBox.Checked,
                stemId: _stemTag.Id,
                languageId: _languageTag.Id
                );
        }
    }
}
