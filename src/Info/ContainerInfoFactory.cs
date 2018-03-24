using PipServices.Commons.Build;
using PipServices.Commons.Refer;

namespace PipServices.Container.Info
{
    public class ContainerInfoFactory : Factory
    {
        public static readonly Descriptor Descriptor = new Descriptor("pip-services", "factory", "container-info", "default", "1.0");
        public static Descriptor ContainerInfoDescriptor = new Descriptor("pip-services", "container-info", "default", "*", "1.0");
        public static Descriptor ContainerInfoDescriptor2 = new Descriptor("pip-services-container", "container-info", "default", "*", "1.0");

        public ContainerInfoFactory()
        {
            RegisterAsType(ContainerInfoDescriptor, typeof(ContainerInfo));
            RegisterAsType(ContainerInfoDescriptor2, typeof(ContainerInfo));
	    }
    }
}
