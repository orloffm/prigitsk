namespace Prigitsk.Core.Entities
{
    /// <summary>
    ///     Represents a remote branch.
    /// </summary>
    public class Branch : Pointer, IBranch
    {
        public Branch(string name, IHash tip) : base(name, tip)
        {
            int indexOfSlash = name.IndexOf('/');
            RemoteName = name.Substring(0, indexOfSlash);
            Label = name.Substring(indexOfSlash + 1);
        }

        public string RemoteName { get; }

        public string Label { get; }
    }
}