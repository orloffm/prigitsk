using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Verbs
{
    public abstract class VerbRunnerFactoryBase<TOptions> : IVerbRunnerFactory
        where TOptions : class, IVerbRunnerOptions
    {
        protected VerbRunnerFactoryBase(ILogger log)
        {
            Log = log;
        }

        protected ILogger Log { get; }

        public IVerbRunner Create(object verbOptions)
        {
            TOptions optionsTyped = verbOptions as TOptions;
            if (optionsTyped == null)
            {
                Log.Warn(
                    "No correct options of type {0} provided when generating a runner.",
                    typeof(TOptions).Name);
            }

            return CreateInternal(optionsTyped);
        }

        protected abstract IVerbRunner<TOptions> CreateInternal(TOptions options);
    }
}