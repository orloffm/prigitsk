using System.Reflection;
using Autofac;
using Prigitsk.Console.General;
using Prigitsk.Console.Verbs;
using Prigitsk.Console.Verbs.Configure;
using Prigitsk.Console.Verbs.Draw;
using Prigitsk.Console.Verbs.Fetch;
using Prigitsk.Core.Nodes.Loading;

namespace Prigitsk.Console
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule<NLoggerModule>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(NodeLoader).Assembly).AsImplementedInterfaces();
            builder.RegisterType<ConfigureRunnerFactory>().Keyed<IVerbRunnerFactory>(Verb.Configure);
            builder.RegisterType<DrawRunnerFactory>().Keyed<IVerbRunnerFactory>(Verb.Draw);
            builder.RegisterType<FetchRunnerFactory>().Keyed<IVerbRunnerFactory>(Verb.Fetch);
            IContainer container = builder.Build();

            IGeneralExecutor exec = container.Resolve<IGeneralExecutor>();
            int exitCode = exec.RunSafe(args);

            return exitCode;
        }
    }
}