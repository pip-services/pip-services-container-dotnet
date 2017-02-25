using PipServices.Commons.Refer;
using PipServices.Commons.Run;
using System.Collections.Generic;

namespace PipServices.Container.Refer
{
    public class RunReferencesDecorator : ReferencesDecorator
    {
        public RunReferencesDecorator(IReferences baseReferences = null, IReferences parentReferences = null)
            : base(baseReferences, parentReferences)
        {
            OpenEnabled = true;
            CloseEnabled = true;
        }

        public bool OpenEnabled { get; set; }
        public bool CloseEnabled { get; set; }

        public override void Put(object locator, object component)
        {
            base.Put(locator, component);

            if (OpenEnabled)
                Opener.OpenOneAsync(null, component).Wait();
        }

        public override object Remove(object locator)
        {
            var component = base.Remove(locator);

            if (CloseEnabled)
                Closer.CloseOneAsync(null, component).Wait();

            return component;
        }

        public override List<object> RemoveAll(object locator)
        {
            var components = base.RemoveAll(locator);

            if (CloseEnabled)
                Closer.CloseAsync(null, components).Wait();

            return components;
        }

    }
}
