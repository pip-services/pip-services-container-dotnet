using PipServices.Commons.Refer;
using PipServices.Commons.Reflect;
using System.Collections.Generic;

namespace PipServices.Container.Refer
{
    public class ReferencesDecorator : IReferences
    {
        public ReferencesDecorator(IReferences baseReferences = null, IReferences parentReferences = null)
        {
            BaseReferences = baseReferences ?? parentReferences;
            ParentReferences = parentReferences ?? baseReferences;
        }

        public IReferences BaseReferences { get; set; }
        public IReferences ParentReferences { get; set; }

        public virtual void Put(object locator, object component)
        {
            BaseReferences.Put(locator, component);
        }

        public virtual object Remove(object locator)
        {
            return BaseReferences.Remove(locator);
        }

        public virtual List<object> RemoveAll(object locator)
        {
            return BaseReferences.RemoveAll(locator);
        }

        public virtual List<object> GetAllLocators()
        {
            return BaseReferences.GetAllLocators();
        }

        public virtual List<object> GetAll()
        {
            return BaseReferences.GetAll();
        }

        public virtual object GetOneOptional(object locator)
        {
            var components = Find<object>(locator, false);
            return components.Count > 0 ? components[0] : null;
        }

        public virtual T GetOneOptional<T>(object locator)
        {
            var components = Find<T>(locator, false);
            return components.Count > 0 ? components[0] : default(T);
        }

        public virtual object GetOneRequired(object locator)
        {
            var components = Find<object>(locator, true);
            return components.Count > 0 ? components[0] : null;
        }

        public virtual T GetOneRequired<T>(object locator)
        {
            var components = Find<T>(locator, true);
            return components.Count > 0 ? components[0] : default(T);
        }

        public virtual List<object> GetOptional(object locator)
        {
            return Find<object>(locator, false);
        }

        public virtual List<T> GetOptional<T>(object locator)
        {
            return Find<T>(locator, false);
        }

        public virtual List<object> GetRequired(object locator)
        {
            return Find<object>(locator, true);
        }

        public virtual List<T> GetRequired<T>(object locator)
        {
            return Find<T>(locator, true);
        }

        public virtual List<object> Find(object locator, bool required)
        {
            return Find<object>(locator, required);
        }

        public virtual List<T> Find<T>(object locator, bool required)
        {
            return BaseReferences.Find<T>(locator, required);
        }

    }
}
