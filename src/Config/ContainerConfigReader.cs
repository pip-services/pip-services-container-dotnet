using System.IO;
using PipServices.Commons.Config;
using PipServices.Commons.Errors;

namespace PipServices.Container.Config
{
    public sealed class ContainerConfigReader
    {

        public static ContainerConfig ReadFromFile(string correlationId, string path)
        {
            if (path == null)
                throw new ConfigException(correlationId, "NO_PATH", "Missing config file path");

            var ext = Path.GetExtension(path);

            if (ext.Equals(".json"))
                return ReadFromJsonFile(correlationId, path);

            if (ext.Equals(".yaml"))
                return ReadFromYamlFile(correlationId, path);

            // By default read as JSON
            return ReadFromJsonFile(correlationId, path);
        }

        public static ContainerConfig ReadFromJsonFile(string correlationId, string path)
        {
            var config = JsonConfigReader.ReadConfig(correlationId, path);
            return ContainerConfig.FromConfig(config);
        }

        public static ContainerConfig ReadFromYamlFile(string correlationId, string path)
        {
            var config = YamlConfigReader.ReadConfig(correlationId, path);
            return ContainerConfig.FromConfig(config);
        }
    }
}
