using PipServices.Commons.Config;
using PipServices.Commons.Errors;
using PipServices.Commons.Refer;

namespace PipServices.Container.Config
{
    public sealed class ComponentConfig
    {
        public ComponentConfig()
        {
        }

        public ComponentConfig(Descriptor descriptor, ConfigParams config)
        {
            Descriptor = descriptor;
            Config = config;
        }

        public Descriptor Descriptor { get; set; }

        public ConfigParams Config { get; set; }

        public static ComponentConfig FromConfig(ConfigParams config)
        {
            var descriptor = Descriptor.FromString(config.GetAsNullableString("descriptor"));

            if (descriptor == null)
                throw new ConfigException(null, "BAD_CONFIG", "Component configuration must have descriptor or type");

            return new ComponentConfig(descriptor, config);
        }
    }
}
