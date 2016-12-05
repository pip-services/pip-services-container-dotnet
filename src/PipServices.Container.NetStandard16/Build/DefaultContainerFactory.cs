using PipServices.Commons.Build;
using PipServices.Commons.Cache;
using PipServices.Commons.Count;
using PipServices.Commons.Log;
using PipServices.Commons.Refer;
using PipServices.Container.Info;

namespace PipServices.Container.Build
{
    public class DefaultContainerFactory : CompositeFactory, IDescriptable
    {
        public static Descriptor Descriptor { get; } = new Descriptor("pip-services-container", "factory", "container", "default", "1.0");

        public DefaultContainerFactory()
        {
            Add(new ContainerInfoFactory());
            Add(new DefaultLoggerFactory());
            Add(new DefaultCountersFactory());
            Add(new DefaultCacheFactory());
        }

        public Descriptor GetDescriptor()
        {
            return Descriptor;
        }
    }
}
