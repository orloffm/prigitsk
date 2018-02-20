namespace Prigitsk.Console.Verbs.Draw
{
    public class DrawRunnerOptions : IDrawRunnerOptions
    {
        public DrawRunnerOptions(
            string repository = null,
            string target = null,
            string output = null,
            string format = null)
        {
            Repository = repository;
            Target = target;
            Output = output;
            Format = format;
        }

        public string Format { get; }

        public string Output { get; }

        public string Repository { get; }

        public string Target { get; }
    }
}