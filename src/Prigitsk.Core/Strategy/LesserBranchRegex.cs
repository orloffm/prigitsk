namespace Prigitsk.Core.Strategy
{
    public sealed class LesserBranchRegex : ILesserBranchRegex
    {
        public LesserBranchRegex(string regex)
        {
            RegexString = regex;
        }

        public string RegexString { get; }
    }
}