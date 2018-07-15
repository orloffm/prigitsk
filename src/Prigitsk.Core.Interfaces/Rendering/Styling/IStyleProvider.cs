using System.Drawing;
using OrlovMikhail.GraphViz.Writing;

namespace Prigitsk.Core.Rendering.Styling
{
    public interface IStyleProvider
    {
        IAttrSet EdgeBranchLabel { get; }

        IAttrSet EdgeBranchStartVirtual { get; }

        IAttrSet EdgeMergedCommits { get; }

        IAttrSet EdgeOther { get; }

        IAttrSet EdgeTagLabel { get; }

        IAttrSet Graph { get; }

        IAttrSet LabelBranch { get; }

        IAttrSet LabelTag { get; }

        IAttrSet NodeGeneric { get; }

        IAttrSet NodeOrphaned { get; }

        IAttrSet GetBranchEdgeStyle(Color drawColor, bool isLesser);

        IAttrSet GetBranchNodeStyle(Color drawColor, bool isLesser);
    }
}