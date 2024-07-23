using Autofac;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FluentFactories.Autofac
{
    public class Flyweight<T> : Module, IFlyWeightContext
    {
        public Flyweight()
        {
            getType = new Func<Type>(() => { return typeof(T); });
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        internal List<Action<ContainerBuilder>> actions = new List<Action<ContainerBuilder>>();
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterGeneric(typeof(FlyWeightFactory<>)).As(typeof(IFlyWeightFactory<>));
            builder.RegisterType(typeof(T)).As(getType());
            builder.Register((ctx) => this).Named<IFlyWeightContext>(typeof(T).Name);
        }
        Func<Type> getType;
        public Flyweight<T> FLyweightAs<IImplemented>()
        {
            getType = new Func<Type>(() => typeof(IImplemented));
            return this;
        }
        private Type Type { get; set; }
        public Flyweight<T> WithShared<TRegister, TI>() where TRegister : TI
        {
            actions.Add(new Action<ContainerBuilder>((cb) =>
            {
                _ = cb.RegisterType<TRegister>().As<TI>().InstancePerLifetimeScope();
            }));
            return this;
        }
        public Flyweight<T> WithShared<TRegister>()
        {
            actions.Add(new Action<ContainerBuilder>((cb) =>
            {
                _ = cb.RegisterType<TRegister>().As<TRegister>().InstancePerLifetimeScope();
            }));
            return this;
        }

        public List<Action<ContainerBuilder>> GetActions()
        {
            return actions;
        }
    }
    public class FlyWeightFactory<TValue> : IDisposable, IFlyWeightFactory<TValue>
    {
        private readonly ILifetimeScope context;
        private readonly IFlyWeightContext ctx;
        Dictionary<object, ILifetimeScope> lookup = new Dictionary<object, ILifetimeScope>();
        private bool disposedValue;

        public FlyWeightFactory(ILifetimeScope context)
        {
            this.context = context;
            this.ctx = context.ResolveNamed<IFlyWeightContext>(typeof(TValue).Name); ;
        }

        public TValue Get(object key)
        {
            if (lookup.ContainsKey(key) == false)
            {
                lookup[key] = context.BeginLifetimeScope((cb) =>
                {
                    foreach (var item in ctx.GetActions())
                    {
                        item.Invoke(cb);
                    };
                });
            }
            return lookup[key].Resolve<TValue>();
        }

        public TValue Get<TKey>(TKey key)
        {
            if (lookup.ContainsKey(key) == false)
            {
                lookup[key] = context.BeginLifetimeScope((cb) =>
                {
                    foreach (var item in ctx.GetActions())
                    {
                        item.Invoke(cb);
                    };
                });
            }
            return lookup[key].Resolve<TValue>(new TypedParameter(typeof(TKey), key));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var item in lookup)
                    {
                        item.Value.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }



        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
