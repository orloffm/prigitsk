using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Verbs
{
    public abstract class VerbRunnerBase<T> : IVerbRunner<T> where T : IVerbRunnerOptions
    {
        protected VerbRunnerBase(T options, ILogger log)
        {
            Options = options;
            Log = log;
        }

        protected ILogger Log { get; }

        protected T Options { get; }

        public void Run()
        {
            Log.Info($"Running {GetType().Name}...");

            RunInternal();

            Log.Trace("Successfully completed run.");
        }

        protected abstract void RunInternal();
    }
}