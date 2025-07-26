namespace Comparatist
{
    public static class InputBox
    {
        public static string? Show(string title, string prompt)
        {
            using var form = CreateForm(title);
            var label = CreateLabel(prompt);
            var textBox = CreateTextBox();
            var buttonOK = CreateOkButton();
            var buttonCancel = CreateCancelButton();

            textBox.TextChanged += (_, _) => buttonOK.Enabled = !string.IsNullOrWhiteSpace(textBox.Text);
            form.Controls.AddRange([label, textBox, buttonOK, buttonCancel]);
            form.AcceptButton = buttonOK;
            form.CancelButton = buttonCancel;

            var result = form.ShowDialog();
            return result == DialogResult.OK ? textBox.Text.Trim() : null;
        }

        private static Form CreateForm(string title) => new()
        {
            Text = title,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterParent,
            MinimizeBox = false,
            MaximizeBox = false,
            ClientSize = new Size(300, 130)
        };

        private static Label CreateLabel(string prompt) => new()
        {
            Text = prompt,
            AutoSize = true,
            Location = new Point(10, 10)
        };

        private static TextBox CreateTextBox() => new()
        {
            Location = new Point(10, 35),
            Width = 270,
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
        };

        private static Button CreateOkButton() => new()
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Enabled = false,
            Anchor = AnchorStyles.Right,
            Location = new Point(130, 75),
            Width = 70
        };

        private static Button CreateCancelButton() => new()
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Anchor = AnchorStyles.Right,
            Location = new Point(210, 75),
            Width = 70
        };
    }
}

