using Autofac;
using log4net.Config;
using OrlovMikhail.GitTools.Helpers;
using OrlovMikhail.GitTools.Loading;
using OrlovMikhail.GitTools.Loading.Client.Common;
using OrlovMikhail.GitTools.Loading.Client.Repository;

namespace ConGitWriter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ConfigureLog4Net();

            ContainerBuilder builder = new ContainerBuilder();
            RegisterClasses(builder);
            IContainer container = builder.Build();

            IWorker w = container.Resolve<IWorker>();
            w.Run(args);
        }

        private static void ConfigureLog4Net()
        {
            XmlConfigurator.Configure();
        }

        private static void RegisterClasses(ContainerBuilder builder)
        {
            ConGitWriterSettingsWrapper settingsWrapper = new ConGitWriterSettingsWrapper(Settings.Default);

            builder.RegisterInstance(settingsWrapper).As<IConGitWriterSettingsWrapper>();
            builder.RegisterType<SettingsHelper>().As<ISettingsHelper>();
            builder.RegisterType<Worker>().As<IWorker>();
            builder.RegisterType<ConsoleArgumentsHelper>().As<IConsoleArgumentsHelper>();
            builder.RegisterType<RepositoryDataBuilderFactory>().As<IRepositoryDataBuilderFactory>();
            builder.RegisterType<LibGit2ClientFactory>().As<IGitClientFactory>();
        }
    }
}