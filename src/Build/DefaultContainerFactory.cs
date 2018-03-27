using PipServices.Commons.Auth;
using PipServices.Commons.Build;
using PipServices.Commons.Cache;
using PipServices.Commons.Config;
using PipServices.Commons.Connect;
using PipServices.Commons.Count;
using PipServices.Commons.Info;
using PipServices.Commons.Log;
using PipServices.Commons.Refer;

namespace PipServices.Container.Build
{
    public class DefaultContainerFactory : CompositeFactory
    {
        public static readonly Descriptor Descriptor = new Descriptor("pip-services", "factory", "container", "default", "1.0");

        public DefaultContainerFactory(params IFactory[] factories)
            : base(factories)
        {
            Add(new InfoFactory());
            Add(new DefaultLoggerFactory());
            Add(new DefaultCountersFactory());
            Add(new DefaultConfigReaderFactory());
            Add(new DefaultCacheFactory());
            Add(new DefaultCredentialStoreFactory());
            Add(new DefaultDiscoveryFactory());
        }
    }
}
