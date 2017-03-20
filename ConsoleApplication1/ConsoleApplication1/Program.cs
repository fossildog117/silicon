using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterpretationEngine;

namespace CE
{

    class Program
    {
        static void Main(string[] args)
        {
            ChartEngine engine = new ChartEngine();

            engine.chart = new BarChart("Number of As", "Frequency");
            engine.calculateHeights("C:/Sets/Students/STU$gcse$A/");

            foreach (Bar b in engine.chart.bars)
            {
                Console.WriteLine(b.uri + " : " + b.height);
            }

            Console.WriteLine("Done");

            Console.ReadKey();
        }
    }
}
