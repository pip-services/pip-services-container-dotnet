using System.Threading.Tasks;
using PipServices.Commons.Refer;
using PipServices.Commons.Config;
using PipServices.Commons.Log;
using PipServices.Commons.Run;

namespace PipServices.Container.Test
{
    public sealed class DummyController : IReferenceable, IReconfigurable, IOpenable, IClosable, INotifiable
    {
        public static Descriptor Descriptor { get; } = new Descriptor("pip-services-dummies", "controller", "*", "1.0");

        private readonly FixedRateTimer _timer;
        private readonly CompositeLogger _logger = new CompositeLogger();
        public string Message { get; private set; } = "Hello World!";
        public long Counter { get; private set; } = 0;

        public DummyController()
        {
            _timer = new FixedRateTimer(this, 1000, 1000);
        }

        public void Configure(ConfigParams config)
        {
            Message = config.GetAsStringWithDefault("message", Message);
        }

        public void SetReferences(IReferences references)
        {
            _logger.SetReferences(references);
        }

        public Task OpenAsync(string correlationId)
        {
            _timer.Start();
            _logger.Trace(correlationId, "Dummy controller opened");

            return Task.Delay(0);
        }

        public async Task CloseAsync(string correlationId)
        {
            await _timer.CloseAsync(correlationId);

            _logger.Trace(correlationId, "Dummy controller closed");
        }

        public Task NotifyAsync(string correlationId)
        {
            _logger.Info(correlationId, "%d - %s", Counter++, Message);

            return Task.Delay(0);
        }
    }
}
