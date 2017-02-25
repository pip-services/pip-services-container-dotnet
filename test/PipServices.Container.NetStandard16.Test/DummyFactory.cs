using PipServices.Commons.Build;
using PipServices.Commons.Refer;

namespace PipServices.Container
{
    public class DummyFactory : IFactory
    {
        public static Descriptor Descriptor { get; } = new Descriptor("pip-services-dummies", "factory", "default", "default", "1.0");

        public bool CanCreate(object locator)
        {
            var descriptor = locator as Descriptor;

            if (descriptor == null)
                return false;

            if (descriptor.Match(DummyController.Descriptor))
                return true;

            return false;
        }

        public object Create(object locator)
        {
            var descriptor = locator as Descriptor;

            if (descriptor == null)
                return null;

            if (descriptor.Match(DummyController.Descriptor))
                return new DummyController();

            return null;
        }
    }
}
