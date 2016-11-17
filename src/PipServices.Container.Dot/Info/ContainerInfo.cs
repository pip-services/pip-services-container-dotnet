using System;
using Newtonsoft.Json;
using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Commons.Refer;

namespace PipServices.Container.Info
{
    public sealed class ContainerInfo : IDescriptable
    {
        public static Descriptor Descriptor { get; } = new Descriptor("pip-services-container", "container-info",
            "default", "1.0");

        private string _name = "unknown";
        private StringValueMap _properties = new StringValueMap();

        public Descriptor GetDescriptor()
        {
            return Descriptor;
        }

        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set { _name = value ?? "unknown"; }
        }

        [JsonProperty("description")]
        public string Description { get; set; } = null;

        [JsonProperty("container_id")]
        public string ContainerId { get; set; } = IdGenerator.NextLong();

        [JsonProperty("start_time")]
        public DateTimeOffset StartTime { get; set; } = DateTimeOffset.UtcNow;


        [JsonProperty("properties")]
        public StringValueMap Properties
        {
            get { return _properties; }
            set { _properties = value ?? new StringValueMap(); }
        }

        public static ContainerInfo FromConfig(ConfigParams config)
        {
            var result = new ContainerInfo();

            var info = config.GetSection("info");

            result.Name = info.GetAsNullableString("name");
            result.Description = info.GetAsNullableString("description");
            result.Properties = config.GetSection("properties");

            return result;
        }
    }
}
