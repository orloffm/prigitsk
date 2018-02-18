namespace Prigitsk.Core.Entities
{
    public class Tag
        : Pointer, ITag
    {
        public Tag(string name, IHash tip) : base(name, tip)
        {
        }
    }
}