using Chemistry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    class Program
    {
        private static int numIterations=100000000;

        static void Main(string[] args)
        {
            var elementH = new Element("H", 1, 1.007975);
            PeriodicTable.Add(elementH);
            elementH.AddIsotope(1, 1.00782503223, 0.999885);
            elementH.AddIsotope(2, 2.01410177812, 0.000115);

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Restart();
            for (int i = 0; i < numIterations; i++)
            {
                var a = PeriodicTable.GetElement(1);
                var b = a.Protons + a.AverageMass + 4;
            }
            stopWatch.Stop();
            Console.WriteLine("Time for getting by atomic number: "+stopWatch.Elapsed);

            stopWatch.Restart();
            for (int i = 0; i < numIterations; i++)
            {
                var a = PeriodicTable.GetElement("H");
                var b = a.Protons + a.AverageMass+4;
            }
            stopWatch.Stop();
            Console.WriteLine("Time for getting by atomic symbol: " + stopWatch.Elapsed);

            Console.ReadLine();
        }
    }
}
