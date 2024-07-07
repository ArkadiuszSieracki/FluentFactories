using Autofac;
using System;
using System.Collections.Generic;
namespace FluentFactories.Autofac
{
    public class ChainOfResponsibilityBuilder<TChain>:Module
    {
        private List<Type> ChainOfResponsibilityDef { get;} = new List<Type>();
   
        public ChainOfResponsibilityBuilder<TChain> WithNext<TConcreteChain>() where TConcreteChain:TChain {
            ChainOfResponsibilityDef.Add(typeof(TConcreteChain));
            return this;
        }
    }
}
