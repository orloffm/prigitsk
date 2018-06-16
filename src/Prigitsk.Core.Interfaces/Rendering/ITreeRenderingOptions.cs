namespace Prigitsk.Core.Rendering
{
    public interface ITreeRenderingOptions
    {
        /// <summary>
        ///     Whether the repository is a GitHub one. (Maybe on a custom domain.)
        /// </summary>
        bool ForceTreatAsGitHub { get; }

        /// <summary>
        ///     If the name of a branch consists of some other branch name and a suffix,
        ///     and this suffix matches any of these expressions, the branch is considered
        ///     to be lesser branch and is drawn differently.
        /// </summary>
        string LesserBranchesRegex { get; }
    }
}