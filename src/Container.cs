using System;
using System.Threading;
using System.Threading.Tasks;
using PipServices.Commons.Errors;
using PipServices.Commons.Log;
using PipServices.Commons.Refer;
using PipServices.Commons.Run;
using PipServices.Container.Build;
using PipServices.Container.Config;
using PipServices.Container.Info;
using PipServices.Container.Refer;

namespace PipServices.Container
{
    public class Container
    {
        public Container() { }

        public Container(ContainerConfig config)
        {
            Config = config;
        }

        protected ILogger Logger = new NullLogger();
        public ContainerInfo Info { get; protected set; } = new ContainerInfo();
        public ContainerConfig Config { get; set; }
        public ContainerReferences References { get; protected set; } = new ContainerReferences();

        public void ReadConfigFromFile(string correlationId, string path)
        {
            Config = ContainerConfigReader.ReadFromFile(correlationId, path);
        }

        protected virtual void InitReferences(IReferences references)
        {
            // Override in base classes
            references.Put(DefaultContainerFactory.Descriptor, new DefaultContainerFactory());
        }

        public async Task StartAsync(string correlationId, CancellationToken token)
        {
            if (Config == null)
                throw new InvalidStateException(correlationId, "NO_CONFIG", "Container was not configured");

            try
            {
                Logger.Trace(correlationId, "Starting container.");

                // Create references with configured components
                InitReferences(References);
                References.PutFromConfig(Config);

                // Reference and open components
                var components = References.GetAll();
                Referencer.SetReferences(References, components);
                await Opener.OpenAsync(correlationId, References.GetAll());

                // Get reference to logger
                Logger = new CompositeLogger(References);

                // Get reference to container info
                var infoDescriptor = new Descriptor("*", "container-info", "*", "*", "*");
                Info = (ContainerInfo) References.GetOneRequired(infoDescriptor);

                Logger.Info(correlationId, "Container {0} started.", Info.Name);
            }
            catch (Exception ex)
            {
                References = null;
                Logger.Error(correlationId, ex, "Failed to start container");

                throw;
            }
        }

        public async Task StopAsync(string correlationId, CancellationToken token)
        {
            if (References == null)
                throw new InvalidStateException(correlationId, "NO_STARTED", "Container was not started");

            try
            {
                Logger.Trace(correlationId, "Stopping {0} container", Info.Name);

                // Close and deference components
                await References.CloseAsync(correlationId);

                Logger.Info(correlationId, "Container {0} stopped", Info.Name);
            }
            catch (Exception ex)
            {
                Logger.Error(correlationId, ex, "Failed to stop container");
                throw;
            }
        }
    }
}
