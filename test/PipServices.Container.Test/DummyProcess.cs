using System;
using System.Threading;
using System.Threading.Tasks;
using PipServices.Commons.Refer;
using PipServices.Commons.Config;
using PipServices.Commons.Errors;
using PipServices.Commons.Log;
using PipServices.Commons.Run;

namespace PipServices.Container.Test
{
    public class DummyProcess : ProcessContainer
    {
        protected override void InitReferences(IReferences references)
        {
            base.InitReferences(references);

            // Factory to statically resolve dummy components
            references.Put(new DummyFactory());
        }

        public Task RunAsync(string[] args, CancellationToken token)
        {
            return RunWithConfigFileAsync("dummy", args, "./config/dummy.yaml", token);
        }
    }
}
