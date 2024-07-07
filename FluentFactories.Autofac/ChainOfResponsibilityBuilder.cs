using Autofac;
using Autofac.Features.Indexed;
using System;
using System.Collections.Generic;
using System.Linq;
namespace FluentFactories.Autofac
{
    public class ChainOfResponsibilityBuilder<TChain>:Module
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        private List<Type> ChainOfResponsibilityDef { get;} = new List<Type>();
   
        public ChainOfResponsibilityBuilder<TChain> WithNext<TConcreteChain>() where TConcreteChain:TChain {
            ChainOfResponsibilityDef.Add(typeof(TConcreteChain));
            return this;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            int index = 0;
            var def = ChainOfResponsibilityDef;
            def.Reverse();
            var first = def.First();
            builder.RegisterType(first).Named(Id + index.ToString(), first);
            object current = default(TChain);
            foreach (var item in def)
            {
                builder.RegisterType(item).Named(Id+index++,item);
                
            }
            _ = builder.Register((ctx) =>
            {
                index = 0;
                foreach (var item in def)
                {
                    current = ctx.ResolveNamed(Id + index++,item, new TypedParameter(typeof(TChain), current));
                }
                
                return current;

            }).As<TChain>();
   

        }
    }
}
