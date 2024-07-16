using Autofac;
using Autofac.Core;
using Autofac.Features.Indexed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;
namespace FluentFactories.Autofac
{
    public class ChainOfResponsibilityBuilder<TChain> : Module
    {
        private ResgisterKind targetRegisterKind;

        class Registeration
        {
            public Registeration(Type type, ResgisterKind registerKind)
            {
                Type = type;
            }

            public Type Type { get; set; }
            public ResgisterKind ResgisterKind { get; set; }
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();

        private List<Registeration> ChainOfResponsibilityDef { get; } = new List<Registeration>();

        public ChainOfResponsibilityBuilder<TChain> WithNext<TConcreteChain>() where TConcreteChain : TChain
        {
            ChainOfResponsibilityDef.Add(new Registeration(typeof(TConcreteChain), ResgisterKind.InstancePerDependency));
            return this;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            switch (targetRegisterKind) {
                case ResgisterKind.InstancePerDependency:
                    GetChainRegisteration(builder).InstancePerDependency();
                    break;
                case ResgisterKind.InstancePerLifetimeScope:
                    GetChainRegisteration(builder).InstancePerLifetimeScope();
                    break;
                case ResgisterKind.SingleInstance:
                    GetChainRegisteration(builder).SingleInstance(); 
                    break;
            }

        }
        private global::Autofac.Builder.IRegistrationBuilder<object?, global::Autofac.Builder.SimpleActivatorData, global::Autofac.Builder.SingleRegistrationStyle> GetChainRegisteration(ContainerBuilder builder)
        {
            int index = 0;
            var def = ChainOfResponsibilityDef;
            def.Reverse();
            var first = def.First();
            var b = builder.RegisterType(first.Type).Named(Id + index.ToString(), first.Type);
            object current = default(TChain);
            foreach (var item in def)
            {
                switch (item.ResgisterKind)
                {
                    case ResgisterKind.InstancePerDependency:
                        builder.RegisterType(item.Type).Named(Id + index++, item.Type); break;
                    case ResgisterKind.InstancePerLifetimeScope:
                        builder.RegisterType(item.Type).Named(Id + index++, item.Type).InstancePerLifetimeScope(); break;
                    case ResgisterKind.SingleInstance:
                        builder.RegisterType(item.Type).Named(Id + index++, item.Type).SingleInstance(); break;
                }

            }
            var res = builder.Register((ctx) =>
               {
                   index = 0;
                   foreach (var item in def)
                   {
                       current = ctx.ResolveNamed(Id + index++, item.Type, new TypedParameter(typeof(TChain), current));
                   }

                   return current;

               }).As<TChain>();
            return res;
        }

        public ChainOfResponsibilityBuilder<TChain> InstancePerDependency()
        {
            UpdateLastRegisteration(ResgisterKind.InstancePerDependency);
            return this;
        }
        private void UpdateLastRegisteration(ResgisterKind kind)
        {
            var lastInChain = this.ChainOfResponsibilityDef.LastOrDefault();
            if (lastInChain is null)
            {
                targetRegisterKind = kind;
            }
            else
            {
                lastInChain.ResgisterKind = kind;
            }
        }
        public ChainOfResponsibilityBuilder<TChain> InstancePerLifetieScope()
        {
            UpdateLastRegisteration(ResgisterKind.InstancePerLifetimeScope);
            return this;
        }

        public ChainOfResponsibilityBuilder<TChain> SingleInstance()
        {
            UpdateLastRegisteration(ResgisterKind.SingleInstance);
            return this;
        }
    }

    internal enum ResgisterKind
    {
        InstancePerDependency,
        InstancePerLifetimeScope,
        SingleInstance
    }
}
