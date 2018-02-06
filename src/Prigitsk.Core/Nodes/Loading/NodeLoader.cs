using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Prigitsk.Core.Tools;

namespace Prigitsk.Core.Nodes.Loading
{
    public class NodeLoader : INodeLoader
    {
        private const string InsDelRegexString =
            @"changed(?:, (?<ins>\d+) insertions\(\+\))?(?:, (?<del>\d+) deletions\(\-\))?";

        private readonly IProcessRunner _processRunner;
        private readonly INodeKeeperFactory _nodeKeeperFactory;
        private readonly string _gitPath;

        private readonly Regex _insDelRegex;
        private string _result;

        public NodeLoader(IProcessRunner processRunner, INodeKeeperFactory nodeKeeperFactory, string gitPath)
        {
            _processRunner = processRunner;
            _nodeKeeperFactory = nodeKeeperFactory;
            _gitPath = gitPath;
            _insDelRegex = new Regex(InsDelRegexString);
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
            _result = _processRunner.Execute(_gitPath, gitCommand);
            //	_result = @"A|||1
            //B|A|origin/master|2
            //C|A||3
            //D|C B||4
            //E|D|origin/develop|5";
        }

        public IEnumerable<INode> GetNodesCollection()
        {
            INodeKeeper nm = _nodeKeeperFactory.CreateKeeper();
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

                ExtractValuesFromLine(
                    lineMain,
                    lineStat,
                    out string hash,
                    out string[] parents,
                    out string caption,
                    out long time,
                    out int insertions,
                    out int deletions);
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

            return nm.EnumerateNodes();
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
                Match m = _insDelRegex.Match(lineStat);
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