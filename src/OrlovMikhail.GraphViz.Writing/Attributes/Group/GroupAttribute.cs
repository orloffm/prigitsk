namespace OrlovMikhail.GraphViz.Writing
{
    public class GroupAttribute : StringAttribute
    {
        public GroupAttribute(string value) : base(value)
        {
        }
        public override string Key => "group";

    }
}