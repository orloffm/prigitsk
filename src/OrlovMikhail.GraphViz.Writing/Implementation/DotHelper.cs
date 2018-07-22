using System.Text;
using System.Text.RegularExpressions;

namespace OrlovMikhail.GraphViz.Writing
{
    public class DotHelper
        : IDotHelper
    {
        private const string AlphaNumericRegexPattern = @"^[a-zA-Z\200-\377][a-zA-Z\200-\3770-9]*$";
        private const string NumeralRegexPattern = @"^[-]?(?:\.[0-9]+|[0-9]+(\.[0-9]*)?)$";

        public string EscapeId(string s)
        {
            s = s.Trim();
            bool isAlphaNumeric = Regex.IsMatch(s, AlphaNumericRegexPattern, RegexOptions.Compiled);
            if (isAlphaNumeric)
            {
                return s;
            }

            bool isNumeral = Regex.IsMatch(s, NumeralRegexPattern, RegexOptions.Compiled);
            if (isNumeral)
            {
                return s;
            }

            bool isQuoted = IsProperlyQuoted(s);
            if (isQuoted)
            {
                return s;
            }

            StringBuilder eb = new StringBuilder();
            eb.Append('"');
            foreach (char c in s)
            {
                if (c == '"')
                {
                    eb.Append('\\');
                }

                eb.Append(c);
            }

            eb.Append('"');
            return eb.ToString();
        }

        public string GetRecordFromAttribute(IAttribute attribute)
        {
            string escapedValue = EscapeId(attribute.StringValue);
            string record = $"{attribute.Key}={escapedValue}";
            return record;
        }

        public bool IsProperlyQuoted(string s)
        {
            if (s.Length < 2)
            {
                return false;
            }

            // Should start and end with quotes.
            if (!(s[0] == '"' && s[s.Length - 1] == '"'))
            {
                return false;
            }

            // Last shouldn't be escaped.
            if (s[s.Length - 2] == '\\')
            {
                return false;
            }

            // Each one inside is escaped.
            for (int i = 1; i < s.Length - 1; i++)
            {
                if (s[i] != '"')
                {
                    continue;
                }

                // Is it escaped?
                if (s[i - 1] != '\\')
                {
                    return false;
                }
            }

            return true;
        }
    }
}