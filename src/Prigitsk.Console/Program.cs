using System.Reflection;
using Autofac;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using OrlovMikhail.Git.LibGit2Sharp;
using OrlovMikhail.GraphViz.Writing;
using Prigitsk.Console.CommandLine.Conversion;
using Prigitsk.Console.CommandLine.Conversion.Configure;
using Prigitsk.Console.CommandLine.Conversion.Draw;
using Prigitsk.Console.CommandLine.Conversion.Fetch;
using Prigitsk.Console.General;
using Prigitsk.Console.Properties;
using Prigitsk.Console.Verbs;
using Prigitsk.Console.Verbs.Configure;
using Prigitsk.Console.Verbs.Draw;
using Prigitsk.Console.Verbs.Fetch;
using Prigitsk.Core.Graph;
using Prigitsk.Core.RepoData;
using Prigitsk.Framework;

namespace Prigitsk.Console
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            IContainer container = PrepareContainer();

            IGeneralExecutor exec = container.Resolve<IGeneralExecutor>();
            int exitCode = exec.RunSafe(args);

            return exitCode;
        }

        private static IContainer PrepareContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Create Logger<T> when ILogger<T> is required.
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));

            // Use NLogLoggerFactory as a factory required by Logger<T>.
            builder.RegisterType<NLogLoggerFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();

            // Assemblies.
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            // Frameworks.
            builder.RegisterAssemblyTypes(typeof(GitBranchWrapped).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(GraphVizWriter).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(OrderedSet<>).Assembly).AsImplementedInterfaces();
            // Lib.
            builder.RegisterAssemblyTypes(typeof(RepositoryData).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(Tree).Assembly).AsImplementedInterfaces();

            // external
            builder.RegisterInstance(AppSettings.Default).AsSelf();

            // converters
            builder.RegisterType<ConfigureRunnerFactory>()
                .Keyed<IVerbRunnerFactory>(Verb.Configure)
                .InstancePerLifetimeScope();
            builder.RegisterType<DrawRunnerFactory>().Keyed<IVerbRunnerFactory>(Verb.Draw)
                .InstancePerLifetimeScope();
            builder.RegisterType<FetchRunnerFactory>().Keyed<IVerbRunnerFactory>(Verb.Fetch)
                .InstancePerLifetimeScope();

            builder.RegisterType<ConfigureVerbOptionsConverter>()
                .Keyed<IVerbOptionsConverter>(Verb.Configure)
                .InstancePerLifetimeScope();
            builder.RegisterType<DrawVerbOptionsConverter>()
                .Keyed<IVerbOptionsConverter>(Verb.Draw)
                .InstancePerLifetimeScope();
            builder.RegisterType<FetchVerbOptionsConverter>()
                .Keyed<IVerbOptionsConverter>(Verb.Fetch)
                .InstancePerLifetimeScope();

            IContainer container = builder.Build();
            return container;
        }
    }
}