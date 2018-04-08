namespace Prigitsk.Console.Verbs.Draw
{
    public class DrawRunnerOptions : IDrawRunnerOptions
    {
        public DrawRunnerOptions(
            string repository = null,
            string target = null,
            string output = null,
            string format = null,
            string remoteToUse = null)
        {
            Repository = repository;
            TargetDirectory = target;
            OutputFileName = output;
            Format = format;
            RemoteToUse = remoteToUse;
        }

        public string Format { get; }

        public string OutputFileName { get; }

        public string RemoteToUse { get; }

        public string Repository { get; }

        public string TargetDirectory { get; }
    }
}