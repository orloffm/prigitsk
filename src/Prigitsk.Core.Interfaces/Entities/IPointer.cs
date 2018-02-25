namespace Prigitsk.Core.Entities
{
    public interface IPointer
    {
        /// <summary>
        ///     Short name. For branches - the name without the remote part.
        /// </summary>
        string Label { get; }

        /// <summary>
        ///     Full name of the pointer.
        /// </summary>
        string FullName { get; }

        IHash Tip { get; }
    }
}