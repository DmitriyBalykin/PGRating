using Autofac;
using PGRating.Crawler.Loader;

namespace PGRating.Autofac
{
    public class AutofacBootstrap
    {
        public static IContainer Container { get; private set; }

        public static void Initialize()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<WebLoader>().As<ILoader>();
            Container = builder.Build();
        }
    }
}