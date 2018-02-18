namespace Prigitsk.Core.Entities
{
    public abstract class Pointer : IPointer
    {
        protected Pointer(string name, IHash tip)
        {
            Tip = tip;
            Name = name;
        }

        public IHash Tip { get; }
        public string Name { get; }
    }
}