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

        private IFactory FindFactory(object locator)
        {
            foreach (var component in GetAll())
            {
                var factory = component as IFactory;
                if (factory != null)
                {
                    if (factory.CanCreate(locator))
                        return factory;
                }
            }

            return null;
        }

        public object Create(object locator)
        {
            // Find factory
            var factory = FindFactory(locator);
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

        public override List<T> Find<T>(ReferenceQuery query, bool required)
        {
            var components = base.Find<T>(query, false);

            // Try to create component
            if (components.Count == 0 && BuildEnabled)
            {
                var component = Create(query.Locator);
                if (component is T)
                {
                    ParentReferences.Put(query.Locator, component);
                    components.Add((T)component);
                }
            }

            // Throw exception is no required components found
            if (required && components.Count == 0)
                throw new ReferenceException(query.Locator);

            return components;
        }
    }
}
