namespace Comparatist.View.Utilities
{
    using System.Drawing;
    using System.Windows.Forms;

    namespace Comparatist.View.Utilities
    {
        internal enum DropPositions
        {
            Before,
            Inside,
            After,
            Cancel
        }

        internal static class DropPositionBox
        {
            public static DropPositions Show(string text, string caption)
            {
                using var form = CreateForm(caption);
                var label = CreateLabel(text);
                var buttonBefore = CreateButton("Before", new Point(10, 50));
                var buttonInside = CreateButton("Inside", new Point(100, 50));
                var buttonAfter = CreateButton("After", new Point(190, 50));
                var buttonCancel = CreateCancelButton(new Point(210, 85));

                DropPositions result = DropPositions.Cancel;

                buttonBefore.Click += (_, _) => { result = DropPositions.Before; form.DialogResult = DialogResult.OK; };
                buttonInside.Click += (_, _) => { result = DropPositions.Inside; form.DialogResult = DialogResult.OK; };
                buttonAfter.Click += (_, _) => { result = DropPositions.After; form.DialogResult = DialogResult.OK; };
                buttonCancel.Click += (_, _) => { result = DropPositions.Cancel; form.DialogResult = DialogResult.Cancel; };

                form.Controls.AddRange([label, buttonBefore, buttonInside, buttonAfter, buttonCancel]);

                form.AcceptButton = buttonInside;
                form.CancelButton = buttonCancel;

                return form.ShowDialog() == DialogResult.OK ? result : DropPositions.Cancel;
            }

            private static Form CreateForm(string title)
            {
                return new Form
                {
                    Text = title,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterParent,
                    MinimizeBox = false,
                    MaximizeBox = false,
                    ClientSize = new Size(300, 130)
                };
            }

            private static Label CreateLabel(string text)
            {
                return new Label
                {
                    Text = text,
                    AutoSize = true,
                    Location = new Point(10, 10)
                };
            }

            private static Button CreateButton(string text, Point location)
            {
                return new Button
                {
                    Text = text,
                    DialogResult = DialogResult.None,
                    Location = location,
                    Width = 75
                };
            }

            private static Button CreateCancelButton(Point location)
            {
                return new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = location,
                    Width = 70,
                    Anchor = AnchorStyles.Right
                };
            }
        }
    }

}
