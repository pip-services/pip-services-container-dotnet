using PipServices.Commons.Refer;
using PipServices.Commons.Run;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipServices.Container.Refer
{
    public class RunReferencesDecorator : ReferencesDecorator, IOpenable
    {
        private bool _opened = false;

        public RunReferencesDecorator(IReferences baseReferences = null, IReferences parentReferences = null)
            : base(baseReferences, parentReferences)
        { }

        public bool IsOpened()
        {
            return _opened;    
        }

        public async Task OpenAsync(string correlationId)
        {
            if (!_opened)
            {
                var components = base.GetAll();
                await Opener.OpenAsync(correlationId, components);
                _opened = true;
            }
        }

        public async Task CloseAsync(string correlationId)
        {
            if (_opened)
            {
                var components = base.GetAll();
                await Closer.CloseAsync(correlationId, components);
                _opened = false;
            }
        }

        public override void Put(object locator, object component)
        {
            base.Put(locator, component);

            if (_opened)
                Opener.OpenOneAsync(null, component).Wait();
        }

        public override object Remove(object locator)
        {
            var component = base.Remove(locator);

            if (_opened)
                Closer.CloseOneAsync(null, component).Wait();

            return component;
        }

        public override List<object> RemoveAll(object locator)
        {
            var components = base.RemoveAll(locator);

            if (_opened)
                Closer.CloseAsync(null, components).Wait();

            return components;
        }

    }
}
