using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure
{
    public sealed class ServiceLocator : IServiceProvider
    {
        private readonly IUnityContainer container;
        private static readonly ServiceLocator instance = new ServiceLocator();

        private ServiceLocator()
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            container = new UnityContainer();
            section.Configure(container);
        }

        public static ServiceLocator Instance
        {
            get { return instance; }
        }

        private IEnumerable<ParameterOverride> GetParameterOverrides(object overridedArguments)
        {
            List<ParameterOverride> overrides = new List<ParameterOverride>();
            Type argumentsType = overridedArguments.GetType();
            argumentsType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList()
                .ForEach(property =>
                {
                    var propertyValue = property.GetValue(overridedArguments, null);
                    var propertyName = property.Name;
                    overrides.Add(new ParameterOverride(propertyName, propertyValue));
                });
            return overrides;
        }

        public T GetService<T>()
        {
            return container.Resolve<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return container.ResolveAll<T>();
        }

        public T GetService<T>(object overridedArguments)
        {
            var overrides = GetParameterOverrides(overridedArguments);
            return container.Resolve<T>(overrides.ToArray());
        }

        public object GetService(Type serviceType, object overridedArguments)
        {
            var overrides = GetParameterOverrides(overridedArguments);
            return container.Resolve(serviceType, overrides.ToArray());
        }

        public object GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

    }
}
