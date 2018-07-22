namespace OrlovMikhail.GraphViz.Writing
{
    public static class NodeSepAttributeExtensions
    {
        public static IAttrSet NodeSep(this IAttrSet attrSet, decimal value)
        {
            NodeSepAttribute a = new NodeSepAttribute(value);
            return attrSet.Add(a);
        }
    }
}