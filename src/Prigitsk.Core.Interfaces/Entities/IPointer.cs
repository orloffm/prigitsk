namespace Prigitsk.Core.Entities
{
    public interface IPointer
    {
        /// <summary>
        ///     Full name of the pointer.
        /// </summary>
        string Name { get; }

        IHash Tip { get; }
    }
}