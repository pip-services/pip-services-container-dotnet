using PipServices.Commons.Config;
using PipServices.Commons.Errors;
using PipServices.Commons.Refer;
using PipServices.Commons.Reflect;

namespace PipServices.Container.Config
{
    public sealed class ComponentConfig
    {
        public ComponentConfig() { }

        public ComponentConfig(Descriptor descriptor, TypeDescriptor type, ConfigParams config)
        {
            Descriptor = descriptor;
            Type = type;
            Config = config;
        }

        public Descriptor Descriptor { get; set; }
        public TypeDescriptor Type { get; set; }
        public ConfigParams Config { get; set; }

        public static ComponentConfig FromConfig(ConfigParams config)
        {
            var descriptor = Descriptor.FromString(config.GetAsNullableString("descriptor"));
            var type = TypeDescriptor.FromString(config.GetAsNullableString("type"));

            if (descriptor == null && type == null)
                throw new ConfigException(null, "BAD_CONFIG", "Component configuration must have descriptor or type");

            return new ComponentConfig(descriptor, type, config);
        }
    }
}
