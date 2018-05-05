namespace Prigitsk.Core.Entities
{
    /// <summary>
    ///     Something that can be referenced and provide a Git tree.
    /// </summary>
    public interface ITreeish
    {
        string Treeish { get; }
    }
}