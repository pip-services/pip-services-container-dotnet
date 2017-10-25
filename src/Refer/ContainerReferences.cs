using System;
using System.Linq;
using PipServices.Commons.Build;
using PipServices.Commons.Config;
using PipServices.Commons.Refer;
using PipServices.Commons.Reflect;
using PipServices.Container.Config;

namespace PipServices.Container.Refer
{
    public sealed class ContainerReferences : ManagedReferences
    {
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
                    else if (componentConfig.Descriptor != null)
                    {
                        locator = componentConfig.Descriptor;
                        IFactory factory = _builder.FindFactory(locator);
                        component = _builder.Create(locator, factory);
                        if (component == null)
                            throw new ReferenceException(null, locator);
                        locator = _builder.ClarifyLocator(locator, factory);
                    }

                    // Check that component was created
                    if (component == null)
                    {
                        throw new CreateException("CANNOT_CREATE_COMPONENT", "Cannot create component")
                                .WithDetails("config", config);
                    }

                    // Add component to the list
                    _references.Put(locator, component);

                    // Configure component
                    var configurable = component as IConfigurable;
                    configurable?.Configure(componentConfig.Config);

                    // Set references to factories
                    if (component is IFactory)
                    {
                        var referenceable = component as IReferenceable;
                        referenceable?.SetReferences(this);
                    }
                }
                catch (Exception ex)
                {
                    throw new ReferenceException(null, locator).WithCause(ex);
                }
            }
        }
    }
}
