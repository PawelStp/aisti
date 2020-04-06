using Autofac;

namespace SPA.Pkb
{
    public static class Initialization
    {
        public static IContainer IoCContainer;

        public static Pkb PkbReference;

        public static void StartIoC()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<PkbModule>();
            IoCContainer = builder.Build();
        }
    }
}