using Bingo.Core;
using System.Diagnostics;

namespace Bingo.Cli
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Strip strip = null;
            for (var i = 0; i < 10000; i++)
            {
                strip = new Generator().Generate();
            }

            sw.Stop();
            Console.WriteLine($"czas {sw.ElapsedMilliseconds}");

            strip.Print();
        }
    }
}
