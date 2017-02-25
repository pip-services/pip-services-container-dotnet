using PipServices.Commons.Build;
using PipServices.Commons.Refer;

namespace PipServices.Container.Info
{
    public sealed class ContainerInfoFactory : IFactory
    {
        public static readonly Descriptor Descriptor = new Descriptor("pip-services-container", "factory", "container-info", "default", "1.0");
        public static Descriptor ContainerInfoDescriptor = new Descriptor("pip-services-container", "container-info", "default", "*", "1.0");

        public bool CanCreate(object locator)
        {
            var descriptor = locator as Descriptor;

            if (descriptor == null)
                return false;

            if (descriptor.Match(ContainerInfoDescriptor))
                return true;

            return false;
        }

        public object Create(object locator)
        {
            var descriptor = locator as Descriptor;

            if (descriptor == null)
                return null;

            if (descriptor.Match(ContainerInfoDescriptor))
                return new ContainerInfo();

            return null;
        }
    }
}
