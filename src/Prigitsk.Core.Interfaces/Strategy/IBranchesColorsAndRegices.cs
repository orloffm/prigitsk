using System.Collections.Generic;
using System.Drawing;

namespace Prigitsk.Core.Strategy
{
    public interface IBranchesColorsAndRegices
    {
        BranchLogicalType[] GetAllBranchLogicalTypesOrdered();
        Color GetColor(BranchLogicalType ft);
        bool TryGetRegexStringsInternal(BranchLogicalType flowType, out ISet<string> regexStrings);
    }
}