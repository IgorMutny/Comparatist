using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparatist
{
    public static class InputDialog
    {
        public static string? Show(string title, string prompt)
        {
            var form = new Form
            {
                Text = title,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false,
                ClientSize = new Size(300, 130)
            };

            var label = new Label
            {
                Text = prompt,
                AutoSize = true,
                Location = new Point(10, 10)
            };

            var textBox = new TextBox
            {
                Location = new Point(10, 35),
                Width = 270,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };

            var buttonOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Enabled = false,
                Anchor = AnchorStyles.Right,
                Location = new Point(130, 75),
                Width = 70
            };

            var buttonCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Anchor = AnchorStyles.Right,
                Location = new Point(210, 75),
                Width = 70
            };

            textBox.TextChanged += (s, e) =>
            {
                buttonOK.Enabled = !string.IsNullOrWhiteSpace(textBox.Text);
            };

            form.Controls.AddRange([label, textBox, buttonOK, buttonCancel]);

            form.AcceptButton = buttonOK;
            form.CancelButton = buttonCancel;

            var result = form.ShowDialog();
            return result == DialogResult.OK ? textBox.Text.Trim() : null;
        }
    }
}
