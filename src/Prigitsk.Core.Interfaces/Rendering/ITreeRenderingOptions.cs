namespace Prigitsk.Core.Rendering
{
    public interface ITreeRenderingOptions
    {
        /// <summary>
        ///     Whether the repository is a GitHub one. (Maybe on a custom domain.)
        /// </summary>
        bool ForceTreatAsGitHub { get; }
    }
}