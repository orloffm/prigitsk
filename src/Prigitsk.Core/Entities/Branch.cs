namespace Prigitsk.Core.Entities
{
    public class Branch : Pointer, IBranch
    {
        public Branch(string name, IHash tip) : base(name, tip)
        {
        }
    }
}