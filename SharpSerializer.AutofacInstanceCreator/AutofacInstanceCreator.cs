namespace SharpSerializer.AutofacInstanceCreator
{
    using System;
    using System.Linq;
    using Autofac;
    using Polenter.Serialization.Core;
    public class AutofacInstanceCreator : IInstanceCreator
    {
        private IContainer Container { get; set; }

        private IInstanceCreator DefaultInstanceCreator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether concrete types should be translated to interfaces.  Defaults to true.
        /// </summary>
        /// <value><c>true</c> if concrete types should be translated to interfaces; otherwise, <c>false</c>.</value>
        public bool TranslateConcreteTypes { get; set; }

        public AutofacInstanceCreator(IContainer container)
        {
            this.Container = container;
            this.TranslateConcreteTypes = true;
            this.DefaultInstanceCreator = new DefaultInstanceCreator();
        }

        /// <summary>
        /// Creates an instance of the object of the specific type.  Note that other instance creators can be written, such as dependency injection instance creators.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        /// <remarks>By default, Activator.CreateInstance will be used to create the instance.  Change the setting InstanceCreator to supply a different instance creator.</remarks>
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
          Type result = null;
          var registration = this.Container.ComponentRegistry.Registrations.FirstOrDefault(r => r.Activator.LimitType == concreteType);
          if (registration != null)
          {
            if (registration.Services.Any())
            {
              string typeName = registration.Services.Last().Description;
              foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
              {
                result = assembly.GetType(typeName);
                if (result != null)
                {
                  break;
                }
              }
            }
          }

          return result;
        }
  }
}
