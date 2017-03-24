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


			BlobReader reader = new BlobReader();
			var outputSet = reader.GetSet("Sets/School/school/public");

			foreach (var pair in outputSet) {
				Console.WriteLine(pair.Key + " : " + pair.Value);
			}

			var size = reader.GetSetSize("Sets/School/school/public");
			Console.WriteLine("Size: " + size);

			var output = reader.GetJson("Sets/School/school/public/1");

			Console.WriteLine(output);
            Console.ReadKey();
        }
    }
}
