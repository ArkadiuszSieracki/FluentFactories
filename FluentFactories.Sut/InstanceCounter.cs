using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentFactories.Sut
{
    public interface IInstanceCounter
    {
        void Confirm(object instance);
        int GetCount(Type type);
    }
    public class InstanceCounter : IInstanceCounter
    {
        List<object> instances = new List<object>();
        public void Confirm(object instance)
        {
            instances.Add(instance);
        }

        public int GetCount(Type type)
        {
            return instances.Count(o=>o.GetType() == type); 
        }
    }
}
