namespace SharpSerializer.AutofacInstanceCreator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;
    using Autofac;
    using Polenter.Serialization.Core;
    public class AutofacInstanceCreator : IInstanceCreator
    {
        private MemoryCache Cache { get; set; }

        private ILifetimeScope Container { get; set; }

        private IInstanceCreator DefaultInstanceCreator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether concrete types should be translated to interfaces.  Defaults to true.
        /// </summary>
        /// <value><c>true</c> if concrete types should be translated to interfaces; otherwise, <c>false</c>.</value>
        public bool TranslateConcreteTypes { get; set; }

        /// <summary>
        /// Gets or sets the interface prefix that will be used when attempting to lookup the interface.  Defaults to "I"
        /// </summary>
        /// <value>The interface prefix.</value>
        public string InterfacePrefix { get; set; }

        public AutofacInstanceCreator(ILifetimeScope container)
        {
            this.Container = container;
            this.TranslateConcreteTypes = true;
            this.DefaultInstanceCreator = new DefaultInstanceCreator();
            this.InterfacePrefix = "I";
            this.Cache = new MemoryCache("AutofacInstanceCreator");
        }

        /// <summary>
        /// Creates an instance of the object of the specific type.  Note that other instance creators can be written, such as dependency injection instance creators.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        /// <remarks>By default, Activator.CreateInstance will be used to create the instance.  Change the setting InstanceCreator to supply a different instance creator.
        /// With the sharp serializer, the concrete types are going to typically be output as part of the serialization.  You may want to register those types AsSelf, and then
        /// the TranslateConcreteTypes call will never be made.
        /// </remarks>
        public object CreateInstance(Type type)
        {
            object result;
            if (this.Container.IsRegistered(type))
            {
                result = this.Container.Resolve(type);
            }
            else
            {
                if (this.TranslateConcreteTypes)
                {
                    Type interfaceType = this.TranslateToInterface(type);
                    if (interfaceType != null)
                    {
                        result = this.Container.Resolve(interfaceType);
                    }
                    else
                    {
                       result = this.DefaultInstanceCreator.CreateInstance(type);
                    }
                }
                else
                {
                  result = this.DefaultInstanceCreator.CreateInstance(type);
                }
            }

            return result;
        }

        private Type TranslateToInterface(Type concreteType)
        {
          Type result = (Type)this.Cache.Get(concreteType.FullName);

          if (result == null)
          {
            var registration = this.Container.ComponentRegistry.Registrations.FirstOrDefault(r => r.Activator.LimitType == concreteType);
            if (registration != null)
            {
              if (registration.Services.Any())
              {
                Type registrationType = registration.Activator.LimitType;
                string inferredInterfaceName = this.InterfacePrefix + registrationType.Name;

                // exact by convention match is preferred
                foreach (var service in registration.Services)
                {
                  string serviceTypeName = service.Description;
                  Type serviceType = this.GetServiceType(serviceTypeName);
                  if (serviceType != null && serviceType.Name == inferredInterfaceName)
                  {
                    result = serviceType;
                    break;
                  }
                }

                if (result != null)
                {
                  this.Cache.Add(concreteType.FullName, result, new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromHours(1) });
                }

                // no match?  We'll just return null and default back to just a create of the entity directly.  We could attempt a match based on nearest namespace, but it'd be wrong a lot.
              }
            }
          }

          return result;
        }

      private Type GetServiceType(string serviceTypeName)
      {
        Type result = null;
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
          result = assembly.GetType(serviceTypeName);
          if (result != null)
          {
            break;
          }
        }

        return result;
      }
  }
}
