using Bingo.Core;
using System.Diagnostics;

namespace Bingo.Cli
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Strip strip = null;

            

            //for (var i = 0; i < 1000; i++)
            //{
            //    //strip = new Generator().Generate();
            //}

            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Parallel.For(0, 10000, i =>
            //{
            //    var strip = new Generator().Generate();

            //});

            for (var i = 0; i < 10000; i++)
            {
                strip = new Generator().Generate();
            }

            sw.Stop();
            Console.WriteLine($"czas {sw.ElapsedMilliseconds}");

            //strip.Print();
        }
    }
}
