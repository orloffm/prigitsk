namespace Prigitsk.Console.Verbs
{
    public interface IVerbRunnerFactory
    {
        IVerbRunner CreateRunner(Verb verb, object verbOptions);
    }
}