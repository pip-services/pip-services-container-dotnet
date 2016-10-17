﻿using System.Collections.Generic;
using PipServices.Commons.Config;

namespace PipServices.Container.Config
{
    public sealed class ContainerConfig : List<ComponentConfig>
    {
        private static long SerialVersionUid { get; } = -1686520964953606299L;

        public ContainerConfig() { }

        public ContainerConfig(IEnumerable<ComponentConfig> components)
        {
            if (components != null)
                AddRange(components);
        }

        public static ContainerConfig FromObject(object value)
        {
            var config = ConfigParams.FromObject(value);

            return FromConfig(config);
        }

        public static ContainerConfig FromConfig(ConfigParams config)
        {
            var result = new ContainerConfig();

            if (config == null)
                return result;

            foreach(var section in config.GetSectionNames())
            {
                var componentConfig = config.GetSection(section);

                result.Add(ComponentConfig.FromConfig(componentConfig));
            }

            return result;
        }
    }
}
