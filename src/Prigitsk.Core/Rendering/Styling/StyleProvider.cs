using System.Drawing;
using OrlovMikhail.GraphViz.Writing;

namespace Prigitsk.Core.Rendering.Styling
{
    public sealed class StyleProvider : IStyleProvider
    {
        public IAttrSet EdgeBranchLabel => GetBasePointerStyle();

        public IAttrSet EdgeBranchStartVirtual => GetBasePointerStyle();

        public IAttrSet EdgeGeneric => AttrSet.Empty
            .ArrowHead(ArrowType.Vee)
            .PenWidth(1)
            .Color("#e4e4e4");

        public IAttrSet EdgeMergedCommits => AttrSet.Empty.Style(Style.Dashed);

        public IAttrSet EdgeOther => AttrSet.Empty;

        public IAttrSet EdgeTagLabel => GetBasePointerStyle().Len(0.3m);

        public IAttrSet GraphGeneric => AttrSet.Empty
            .Rankdir(Rankdir.LR)
            .NodeSep(0.2m)
            .RankSep(0.2m)
            .ForceLabels(false);

        public IAttrSet LabelBranch => AttrSet.Empty
            .FixedSize(false)
            .PenWidth(0)
            .FillColor(GraphVizColor.None)
            .Shape(Shape.None)
            .Width(0)
            .Height(0)
            .Margin(0.05m);

        public IAttrSet LabelTag => AttrSet.Empty
            .FixedSize(false)
            .PenWidth(0.2m)
            .FillColor("#C6C6C6")
            .Shape(Shape.Cds)
            .Width(0)
            .Height(0)
        // .Margin(0.11m, 0.055m)
        ;

        public IAttrSet NodeGeneric => AttrSet.Empty
            .Label(string.Empty)
            .FontName("Consolas")
            .FontSize(8)
            .Style(Style.Filled);

        public IAttrSet NodeOrphaned => AttrSet.Empty
            .Group("")
            .FillColor(Color.White)
            .Color(Color.Black);

        public IAttrSet GetBranchEdgeStyle(Color drawColor, bool isLesser)
        {
            return AttrSet.Empty
                .PenWidth(isLesser ? 1 : 2)
                .Color(drawColor);
        }

        public IAttrSet GetBranchNodeStyle(Color drawColor, bool isLesser)
        {
            return AttrSet.Empty
                .Width(isLesser ? 0.1m : 0.2m)
                .Height(isLesser ? 0.1m : 0.2m)
                .FixedSize(true)
                .Margin(0.11m, 0.055m)
                .Shape(Shape.Circle)
                .PenWidth(isLesser ? 1 : 2)
                .FillColor(drawColor)
                .Color(drawColor);
        }

        private IAttrSet GetBasePointerStyle()
        {
            return AttrSet.Empty
                .Style(Style.Dotted)
                .ArrowHead(ArrowType.None)
                .PenWidth(1);
        }
    }
}