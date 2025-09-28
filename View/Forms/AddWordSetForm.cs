using Comparatist.Data.Entities;
using Comparatist.View.Utilities;

namespace Comparatist.View.Forms
{
    internal partial class AddWordSetForm : Form
    {
        private Dictionary<Guid, DataGridViewCell> _resultingCells = new();
        private string _stemValue;

        public AddWordSetForm(
            string stemValue,
            IEnumerable<Language> languages,
            Dictionary<Guid, string> existingWords)
        {
            _stemValue = stemValue;

            InitializeComponent();

            _grid.EnableAutoReplace();

            foreach (Language language in languages)
            {
                int index = _grid.Rows.Add();
                var row = _grid.Rows[index];

                var languageCell = row.Cells[0];
                languageCell.Value = language.Value;
                languageCell.ReadOnly = true;
                languageCell.Style.BackColor = Color.LightGray;

                var wordCell = row.Cells[1];

                if (existingWords.TryGetValue(language.Id, out var wordValue))
                {
                    wordCell.Value = wordValue;
                    wordCell.ReadOnly = true;
                    wordCell.Style.BackColor = Color.LightGray;
                }
                else
                {
                    wordCell.Value = string.Empty;
                    wordCell.ReadOnly = false;
                    wordCell.Style.BackColor = Color.White;
                    _resultingCells.Add(language.Id, wordCell);
                }

                _okButton.Click += OnOkButtonClick;
                _cancelButton.Click += OnCancelButtonClick;
            }
        }

        public Dictionary<Guid, string>? Values { get; private set; }

        private void OnOkButtonClick(object? sender, EventArgs e)
        {
            Values = _resultingCells
                .Where(e => !string.IsNullOrWhiteSpace(e.Value.Value as string))
                .Select(pair => new KeyValuePair<Guid, string>(
                    pair.Key,
                    (string)pair.Value.Value))
                .ToDictionary();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnCancelButtonClick(object? sender, EventArgs e)
        {
            Values = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
