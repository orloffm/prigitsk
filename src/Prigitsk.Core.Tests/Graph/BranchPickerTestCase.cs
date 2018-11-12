namespace Prigitsk.Core.Tests.Graph
{
    public sealed class BranchPickerTestCase
    {
        public string[] ExcludeRegices { get; set; }

        public string[] ExpectedPicked { get; set; }

        public string[] IncludeRegices { get; set; }

        public string[] Input { get; set; }
    }
}