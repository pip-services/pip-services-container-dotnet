using System;
using System.Threading.Tasks;
using PipServices.Commons.Config;
using PipServices.Commons.Errors;
using PipServices.Commons.Info;
using PipServices.Commons.Log;
using PipServices.Commons.Refer;
using PipServices.Commons.Run;
using PipServices.Container.Build;
using PipServices.Container.Config;
using PipServices.Container.Refer;

namespace PipServices.Container
{
    public class Container: IConfigurable, IReferenceable, IUnreferenceable, IOpenable
    {
        protected ILogger _logger = new NullLogger();
        protected DefaultContainerFactory _factories = new DefaultContainerFactory();
        protected ContextInfo _info;
        protected ContainerConfig _config;
        protected ContainerReferences _references;

        public Container(string name = null, string description = null) 
        {
            _info = new ContextInfo(name, description);
        }

        public virtual void Configure(ConfigParams config)
        {
            _config = ContainerConfig.FromConfig(config);
        }

        public void ReadConfigFromFile(string correlationId, string path, ConfigParams parameters)
        {
            _config = ContainerConfigReader.ReadFromFile(correlationId, path, parameters);
        }

        public virtual void SetReferences(IReferences references)
        {
            // Override in child class
        }

        public virtual void UnsetReferences()
        {
            // Override in child class
        }

        protected virtual void InitReferences(IReferences references)
        {
            var existingInfo = references.GetOneOptional<ContextInfo>(DefaultInfoFactory.ContextInfoDescriptor);
            if (existingInfo == null)
                references.Put(DefaultInfoFactory.ContextInfoDescriptor, _info);
            else _info = existingInfo;

            references.Put(DefaultContainerFactory.Descriptor, _factories);
        }

        public virtual bool IsOpened()
        {
            return _references != null;
        }

        public async Task OpenAsync(string correlationId)
        {
            if (_references != null)
                throw new InvalidStateException(correlationId, "ALREADY_OPENED", "Container was already opened");

            //if (_config == null)
            //    throw new InvalidStateException(correlationId, "NO_CONFIG", "Container was not configured");

            try
            {
                _logger.Trace(correlationId, "Starting container.");

                // Create references with configured components
                _references = new ContainerReferences();
                InitReferences(_references);
                _references.PutFromConfig(_config);
                SetReferences(_references);

                // Get custom description if available
                var infoDescriptor = new Descriptor("*", "context-info", "*", "*", "*");
                _info = _references.GetOneRequired<ContextInfo>(infoDescriptor);

                await _references.OpenAsync(correlationId);

                // Get reference to logger
                _logger = new CompositeLogger(_references);
                _logger.Info(correlationId, "Container {0} started.", _info.Name);
            }
            catch (Exception ex)
            {
                _logger.Error(correlationId, ex, "Failed to start container");

                await CloseAsync(correlationId);

                throw;
            }
        }

        public async Task CloseAsync(string correlationId)
        {
            if (_references == null)
                return;

            try
            {
                _logger.Trace(correlationId, "Stopping {0} container", _info.Name);

                // Close and dereference components
                await _references.CloseAsync(correlationId);
                _references = null;

                _logger.Info(correlationId, "Container {0} stopped", _info.Name);
            }
            catch (Exception ex)
            {
                _logger.Error(correlationId, ex, "Failed to stop container");
                throw;
            }
        }
    }
}
