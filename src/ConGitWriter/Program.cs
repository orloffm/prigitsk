using Autofac;
using log4net.Config;

namespace ConGitWriter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ConfigureLog4Net();

            ContainerBuilder builder = new ContainerBuilder();
            IContainer container = builder.Build();

        }

        private static void ConfigureLog4Net()
        {
            XmlConfigurator.Configure();
        }
        
    }
}