using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using OrlovMikhail.GitTools.Helpers;

namespace ConGitWriter
{
    class Program
    {

        static void Main(string[] args)
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
            log4net.Config.XmlConfigurator.Configure();
        }

        private static void RegisterClasses(ContainerBuilder builder)
        {
            ConGitWriterSettingsWrapper settingsWrapper = new ConGitWriterSettingsWrapper(Settings.Default);

            builder.RegisterInstance(settingsWrapper).As<IConGitWriterSettingsWrapper>();
            builder.RegisterType<SettingsHelper>().As<ISettingsHelper>();
            builder.RegisterType<Worker>().As<IWorker>();
            builder.RegisterType<ConsoleArgumentsHelper>().As<IConsoleArgumentsHelper>();
        }
    }
}
