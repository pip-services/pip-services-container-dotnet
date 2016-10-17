using System.ComponentModel;
using Xunit;
using PipServices.Container.Config;
using PipServices.Commons.Refer;
using PipServices.Commons.Config;
using PipServices.Commons.Errors;

namespace PipServices.Container.Test.Config
{
    public sealed class ComponentConfigTest
    {
        [Fact]
        public void TestDescriptor()
        {
            var componentConfig = new ComponentConfig();
            Assert.Null(componentConfig.Descriptor);

            var descriptor = new Descriptor("group", "type", "id", "version");
            componentConfig.Descriptor = descriptor;
            Assert.Equal(componentConfig.Descriptor, descriptor);
        }

        [Fact]
        public void TestConfigParams()
        {
            var componentConfig = new ComponentConfig();
            Assert.Null(componentConfig.Config);

            var config = ConfigParams.FromTuples(
                "config.key", "key",
                "config.key2", "key2"
                );
            componentConfig.Config = config;
            Assert.Equal(componentConfig.Config, config);
        }

        [Fact]
        public void TestFromConfig()
        {
            var config = ConfigParams.FromTuples();
            ComponentConfig componentConfig;
            try
            {
                componentConfig = ComponentConfig.FromConfig(config);
            }
            catch (ConfigException e)
            {
                Assert.Equal(e.Message, "Component configuration must have descriptor or type");
            }

            config = ConfigParams.FromTuples(
                "descriptor", "descriptor_name",
                "type", "type",
                "config.key", "key",
                "config.key2", "key2"
                );
            try
            {
                componentConfig = ComponentConfig.FromConfig(config);
            }
            catch (ConfigException e)
            {
                Assert.Equal(e.Message, "Descriptor descriptor_name is in wrong format");
            }

            Descriptor descriptor = new Descriptor("group", "type", "id", "version");
            config = ConfigParams.FromTuples(
                "descriptor", "group:type:id:version",
                "config.key", "key",
                "config.key2", "key2"
                );
            componentConfig = ComponentConfig.FromConfig(config);

            Assert.Equal(componentConfig.Descriptor, descriptor);
        }
    }
}
