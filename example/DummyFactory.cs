using PipServices.Commons.Build;
using PipServices.Commons.Refer;

namespace PipServices.Container
{
    public class DummyFactory : Factory
    {
        public static Descriptor Descriptor = new Descriptor("pip-services-dummies", "factory", "default", "default", "1.0");
        public static Descriptor ControllerDescriptor = new Descriptor("pip-services-dummies", "controller", "*", "*", "1.0");

        public DummyFactory()
        {
            RegisterAsType(ControllerDescriptor, typeof(DummyController));
	    }
    }
}
