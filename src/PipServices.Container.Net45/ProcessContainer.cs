using System;
using System.Threading;
using System.Threading.Tasks;
using PipServices.Commons.Log;
using PipServices.Container.Config;

namespace PipServices.Container
{
    public class ProcessContainer : Container
    {
        private readonly SemaphoreSlim _exitEvent = new SemaphoreSlim(0);
        private string _correlationId;

        public void ReadConfigFromFile(string correlationId, string[] args, string defaultPath)
        {
            var path = args.Length > 0 ? args [0] : defaultPath;

            ReadConfigFromFile(correlationId, path);
        }

        private void CaptureErrors()
        {
            AppDomain.CurrentDomain.UnhandledException += HandleUncaughtException;
        }

        private void HandleUncaughtException(object sender, UnhandledExceptionEventArgs args)
        {
            Logger.Fatal(_correlationId, (Exception)args.ExceptionObject, "Process is terminated");

            _exitEvent.Release();
        }

        private void CaptureExit(string correlationId)
        {
            Logger.Info(null, "Press Control-C to stop the microservice...");

            AppDomain.CurrentDomain.DomainUnload += (sender, args) => InvokeBatchProcessors(correlationId, Logger, _exitEvent);

            // Wait and close
            try
            {
                _exitEvent.Wait();
            }
            catch (OperationCanceledException)
            {
                // Ignore...
            }
            catch (ObjectDisposedException)
            {
                // Ignore...
            }
        }

        private void InvokeBatchProcessors(string correlationId, ILogger logger, SemaphoreSlim exitEvent)
        {
            logger.Info(correlationId, "Goodbye!");

            exitEvent.Release();

            //Runtime.getRuntime().exit(1);
        }

        public async Task RunAsync(string correlationId, CancellationToken token)
        {
            _correlationId = correlationId;

            CaptureErrors();
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
