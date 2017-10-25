using System.IO;
using PipServices.Commons.Config;
using PipServices.Commons.Errors;

namespace PipServices.Container.Config
{
    public sealed class ContainerConfigReader
    {

        public static ContainerConfig ReadFromFile(string correlationId, string path, ConfigParams parameters)
        {
            if (path == null)
                throw new ConfigException(correlationId, "NO_PATH", "Missing config file path");

            var ext = Path.GetExtension(path);

            if (ext.Equals(".json"))
                return ReadFromJsonFile(correlationId, path, parameters);

            if (ext.Equals(".yaml") || ext.Equals(".yml"))
                return ReadFromYamlFile(correlationId, path, parameters);

            // By default read as Yaml
            return ReadFromYamlFile(correlationId, path, parameters);
        }

        public static ContainerConfig ReadFromJsonFile(string correlationId, string path, ConfigParams parameters)
        {
            var config = JsonConfigReader.ReadConfig(correlationId, path, parameters);
            return ContainerConfig.FromConfig(config);
        }

        public static ContainerConfig ReadFromYamlFile(string correlationId, string path, ConfigParams parameters)
        {
            var config = YamlConfigReader.ReadConfig(correlationId, path, parameters);
            return ContainerConfig.FromConfig(config);
        }
    }
}
