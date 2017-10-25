using System;
using System.Threading;
using System.Threading.Tasks;
using PipServices.Commons.Log;
using PipServices.Commons.Config;

namespace PipServices.Container
{
    public class ProcessContainer : Container
    {
        protected string _configPath = "../config/config.yml";
        private readonly ManualResetEvent _exitEvent = new ManualResetEvent(false);

        public ProcessContainer(string name = null, string description = null)
            : base(name, description)
        {
            _logger = new ConsoleLogger();
        }

        private string GetConfigPath(string[] args)
        {
            for (var index = 0; index < args.Length; index++) {
                var arg = args[index];
                var nextArg = index < args.Length - 1 ? args[index + 1] : null;
                nextArg = nextArg != null && nextArg.StartsWith("-", StringComparison.InvariantCulture) ? null : nextArg;
                if (nextArg != null) 
                {
                    if (arg == "--config" || arg == "-c")
                    {
                        return nextArg;
                    }
                }
            }
            return _configPath;
        }

        private ConfigParams GetParameters(string[] args)
        {
            // Process command line parameters
            var line = "";
            for (var index = 0; index < args.Length; index++)
            {
                var arg = args[index];
                var nextArg = index < args.Length - 1 ? args[index + 1] : null;
                nextArg = nextArg != null && nextArg.StartsWith("-", StringComparison.InvariantCulture) ? null : nextArg;
                if (nextArg != null)
                {
                    if (arg == "--param" || arg == "--params" || arg == "-p")
                    {
                        if (line.Length > 0)
                            line = line + ';';
                        line = line + nextArg;
                        index++;
                    }
                }
            }
            var parameters = ConfigParams.FromString(line);

            // Process environmental variables
            foreach (var key in Environment.GetEnvironmentVariables().Keys)
            {
                var name = key.ToString();
                var value = Environment.GetEnvironmentVariable(name);
                parameters.Set(name, value);
            }

            return parameters;
        }

        private bool ShowHelp(string[] args)
        {
            for (var index = 0; index < args.Length; index++)
            {
                var arg = args[index];
                if (arg == "--help" || arg == "-h")
                    return true;
            }
            return false;
        }

        private void PrintHelp()
        {
            Console.Out.WriteLine("Pip.Services process container - http://www.github.com/pip-services/pip-services");
            Console.Out.WriteLine("run [-h] [-c <config file>] [-p <param>=<value>]*");
        }

        //public object AppDomain { get; private set; }
        private void CaptureErrors(string correlationId)
        {
            //AppDomain.CurrentDomain.UnhandledException += (obj, e) =>
            //{
            //    _logger.Fatal(correlationId, e.ExceptionObject, "Process is terminated");
            //    _exitEvent.Set();
            //};
        }

        private void CaptureExit(string correlationId)
        {
            _logger.Info(correlationId, "Press Control-C to stop the microservice...");

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                _logger.Info(correlationId, "Goodbye!");

                eventArgs.Cancel = true;
                _exitEvent.Set();

                Environment.Exit(1);
            };

            // Wait and close
            _exitEvent.WaitOne();
        }

        public async Task RunAsync(string[] args)
        {
            if (ShowHelp(args))
            {
                PrintHelp();
                return;
            }

            var correlationId = _info.Name;
            var path = GetConfigPath(args);
            var parameters = GetParameters(args);
            this.ReadConfigFromFile(correlationId, path, parameters);

            CaptureErrors(correlationId);
            await OpenAsync(correlationId);
            CaptureExit(correlationId);
            await CloseAsync(correlationId);
        }

    }
}
