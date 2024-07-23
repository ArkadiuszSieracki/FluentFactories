using Autofac;
namespace FluentFactories.Autofac
{
    public static class AutofacFluentFactoriesExtensions
    {
        public static ChainOfResponsibilityBuilder<IChaintInterfaceBase> RegisterChain<IChaintInterfaceBase>(this global::Autofac.ContainerBuilder container)
        {
            var module = new ChainOfResponsibilityBuilder<IChaintInterfaceBase>();
            container.RegisterModule(module);
            return module;
        }

        public static Flyweight<TFlyweight> RegisterFlyWeight<TFlyweight>(this global::Autofac.ContainerBuilder container)
        {
            var module = new Flyweight<TFlyweight>();
            container.RegisterModule(module);
            return module;
        }
    }
}
