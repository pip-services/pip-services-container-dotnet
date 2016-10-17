using System;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using PipServices.Commons.Log;
using PipServices.Container.Config;

namespace PipServices.Container
{
    public class ProcessContainer : Container
    {
        private readonly SemaphoreSlim _exitEvent = new SemaphoreSlim(0);

        public void ReadConfigFromFile(string correlationId, string[] args, string defaultPath)
        {
            var path = args.Length > 0 ? args [0] : defaultPath;

            ReadConfigFromFile(correlationId, path);
        }

        //public void UncaughtException(Thread thread, Exception ex)
        //{
        //    Logger.Fatal(correlationId, ex, "Process is terminated");

        //    _exitEvent.Release();
        //}

        private void CaptureErrors(string correlationId)
        {
            //Thread.SetDefaultUncaughtExceptionHandler(new Thread.UncaughtExceptionHandler());
        }

        private void CaptureExit(string correlationId)
        {
            Logger.Info(null, "Press Control-C to stop the microservice...");

            AssemblyLoadContext.Default.Unloading += context => InvokeBatchProcessors(correlationId, Logger, _exitEvent);

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

        public Task RunWithConfigFile(string correlationId, string[] args, string defaultPath, CancellationToken token)
        {
            ReadConfigFromFile(correlationId, args, defaultPath);
            return RunAsync(correlationId, token);
        }
    }
}
