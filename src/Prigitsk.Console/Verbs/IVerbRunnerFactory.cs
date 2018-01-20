namespace Prigitsk.Console.Verbs
{
    public interface IVerbRunnerFactory
    {
        /// <summary>
        ///     Creates a runner instance.
        /// </summary>
        /// <param name="parseResultVerbOptions">Runner options.</param>
        IVerbRunner Create(object parseResultVerbOptions);
    }
}