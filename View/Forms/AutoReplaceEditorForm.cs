using System;
namespace Comparatist.View.Forms
{
    internal partial class AutoReplaceEditorForm : Form
    {
        public Dictionary<string, string> Replacements { get; private set; }

        public AutoReplaceEditorForm(Dictionary<string, string> currentReplacements)
        {
            InitializeComponent();

            Replacements = new Dictionary<string, string>(currentReplacements);
            LoadData();
        }

        private void LoadData()
        {
            _grid.Rows.Clear();
            foreach (var kvp in Replacements)
            {
                _grid.Rows.Add(kvp.Key, kvp.Value);
            }
        }

        private void OnEndEdit(object? sender, EventArgs e)
        {
            var newReplacements = new Dictionary<string, string>();

            foreach (DataGridViewRow row in _grid.Rows)
            {
                if (row.IsNewRow) continue;

                var keyCell = row.Cells[0].Value;
                var valueCell = row.Cells[1].Value;

                if (keyCell == null || valueCell == null)
                    continue;

                var key = keyCell.ToString()?.Trim();
                var value = valueCell.ToString();

                if (string.IsNullOrEmpty(key))
                    continue;

                if (newReplacements.ContainsKey(key))
                {
                    MessageBox.Show($"Duplicate shortcut: {key}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                newReplacements[key] = value ?? "";
            }

            Replacements = newReplacements;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
