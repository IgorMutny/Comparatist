using Comparatist.View.Autoreplace;

namespace Comparatist.View.Utilities
{
    internal static class TextBoxExtensions
    {
        public static void EnableAutoReplace(this TextBox textBox)
        {
            EventHandler handler = null!;

            handler = (s, e) =>
            {
                var tb = (TextBox)s!;
                var caretPos = tb.SelectionStart;
                var originalLength = tb.Text.Length;

                string replaced = AutoReplaceManager.Instance.Apply(tb.Text);

                if (tb.Text != replaced)
                {
                    tb.TextChanged -= handler;
                    tb.Text = replaced;

                    int offset = replaced.Length - originalLength;
                    tb.SelectionStart = Math.Max(0, Math.Min(replaced.Length, caretPos + offset));

                    tb.TextChanged += handler;
                }
            };

            textBox.TextChanged += handler;
        }
    }
}
