namespace Prigitsk.Console.Verbs.Draw
{
    public interface IDrawRunnerOptions
        : IVerbRunnerOptions
    {
        string Repository { get; }
        string Target { get; }
        string Output { get; }
        string Format { get; }
    }
}