using System;
using Newtonsoft.Json;
using PipServices.Commons.Config;
using PipServices.Commons.Data;
using PipServices.Commons.Refer;

namespace PipServices.Container.Info
{
    public sealed class ContainerInfo : IReconfigurable
    {
        private string _name = "unknown";
        private StringValueMap _properties = new StringValueMap();

        public ContainerInfo(string name = null, string description = null)
        {
            _name = name ?? "unknown";
            Description = description;
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
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        [JsonProperty("uptime")]
        public long Uptime
        {
            get 
            {
                return DateTime.UtcNow.Ticks - StartTime.Ticks;
            }
        }

        [JsonProperty("properties")]
        public StringValueMap Properties
        {
            get { return _properties; }
            set { _properties = value ?? new StringValueMap(); }
        }

        public void Configure(ConfigParams config)
        {
            Name = config.GetAsStringWithDefault("name", Name);
            Name = config.GetAsStringWithDefault("info.name", Name);

            Description = config.GetAsStringWithDefault("description", Description);
            Description = config.GetAsStringWithDefault("info.description", Description);

            Properties = config.GetSection("properties");
        }

        public static ContainerInfo FromConfig(ConfigParams config)
        {
            var result = new ContainerInfo();
            result.Configure(config);
            return result;
        }
    }
}
