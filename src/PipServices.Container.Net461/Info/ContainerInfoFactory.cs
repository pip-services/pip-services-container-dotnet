using PipServices.Commons.Build;
using PipServices.Commons.Refer;

namespace PipServices.Container.Info
{
    public sealed class ContainerInfoFactory : IFactory, IDescriptable
    {
        private static Descriptor Descriptor { get; }= new Descriptor("pip-services-container", "factory", "container-info", "1.0");

        public Descriptor GetDescriptor()
        {
            return Descriptor;
        }

        public bool CanCreate(object locator)
        {
            var descriptor = locator as Descriptor;

            if (descriptor == null)
                return false;

            if (descriptor.Match(ContainerInfo.Descriptor))
                return true;

            return false;
        }

        public object Create(object locator)
        {
            var descriptor = locator as Descriptor;

            if (descriptor == null)
                return null;

            if (descriptor.Match(ContainerInfo.Descriptor))
                return new ContainerInfo();

            return null;
        }
    }
}
