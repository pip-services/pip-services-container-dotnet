using System.Threading;
using System.Threading.Tasks;
using PipServices.Commons.Refer;

namespace PipServices.Container
{
    public class DummyProcess : ProcessContainer
    {
        public DummyProcess()
            : base("dummy", "Sample dummy process")
        {
            this._configPath = "./dummy.yml";
            this._factories.Add(new DummyFactory());
        }
    }
}
