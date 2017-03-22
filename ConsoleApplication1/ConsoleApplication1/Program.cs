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

			LineGraph bChart = GraphRetriever.GetLineGraph("C:/Sets/Students/STU$gcse$B/");

			foreach (Bar b in bChart.bars) {
                Console.WriteLine(b.uri + " : " + b.height);
            }

            Console.WriteLine("Retrieved Line Graph");
			Console.WriteLine("-------------------");

			BarChart barChart = GraphRetriever.GetBarChart("C:/Sets/Students/STU$gcse$A/", "Number of As");

			foreach (Bar b in barChart.bars) {
				Console.WriteLine(b.uri + " : " + b.height);
			}

			Console.WriteLine("Retrieved Bar Chart");
			Console.WriteLine("-------------------");

			PieChart pieChart = GraphRetriever.GetPieChart("C:/Sets/Students/STU$gcse$C/");

			foreach (Bar b in pieChart.bars) {
				Console.WriteLine(b.uri + " : " + b.height);
			}

			Console.WriteLine("Retrieved Pie Chart");
			Console.WriteLine("-------------------");

            Console.ReadKey();
        }
    }
}
