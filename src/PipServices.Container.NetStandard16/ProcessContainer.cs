using PipServices.Container.Config;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PipServices.Container
{
    public class ProcessContainer : Container
    {
        private readonly ManualResetEvent _exitEvent = new ManualResetEvent(false);

        public object AppDomain { get; private set; }

        public void ReadConfigFromFile(string correlationId, string[] args, string defaultPath)
        {
            var path = args.Length > 0 ? args [0] : defaultPath;
            ReadConfigFromFile(correlationId, path);
        }

        private void CaptureErrors(string correlationId)
        {
#if !CORE_NET
            AppDomain.CurrentDomain.UnhandledException += (obj, e) =>
            {
                Logger.Fatal(correlationId, e.ExceptionObject, "Process is terminated");
                _exitEvent.Set();
            };
#endif
        }

        private void CaptureExit(string correlationId)
        {
            Logger.Info(correlationId, "Press Control-C to stop the microservice...");

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Logger.Info(correlationId, "Goodbye!");

                eventArgs.Cancel = true;
                _exitEvent.Set();

                Environment.Exit(1);
            };

            // Wait and close
            _exitEvent.WaitOne();
        }

        public async Task RunAsync(string correlationId, CancellationToken token)
        {
            CaptureErrors(correlationId);
            await StartAsync(correlationId, token);
            CaptureExit(correlationId);
            await StopAsync(correlationId, token);
        }

        public Task RunWithConfigAsync(string correlationId, ContainerConfig config, CancellationToken token)
        {
            Config = config;
            return RunAsync(correlationId, token);
        }

        public Task RunWithConfigFileAsync(string correlationId, string path, CancellationToken token)
        {
            ReadConfigFromFile(correlationId, path);
            return RunAsync(correlationId, token);
        }

        public Task RunWithConfigFileAsync(string correlationId, string[] args, string defaultPath, CancellationToken token)
        {
            ReadConfigFromFile(correlationId, args, defaultPath);
            return RunAsync(correlationId, token);
        }
    }
}
