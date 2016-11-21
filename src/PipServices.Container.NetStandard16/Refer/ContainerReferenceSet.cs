using System;
using System.Linq;
using PipServices.Commons.Build;
using PipServices.Commons.Config;
using PipServices.Commons.Refer;
using PipServices.Commons.Reflect;
using PipServices.Container.Config;

namespace PipServices.Container.Refer
{
    public sealed class ContainerReferenceSet : ReferenceSet
    {
        private IFactory FindFactory(object locator)
        {
            foreach (var factory in References.Cast<IFactory>())
            {
                if (factory != null)
                {
                    if (factory.CanCreate(locator))
                        return factory;
                }
            }

            return null;
        }

        private object CreateStatically(object locator)
        {
            // Find factory
            var factory = FindFactory(locator);

            if (factory == null)
                return null;

            try
            {
                // Create component
                var component = factory.Create(locator);

                if (component == null)
                    return null;

                // Replace locator
                if (component is IDescriptable)
                    locator = ((IDescriptable) component).GetDescriptor();

                return component;
            }
            catch (CreateException ex)
            {
                throw new ReferenceException(null, locator).WithCause(ex);
            }
        }

        protected override object ResolveMissing(object locator)
        {
            var component = CreateStatically(locator);

            // Add to the list
            if (component != null)
                Put(component, locator);

            // Reference with other components
            var referenceable = component as IReferenceable;

            referenceable?.SetReferences(this);

            return component;
        }

        public void PutFromConfig(ContainerConfig config)
        {
            foreach (var componentConfig in config)
            {
                object component = null;
                object locator = null;

                try
                {
                    // Create component dynamically
                    if (componentConfig.Type != null)
                    {
                        locator = componentConfig.Type;
                        component = TypeReflector.CreateInstanceByDescriptor(componentConfig.Type);
                    }

                    // Or create component statically
                    if (componentConfig.Descriptor != null)
                    {
                        locator = componentConfig.Descriptor;
                        component = CreateStatically(componentConfig.Descriptor);
                    }

                    // Check that component was created
                    if (component == null)
                    {
                        throw new CreateException("CANNOT_CREATE_COMPONENT", "Cannot create component")
                                .WithDetails("config", config);
                    }

                    // Add component to the list
                    if (component is ILocateable || component is IDescriptable)
                        Put(component);
                    else
                        Put(component, locator);

                    // Configure component
                    var configurable = component as IConfigurable;

                    configurable?.Configure(componentConfig.Config);
                }
                catch (Exception ex)
                {
                    throw new ReferenceException(null, locator).WithCause(ex);
                }
            }
        }
    }
}
