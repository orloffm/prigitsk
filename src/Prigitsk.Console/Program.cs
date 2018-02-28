using System.IO.Abstractions;
using System.Reflection;
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
using Prigitsk.Core.Git.LibGit2Sharp;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Tools;
using Prigitsk.Core.Graph;
using ContainerBuilder = Autofac.ContainerBuilder;
using IContainer = Autofac.IContainer;
using ModuleRegistrationExtensions = Autofac.ModuleRegistrationExtensions;
using RegistrationExtensions = Autofac.RegistrationExtensions;
using ResolutionExtensions = Autofac.ResolutionExtensions;

namespace Prigitsk.Console
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            //LogManager.LoadConfiguration("nlog.config");
            //Logger logger = LogManager.GetCurrentClassLogger();
            //logger.Info("ABC");
            IContainer container = PrepareContainer();

            IGeneralExecutor exec = ResolutionExtensions.Resolve<IGeneralExecutor>(container);
            int exitCode = exec.RunSafe(args);

            return exitCode;
        }

        private static IContainer PrepareContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // assemblies
            RegistrationExtensions.AsImplementedInterfaces(
                RegistrationExtensions.RegisterAssemblyTypes(builder, Assembly.GetExecutingAssembly()));
            RegistrationExtensions.AsImplementedInterfaces(
                RegistrationExtensions.RegisterAssemblyTypes(builder, typeof(GitBranchWrapped).Assembly));
            RegistrationExtensions.AsImplementedInterfaces(
                RegistrationExtensions.RegisterAssemblyTypes(builder, typeof(RepositoryData).Assembly));
            RegistrationExtensions.AsImplementedInterfaces(
                RegistrationExtensions.RegisterAssemblyTypes(builder, typeof(ProcessRunner).Assembly));
            RegistrationExtensions.AsImplementedInterfaces(
                RegistrationExtensions.RegisterAssemblyTypes(builder, typeof(Tree).Assembly));

            // external
            ModuleRegistrationExtensions.RegisterModule<NLoggerModule>(builder);
            RegistrationExtensions.RegisterType<FileSystem>(builder).As<IFileSystem>();

            RegistrationExtensions.AsSelf(RegistrationExtensions.RegisterInstance(builder, AppSettings.Default));

            // converters
            RegistrationExtensions.RegisterType<ConfigureRunnerFactory>(builder)
                .Keyed<IVerbRunnerFactory>(Verb.Configure)
                .InstancePerLifetimeScope();
            RegistrationExtensions.RegisterType<DrawRunnerFactory>(builder).Keyed<IVerbRunnerFactory>(Verb.Draw)
                .InstancePerLifetimeScope();
            RegistrationExtensions.RegisterType<FetchRunnerFactory>(builder).Keyed<IVerbRunnerFactory>(Verb.Fetch)
                .InstancePerLifetimeScope();

            RegistrationExtensions.RegisterType<ConfigureVerbOptionsConverter>(builder)
                .Keyed<IVerbOptionsConverter>(Verb.Configure)
                .InstancePerLifetimeScope();
            RegistrationExtensions.RegisterType<DrawVerbOptionsConverter>(builder)
                .Keyed<IVerbOptionsConverter>(Verb.Draw)
                .InstancePerLifetimeScope();
            RegistrationExtensions.RegisterType<FetchVerbOptionsConverter>(builder)
                .Keyed<IVerbOptionsConverter>(Verb.Fetch)
                .InstancePerLifetimeScope();

            IContainer container = builder.Build();
            return container;
        }
    }
}