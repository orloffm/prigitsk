using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Prigitsk.Core.Graph
{
    public sealed class BranchPicker : IBranchPicker
    {
        private readonly Regex[] _excludeRegices;
        private readonly Regex[] _includeRegices;
        private readonly ILogger<BranchPicker> _logger;

        public BranchPicker(IBranchPickingOptions options, ILogger<BranchPicker> logger)
        {
            _logger = logger;

            _logger.Debug("Loading exclude branches regular expressions.");
            _excludeRegices = CreateRegices(options.ExcludeBranchesRegices);
            _logger.Debug("Finished loading exclude branches regular expressions.");

            _logger.Debug("Loading include branches regular expressions.");
            _includeRegices = CreateRegices(options.IncludeBranchesRegices);
            _logger.Debug("Finished loading include branches regular expressions.");
        }

        public bool ShouldBePicked(string branchLabel)
        {
            Regex matchedBy;

            if (_includeRegices != null)
            {
                matchedBy = _includeRegices.FirstOrDefault(r => r.IsMatch(branchLabel));
                if (matchedBy != null)
                {
                    _logger.Debug(
                        "{0} - picked (matched by {1}).",
                        branchLabel,
                        matchedBy.ToString()
                    );
                    return true;
                }

                _logger.Debug(
                    "{0} - not picked (no inclusion regices match).",
                    branchLabel
                );
                return false;
            }

            if (_excludeRegices != null)
            {
                matchedBy = _excludeRegices.FirstOrDefault(r => r.IsMatch(branchLabel));
                if (matchedBy != null)
                {
                    _logger.Debug(
                        "{0} - not picked (matched by {1}; no include regices).",
                        branchLabel,
                        matchedBy.ToString()
                    );
                    return false;
                }

                _logger.Debug(
                    "{0} - picked (not exclusion regices match; no include regices).",
                    branchLabel
                );
                return true;
            }

            _logger.Debug("{0} - picked (all are).", branchLabel);
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

                _logger.Debug(" {0}", regexString);
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