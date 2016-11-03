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

            var index = path.LastIndexOf('.');
            var ext = index > 0 ? path.Substring(index + 1).ToLower() : "";

            if (ext.Equals("json"))
                return ReadFromJsonFile(correlationId, path);

            if (ext.Equals("yaml"))
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
