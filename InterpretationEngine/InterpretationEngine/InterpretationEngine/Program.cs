using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nav = Navigator;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

namespace InterpretationEngine
{

    class Bar
    {
        public string uri     { get; }
        public string value   { get; }
        public double height  { get; set; }

        public Bar(string uri, string value)
        {
            this.uri = uri;
            this.value = value;
        }
    }

    class BarChart
    {
        public string xAxis;
        public string yAxis;
        public List<Bar> bars;

        public BarChart()
        {

        }

        public BarChart(string xAxis, string yAxis)
        {
            this.xAxis = xAxis;
            this.yAxis = yAxis;
            this.bars = new List<Bar>();
        }
    }

    class Program
    {

        static BarChart chart;
		static BlobReader reader = new BlobReader();

        static string findValue(string uri)
        {
            if (uri.Length <= 0)
            {
                return "";
            }

            if (uri.Last() == '/')
            {
                return "";
            }

            return uri.Last() + findValue(uri.Substring(0, uri.Length - 1));
        }

        static List<Bar> DirectoryList(string directory)
        {
			string setName = "sets";
            List<Bar> outputList = new List<Bar>();

			foreach (var value in reader.blobContainer.ListBlobs(directory)) 
            {
                string filePath = value.Uri.LocalPath.Substring(setName.Length + 2);
				outputList.Add(new Bar(value.Uri.LocalPath.Substring(setName.Length + 2), findValue(filePath)));
            }

            return outputList;

        }

        static void calculateHeights(string request)
        {

			chart.bars = DirectoryList(request);
			Task[] tasks = new Task[chart.bars.Count];
			int counter = 0;

			foreach (Bar directory in chart.bars)
            {
				tasks[counter++] = Task.Factory.StartNew(() => setHeight(directory));
            }

			Task.WaitAll(tasks);

        }

		static void setHeight(Bar bar) {
			bar.height = reader.GetSetSize(bar.uri);
		}

        public static string SerializeObject<T>(T serializableObject)
        {
            Console.WriteLine("Attempting to seralize...");

            XmlSerializer xmlSerializer = new XmlSerializer(serializableObject.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, serializableObject);
                return textWriter.ToString();
            }

        }


        static void Main(string[] args)
        {




			// Dictionary<int, string> firstListRandom = inter.RetreiveList("C:/Sets/Students/STU$gcse$A/11");
			// Dictionary<int, string> secondListRandom= inter.RetreiveList("C:/Sets/Students/STU$student_stats$attendance/p_1");

			// Dictionary<int, string> firstListFull = reader.ReadRequest("C:/Sets/Students/STU$gcse$A/11");
			// Dictionary<int, string> secondListFull = reader.ReadRequest("C:/Sets/Students/STU$student_stats$attendance/p_1");
			//string request = "student_a";
			chart = new BarChart("Number of As", "Frequency");
			calculateHeights("C:/Sets/Students/STU$gcse$A/");

			foreach (Bar b in chart.bars) {
				Console.WriteLine(b.uri + " : " + b.height);
			}

			Console.WriteLine("Done");
			//BlobReader reader = new BlobReader();
			//Dictionary<int, string> normalSet = reader.GetSet("C:/Sets/Students/STU$gcse$A/5");
			//Console.WriteLine("Original set size  = " + normalSet.Count);
			//reader.GetSetSize("C:/Sets/Students/STU$gcse$A/5");
            Console.ReadKey(); 
        }
    }
}
