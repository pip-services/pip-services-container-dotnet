using System;
using System.Threading;

namespace PipServices.Container
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var process = new DummyProcess();
                process.RunAsync(args).Wait();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
