using Autofac;

namespace SPA.Pkb
{
    public class PkbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<Pkb>()
                .As<IPkb>()
                .SingleInstance();
        }
    }
}