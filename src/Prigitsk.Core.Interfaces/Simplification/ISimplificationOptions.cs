namespace Prigitsk.Core.Simplification
{
    public interface ISimplificationOptions
    {
        bool FirstBranchNodeMayBeRemoved { get; }

        bool KeepAllOrphans { get; }

        bool LeaveTails { get; }
    }
}