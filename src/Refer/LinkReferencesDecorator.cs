using System.Collections.Generic;
using System.Threading.Tasks;
using PipServices.Commons.Refer;
using PipServices.Commons.Run;

namespace PipServices.Container.Refer
{
    public class LinkReferencesDecorator : ReferencesDecorator, IOpenable
    {
        private bool _opened = false;

        public LinkReferencesDecorator(IReferences baseReferences = null, IReferences parentReferences = null)
            : base(baseReferences, parentReferences)
        {}

        public bool IsOpened()
        {
            return _opened;
        }

        public async Task OpenAsync(string correlationId)
        {
            if (!_opened)
            {
                _opened = true;
                var components = base.GetAll();
                Referencer.SetReferences(this.ParentReferences, components);
            }

            await Task.Delay(0);
        }

        public async Task CloseAsync(string correlationId)
        {
            if (_opened)
            {
                _opened = false;
                var components = base.GetAll();
                Referencer.UnsetReferences(components);
            }

            await Task.Delay(0);
        }

        public override void Put(object locator, object component)
        {
            base.Put(locator, component);

            if (_opened)
                Referencer.SetReferencesForOne(ParentReferences, component);
        }

        public override object Remove(object locator)
        {
            var component = base.Remove(locator);

            if (_opened)
                Referencer.UnsetReferencesForOne(component);

            return component;
        }

        public override List<object> RemoveAll(object locator)
        {
            var components = base.RemoveAll(locator);

            if (_opened)
                Referencer.UnsetReferences(components);

            return components;
        }

    }
}
