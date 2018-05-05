namespace OrlovMikhail.GraphViz.Writing
{
    public static class NodeSepAttributeExtensions
    {
        public static IAttrSet NodeSep(this IAttrSet attrSet, double value)
        {
            NodeSepAttribute a = new NodeSepAttribute(value);
            attrSet.Add(a);
            return attrSet;
        }
    }
}