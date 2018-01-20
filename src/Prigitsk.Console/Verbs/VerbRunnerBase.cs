using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Prigitsk.Console.Verbs
{
    public abstract class VerbRunnerBase<T> : IVerbRunner<T> where T : IVerbRunnerOptions
    {
        protected  T Options { get; private set; }
        protected ILogger Log { get; private set; }

        protected VerbRunnerBase(T options, ILogger log)
        {
            Options = options;
            Log = log;
        }

        public abstract void Run();
    }
}
