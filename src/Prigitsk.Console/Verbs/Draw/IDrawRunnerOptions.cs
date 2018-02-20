namespace Prigitsk.Console.Verbs.Draw
{
    public interface IDrawRunnerOptions
        : IVerbRunnerOptions
    {
        string Format { get; }

        string Output { get; }

        string Repository { get; }

        string Target { get; }
    }
}