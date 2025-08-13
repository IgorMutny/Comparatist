using Comparatist.View.Autoreplace;

namespace Comparatist.View.Utilities
{
    internal static class DataGridViewExtensions
    {
        public static void EnableAutoReplace(this DataGridView grid)
        {
            grid.EditingControlShowing += (s, e) =>
            {
                if (e.Control is TextBox textBox)
                {
                    textBox.TextChanged -= TextBoxOnTextChanged;
                    textBox.TextChanged += TextBoxOnTextChanged;
                }
            };
        }

        private static void TextBoxOnTextChanged(object? sender, EventArgs e)
        {
            if (sender is not TextBox tb) return;

            var caretPos = tb.SelectionStart;
            var originalLength = tb.Text.Length;

            string replaced = AutoReplaceManager.Instance.Apply(tb.Text);

            if (tb.Text != replaced)
            {
                tb.TextChanged -= TextBoxOnTextChanged;
                tb.Text = replaced;

                int offset = replaced.Length - originalLength;
                tb.SelectionStart = Math.Max(0, Math.Min(replaced.Length, caretPos + offset));

                tb.TextChanged += TextBoxOnTextChanged;
            }
        }
    }
}
