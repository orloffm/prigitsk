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

        /// <summary>
        ///     Logically separated to have a common place to prepare things before execution.
        /// </summary>
        protected virtual void Initialise()
        {
        }

        public void Run()
        {
            Log.Debug($"Initialising {GetType().Name}...");

            Initialise();

            Log.Debug("Running.");

            RunInternal();

            Log.Debug("Successfully completed run.");
        }

        /// <summary>
        ///     The actual execution happens here.
        /// </summary>
        protected abstract void RunInternal();
    }
}