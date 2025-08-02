using System.Text.RegularExpressions;

namespace Comparatist.View.Utilities
{
    public static class RichTextFormatter
    {
        private static readonly Regex _tagRegex = new(@"\[(/?)(b|i|u|s)\]", RegexOptions.IgnoreCase);

        public static List<(string Text, FontStyle Style)> Parse(string input)
        {
            var result = new List<(string Text, FontStyle Style)>();
            var styleStack = new Stack<FontStyle>();
            styleStack.Push(FontStyle.Regular);

            int lastIndex = 0;

            foreach (Match match in _tagRegex.Matches(input))
            {
                if (match.Index > lastIndex)
                {
                    string plainText = input.Substring(lastIndex, match.Index - lastIndex);
                    result.Add((plainText, styleStack.Peek()));
                }

                string tag = match.Groups[2].Value.ToLower();
                bool closing = match.Groups[1].Value == "/";

                var currentStyle = styleStack.Peek();

                if (closing)
                {
                    // Pop matching style
                    var popped = styleStack.Pop();
                }
                else
                {
                    FontStyle newStyle = tag switch
                    {
                        "b" => currentStyle | FontStyle.Bold,
                        "i" => currentStyle | FontStyle.Italic,
                        "u" => currentStyle | FontStyle.Underline,
                        "s" => currentStyle | FontStyle.Strikeout,
                        _ => currentStyle
                    };
                    styleStack.Push(newStyle);
                }

                lastIndex = match.Index + match.Length;
            }

            if (lastIndex < input.Length)
            {
                result.Add((input.Substring(lastIndex), styleStack.Peek()));
            }

            return result;
        }
    }
}