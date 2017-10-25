using System.Threading;
using System.Threading.Tasks;
using PipServices.Commons.Refer;

namespace PipServices.Container
{
    public class DummyProcess : ProcessContainer
    {
        protected override void InitReferences(IReferences references)
        {
            base.InitReferences(references);

            // Factory to statically resolve dummy components
            references.Put(DummyFactory.Descriptor, new DummyFactory());
        }

        public Task RunAsync(string[] args, CancellationToken token)
        {
            return RunWithConfigFileAsync("dummy", args, "./config/dummy.yaml", token);
        }
    }
}
