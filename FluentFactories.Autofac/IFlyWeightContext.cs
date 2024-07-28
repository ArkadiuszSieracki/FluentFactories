using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace FluentFactories.Autofac
{
    internal interface IFlyWeightContext
    {
        List<Action<ContainerBuilder>> GetActions();
    }
}