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

        protected T Options { get; }
        protected ILogger Log { get; }

        public void Run()
        {
            Log.Trace("Starting run.");

            RunInternal();

            Log.Trace("Successfully completed run.");
        }

        protected abstract void RunInternal();
    }
}