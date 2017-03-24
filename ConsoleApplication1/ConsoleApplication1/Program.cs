using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE
{

    class Program
    {

        static void Main(string[] args)
        {

			//LineGraph bChart = GraphRetriever.GetLineGraph("C:/Sets/Students/STU$gcse$B/");

			//foreach (Bar b in bChart.bars) {
			//             Console.WriteLine(b.uri + " : " + b.height);
			//         }

			//         Console.WriteLine("Retrieved Line Graph");
			//Console.WriteLine("-------------------");

			var map = GraphRetriever.GetMap();

			Dictionary<int, string> mySet = GraphRetriever.GetSetWithKey("student_stats-attendance");

			//Console.ReadKey();
        }
    }
}
