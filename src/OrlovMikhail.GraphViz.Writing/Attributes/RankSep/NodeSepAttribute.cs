namespace OrlovMikhail.GraphViz.Writing
{
    public class NodeSepAttribute : DoubleAttribute
    {
        public NodeSepAttribute(decimal value) : base(value)
        {
        }
        public override string Key => "nodeSep";

    }
}