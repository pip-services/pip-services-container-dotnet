using PipServices.Commons.Build;
using PipServices.Commons.Refer;
using System;
using System.Collections.Generic;

namespace PipServices.Container.Refer
{
    public class BuildReferencesDecorator: ReferencesDecorator
    {
        public BuildReferencesDecorator(IReferences baseReferences = null, IReferences parentReferences = null)
            : base(baseReferences, parentReferences)
        {
            BuildEnabled = true;
        }

        public bool BuildEnabled { get; set; }

        public IFactory FindFactory(object locator)
        {
            foreach (var component in GetAll())
            {
                var factory = component as IFactory;
                if (factory != null)
                {
                    if (factory.CanCreate(locator) != null)
                        return factory;
                }
            }

            return null;
        }

        public object Create(object locator, IFactory factory)
        {
            // Find factory
            if (factory == null) return null;

            try
            {
                // Create component
                return factory.Create(locator);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ClarifyLocator(object locator, IFactory factory)
        {
            if (factory == null) return locator;
            if (!(locator is Descriptor)) return locator;

            object anotherLocator = factory.CanCreate(locator);
            if (anotherLocator == null) return locator;
            if (!(anotherLocator is Descriptor)) return locator;

            Descriptor descriptor = (Descriptor)locator;
            Descriptor anotherDescriptor = (Descriptor)anotherLocator;

            return new Descriptor(
                descriptor.Group != null ? descriptor.Group : anotherDescriptor.Group,
                descriptor.Type != null ? descriptor.Type : anotherDescriptor.Type,
                descriptor.Kind != null ? descriptor.Kind : anotherDescriptor.Kind,
                descriptor.Name != null ? descriptor.Name : anotherDescriptor.Name,
                descriptor.Version != null ? descriptor.Version : anotherDescriptor.Version
            );
        }

        public override List<T> Find<T>(ReferenceQuery query, bool required)
        {
            var components = base.Find<T>(query, false);
            var locator = query.Locator;

            // Try to create component
            if (components.Count == 0 && BuildEnabled)
            {
                var factory = FindFactory(locator);
                var component = Create(locator, factory);
                if (component is T)
                {
                    locator = ClarifyLocator(locator, factory);
                    ParentReferences.Put(locator, component);
                    components.Add((T)component);
                }
            }

            // Throw exception is no required components found
            if (required && components.Count == 0)
                throw new ReferenceException(locator);

            return components;
        }
    }
}
