namespace Prigitsk.Console.Verbs
{
    public interface IVerbRunnerFactory
    {
        /// <summary>
        ///     Creates a runner instance.
        /// </summary>
        /// <param name="verbOptions">Runner options.</param>
        IVerbRunner Create(IVerbRunnerOptions verbOptions);
    }
}