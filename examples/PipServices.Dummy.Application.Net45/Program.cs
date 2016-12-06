using System;
using System.Threading;
using PipServices.Container;

namespace PipServices.Dummy.Application.Net45
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var task = (new DummyProcess()).RunAsync(args, CancellationToken.None);
                task.Wait();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
