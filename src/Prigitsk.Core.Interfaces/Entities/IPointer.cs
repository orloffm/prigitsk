namespace Prigitsk.Core.Entities
{
    public interface IPointer
    {
        IHash Tip { get; }

        /// <summary>
        /// Full name of the pointer.
        /// </summary>
        string Name { get; }
    }
}