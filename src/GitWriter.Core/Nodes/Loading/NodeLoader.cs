using System;
using System.Linq;
using System.Text.RegularExpressions;
using GitWriter.Abstractions;

namespace GitWriter.Core.Nodes.Loading
{
    public class NodeLoader : INodeLoader
    {
        private const string GitPath = @"C:\Program Files\Git\bin\git.exe";

        private const string InsDelRegexString =
            @"changed(?:, (?<ins>\d+) insertions\(\+\))?(?:, (?<del>\d+) deletions\(\-\))?";

        private readonly IProcessRunner _processRunner;

        private readonly Regex InsDelRegex;
        private string _result;

        public NodeLoader(IProcessRunner processRunner)
        {
            _processRunner = processRunner;
            InsDelRegex = new Regex(InsDelRegexString);
        }

        public void LoadFrom(
            string gitSubDirectory,
            ExtractionOptions extractOptions)
        {
            string extractionCommand = @"--git-dir ""{0}"" log --dense --all --format=format:""%h|%p|%d|%at""";
            if (extractOptions.ExtractStats)
            {
                extractionCommand += " --shortstat";
            }
            // const string extractionCommand = @"—git-dir ""{0
            // —dense —all —format=format:,,,,%h|%p|%d,,",,;
            string gitCommand = string.Format(extractionCommand, gitSubDirectory);
            _result = _processRunner.Execute(GitPath, gitCommand);
            //	_result = @"A|||1
            //B|A|origin/master|2
            //C|A||3
            //D|C B||4
            //E|D|origin/develop|5";
        }

        public Node[] GetNodesCollection()
        {
            INodeManager nm = new NodeManager();
            string[] lines = _result.Split('\n');
            for (int index = 0; index < lines.Length;)
            {
                string lineMain = lines[index];
                // Skip empty line.
                if (string.IsNullOrWhiteSpace(lineMain))
                {
                    index++;
                    continue;
                }
                // There may be no stat line,
                string lineStat = null;
                if (index < lines.Length - 1)
                {
                    lineStat = lines[index + 1];
                }

                if (lineStat != null && !lineStat.StartsWith(" "))
                {
                    lineStat = null;
                }
                string[] parents;
                string caption;
                long time;
                int insertions;
                int deletions;
                string hash;
                ExtractValuesFromLine(
                    lineMain,
                    lineStat,
                    out hash,
                    out parents,
                    out caption,
                    out time,
                    out insertions,
                    out deletions);
                nm.SetData(hash, caption, time, insertions, deletions);
                foreach (string parent in parents)
                {
                    nm.AddChildren(parent, hash);
                }
                // Go to next,
                index++;
                // If there was a stat line, we also skip it.
                if (lineStat != null)
                {
                    index++;
                }
            }
            return nm.EnumerateNodes().ToArray();
        }

        private void ExtractValuesFromLine(
            string lineMain,
            string lineStat,
            out string hash,
            out string[] parents,
            out string caption,
            out long time,
            out int insertions,
            out int deletions)
        {
            string[] cells = lineMain.Split('|');
            hash = cells[0];
            parents = cells[1].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            caption = cells[2];
            time = long.Parse(cells[3]);
            if (lineStat != null)
            {
                Match m = InsDelRegex.Match(lineStat);
                string insString = m.Groups["ins"].Value;
                string delString = m.Groups["del"].Value;
                int.TryParse(insString, out insertions);
                int.TryParse(delString, out deletions);
            }
            else
            {
                insertions = 0;
                deletions = 0;
            }
        }
    }
}