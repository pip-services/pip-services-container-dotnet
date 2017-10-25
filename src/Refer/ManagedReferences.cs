using PipServices.Commons.Refer;
using PipServices.Commons.Run;
using System.Threading.Tasks;

namespace PipServices.Container.Refer
{
    public class ManagedReferences: ReferencesDecorator, IOpenable
    {
        protected References _references;
        protected BuildReferencesDecorator _builder;
        protected LinkReferencesDecorator _linker;
        protected RunReferencesDecorator _runner;

        public ManagedReferences(object[] tuples = null)
            : base(null, null)
        {
            _references = new References(tuples);
            _builder = new BuildReferencesDecorator(_references, this);
            _linker = new LinkReferencesDecorator(_builder, this);
            _runner = new RunReferencesDecorator(_linker, this);

            BaseReferences = _runner;
        }

        public bool IsOpened()
        {
            return _linker.IsOpened() && _runner.IsOpened();
        }

        public async Task OpenAsync(string correlationId)
        {
            await _linker.OpenAsync(correlationId);
            await _runner.OpenAsync(correlationId);
        }

        /// <summary>
        /// close all references as an asynchronous operation.
        /// </summary>
        /// <param name="correlationId">a unique transaction id to trace calls across components</param>
        /// <returns>Task.</returns>
        public async Task CloseAsync(string correlationId)
        {
            await _runner.CloseAsync(correlationId);
            await _linker.CloseAsync(correlationId);
        }

        public static ManagedReferences FromTyples(params object[] tuples)
        {
            return new ManagedReferences(tuples);
        }
    }
}
