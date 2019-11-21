using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace hangfiretest
{
    public static class AutofacExt
    {
        private static IContainer _container;
        public static IContainer InitAutofac(IServiceCollection services, Assembly executingAssembly)
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(test)).As(typeof(Itest)).InstancePerDependency().PropertiesAutowired();
            _container = builder.Build();
            return _container;
        }
       
    }
}
