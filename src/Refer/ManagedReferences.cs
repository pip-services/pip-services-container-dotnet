using PipServices.Commons.Refer;
using PipServices.Commons.Run;
using System.Threading.Tasks;

namespace PipServices.Container.Refer
{
    public class ManagedReferences: ReferencesDecorator, IOpenable, IClosable
    {
        protected References _references;
        protected BuildReferencesDecorator _builder;
        protected LinkReferencesDecorator _linker;
        protected RunReferencesDecorator _runner;

        public ManagedReferences(object[] tuples = null)
        {
            _references = new References(tuples);
            _builder = new BuildReferencesDecorator(_references, this);
            _linker = new LinkReferencesDecorator(_builder, this);
            _runner = new RunReferencesDecorator(_linker, this);

            BaseReferences = _runner;
        }

        public bool IsOpened()
        {
            var components = _references.GetAll();
            return Opener.IsOpened(components);
        }

        public async Task OpenAsync(string correlationId)
        {
            var components = _references.GetAll();
            Referencer.SetReferences(this, components);
            await Opener.OpenAsync(correlationId, components);
        }

        /// <summary>
        /// close all references as an asynchronous operation.
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <returns>Task.</returns>
        public async Task CloseAsync(string correlationId)
        {
            var components = _references.GetAll();
            await Closer.CloseAsync(correlationId, components);
            Referencer.UnsetReferences(components);

            _references.Clear();
        }

        public static ManagedReferences FromTyples(params object[] tuples)
        {
            return new ManagedReferences(tuples);
        }
    }
}
