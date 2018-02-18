﻿using System.IO.Abstractions;
using System.Reflection;
using Autofac;
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

            IGeneralExecutor exec = container.Resolve<IGeneralExecutor>();
            int exitCode = exec.RunSafe(args);

            return exitCode;
        }

        private static IContainer PrepareContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            // external
            builder.RegisterModule<NLoggerModule>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            builder.RegisterInstance(AppSettings.Default).AsSelf();

            // converters
            builder.RegisterType<ConfigureRunnerFactory>().Keyed<IVerbRunnerFactory>(Verb.Configure)
                .InstancePerLifetimeScope();
            builder.RegisterType<DrawRunnerFactory>().Keyed<IVerbRunnerFactory>(Verb.Draw).InstancePerLifetimeScope();
            builder.RegisterType<FetchRunnerFactory>().Keyed<IVerbRunnerFactory>(Verb.Fetch).InstancePerLifetimeScope();

            builder.RegisterType<ConfigureVerbOptionsConverter>().Keyed<IVerbOptionsConverter>(Verb.Configure)
                .InstancePerLifetimeScope();
            builder.RegisterType<DrawVerbOptionsConverter>().Keyed<IVerbOptionsConverter>(Verb.Draw)
                .InstancePerLifetimeScope();
            builder.RegisterType<FetchVerbOptionsConverter>().Keyed<IVerbOptionsConverter>(Verb.Fetch)
                .InstancePerLifetimeScope();

            // core assembly
            builder.RegisterAssemblyTypes(typeof(BranchWrapped).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(RepositoryData).Assembly).AsImplementedInterfaces();

            IContainer container = builder.Build();
            return container;
        }
    }
}