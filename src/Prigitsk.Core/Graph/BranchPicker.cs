using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Prigitsk.Core.Graph
{
    public sealed class BranchPicker : IBranchPicker
    {
        private readonly Regex[] _regices;

        public BranchPicker(IBranchPickingOptions options)
        {
            Regex[] regices = CreateRegices(options.IncludeBranchesRegices).ToArray();
            if (regices.Length > 0)
            {
                _regices = regices;
            }
        }

        public bool ShouldBePicked(string branchLabel)
        {
            if (_regices == null)
            {
                return true;
            }

            bool anyMatches = _regices.Any(r => r.IsMatch(branchLabel));
            return anyMatches;
        }

        private IEnumerable<Regex> CreateRegices(IEnumerable<string> regexStrings)
        {
            if (regexStrings == null)
            {
                yield break;
            }

            foreach (string regexString in regexStrings)
            {
                if (string.IsNullOrWhiteSpace(regexString))
                {
                    continue;
                }

                Regex r = new Regex(regexString, RegexOptions.IgnoreCase);
                yield return r;
            }
        }
    }
}