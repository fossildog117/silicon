using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE
{

    class Program
    {

		static Dictionary<int, string> GetSetMap(string request) {
			// string request = student_stats-attendance
			// student_stats-attendance : Sets/Student/student_stats/attendance

			BlobReader reader = new BlobReader();
			Dictionary<string, string> setMap = reader.GetMap();

			string filePath = setMap[request];

			Console.WriteLine(filePath);

			Dictionary<int, string> retDict = reader.GetSet(filePath);

			Console.WriteLine("==============");

			return retDict;
		}

        static void Main(string[] args)
        {

			//LineGraph bChart = GraphRetriever.GetLineGraph("C:/Sets/Students/STU$gcse$B/");

			//foreach (Bar b in bChart.bars) {
			//             Console.WriteLine(b.uri + " : " + b.height);
			//         }

			//         Console.WriteLine("Retrieved Line Graph");
			//Console.WriteLine("-------------------");

			Dictionary<int, string> mySet = GetSetMap("students-gender");

			Console.WriteLine("---------------");

			foreach (KeyValuePair<int, string> pair in mySet) {
				Console.WriteLine(pair.Key + " : " + pair.Value);
			}

			//var output = reader.GetJson("SetMap");

			//Console.WriteLine(output);

			//Console.ReadKey();
        }
    }
}
