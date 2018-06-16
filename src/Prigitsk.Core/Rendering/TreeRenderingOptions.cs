namespace Prigitsk.Core.Rendering
{
    public sealed class TreeRenderingOptions
        : ITreeRenderingOptions
    {
        public TreeRenderingOptions()
        {
            ForceTreatAsGitHub = false;
        }

        public static TreeRenderingOptions Default => new TreeRenderingOptions();

        public bool ForceTreatAsGitHub { get; set; }

        public string LesserBranchesRegex { get; set; }
    }
}