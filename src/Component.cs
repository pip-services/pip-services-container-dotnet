using System;
using PipServices.Commons.Config;
using PipServices.Commons.Count;
using PipServices.Commons.Log;
using PipServices.Commons.Refer;

namespace PipServices.Container
{
    public class Component: IConfigurable, IReferenceable
    {
        protected DependencyResolver _dependencyResolver = new DependencyResolver();
        protected CompositeLogger _logger = new CompositeLogger();
        protected CompositeCounters _counters = new CompositeCounters();

        public void Configure(ConfigParams config)
        {
            _dependencyResolver.Configure(config);
            _logger.Configure(config);
        }

        public void SetReferences(IReferences references)
        {
            _dependencyResolver.SetReferences(references);
            _logger.SetReferences(references);
            _counters.SetReferences(references);
        }
    }
}
