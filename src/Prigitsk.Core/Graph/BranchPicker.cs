using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Prigitsk.Core.Graph
{
    public sealed class BranchPicker : IBranchPicker
    {
        private readonly Regex[] _excludeRegices;
        private readonly Regex[] _includeRegices;

        public BranchPicker(IBranchPickingOptions options)
        {
            _excludeRegices = CreateRegices(options.ExcludeBranchesRegices);
            _includeRegices = CreateRegices(options.IncludeBranchesRegices);
        }

        public bool ShouldBePicked(string branchLabel)
        {
            if (_includeRegices != null)
            {
                bool isExplicitInclude = _includeRegices.Any(r => r.IsMatch(branchLabel));
                return isExplicitInclude;
            }

            if (_excludeRegices != null)
            {
                bool isExplicitExclude = _excludeRegices.Any(r => r.IsMatch(branchLabel));
                return !isExplicitExclude;
            }

            return true;
        }

        private Regex[] CreateRegices(IEnumerable<string> regexStrings)
        {
            if (regexStrings == null)
            {
                return null;
            }

            var regices = new List<Regex>();
            foreach (string regexString in regexStrings)
            {
                if (string.IsNullOrWhiteSpace(regexString))
                {
                    continue;
                }

                Regex r = new Regex(regexString, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                regices.Add(r);
            }

            if (regices.Count == 0)
            {
                return null;
            }

            return regices.ToArray();
        }
    }
}