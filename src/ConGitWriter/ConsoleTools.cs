using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConGitWriter
{
    public static class ConsoleTools
    {
        public static Dictionary<string, string> ArgumentsToDictionary(string[] args)
        {
            Dictionary<string, string> returnee = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Regex matcher = new Regex(@"^/(?<param>[^\r\n=:]*)(?:[:=]{1}(?<value>.*))?$", RegexOptions.Compiled);

            foreach(string arg in args)
            {
                string s = arg.Trim();
                Match m = matcher.Match(s);
                if(m.Success)
                    returnee.Add(m.Groups["param"].Value, m.Groups["value"].Value);
                else
                    throw new ArgumentException("Wrong argument: \"" + s + "\".");
            }

            return returnee;
        }
    }
}
