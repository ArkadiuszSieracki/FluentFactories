using Autofac;
using Autofac.Features.Indexed;
using System;
using System.Collections.Generic;
using System.Linq;
namespace FluentFactories.Autofac
{
    public class ChainOfResponsibilityBuilder<TChain>:Module
    {
        public string Id { get; set; } =/* Guid.NewGuid().ToString();*/ string.Empty;
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
                builder.RegisterType(item);
                
            }
            _ = builder.Register((ctx) =>
            {
               
                foreach (var item in def)
                {
                    current = ctx.Resolve(item, new TypedParameter(typeof(TChain), current));
                }
                
                return current;

            }).As<TChain>();
            //for ( ; index < def.Count; index++)
            //{
            //    builder.RegisterType(def[index]).Named(Id + index, def[index]);
            //    int indexC = index;
            //    _ = builder.Register((ctx) =>
            //    {
            //        var ctbr = def[indexC - 1];
            //        current = ctx.ResolveNamed(Id + (indexC-1).ToString(), ctbr, new TypedParameter(typeof(TChain), current));
            //        return current;
            //    }).Named<TChain>(Id + index);
            //}
            //builder.Register((ctx) =>
            //{
            //    TChain item = ctx.ResolveNamed<TChain>(Id + (index - 1).ToString());
            //    return item;
            //}).As<TChain>();

        }
    }
}
