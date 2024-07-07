using Autofac;
namespace FluentFactories.Autofac
{
    public static class AutofacFluentFactoriesExtensions
    {
        public static ChainOfResponsibilityBuilder<IChaintInterfaceBase> RegisterTransientChain<IChaintInterfaceBase>(this global::Autofac.ContainerBuilder container)
        {
            var module = new ChainOfResponsibilityBuilder<IChaintInterfaceBase>();
            container.RegisterModule(module);
            return module;
        }
    }
}
