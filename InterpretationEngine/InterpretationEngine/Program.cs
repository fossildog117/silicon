using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace InterpretationEngine
{
	class Bar
	{
		public string uri { get; }
		public string value { get; }
		public double height { get; set; }

		public Bar(string uri, string value)
		{
			this.uri = uri;
			this.value = value;
		}
	}

	class Line
	{
		string name;
		List<Point> points;
	}

	class Point
	{

		double y;
		double x;

		public Point(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

	}

	class Titles {

		string xAxis;
		string yAxis;
		string title;

		public Titles(string xAxis, string yAxis, string title) {
			this.xAxis = xAxis;
			this.yAxis = yAxis;
			this.title = title;
		}

	}

	abstract class Chart {
		
		public Titles titles;
		public List<Bar> bars;
		public List<Line> lines;
	
	}

	class PieChart : Chart {
	
		public PieChart() {
			this.bars = new List<Bar>();
		}

	}


	class LineGraph : Chart {

		public LineGraph(string xAxis, string yAxis, string title = "") {
			this.titles = new Titles(xAxis, yAxis, title);
			lines = new List<Line>();
		}

		public void GenerateLines() {

			// function to get data
			// generate lines from data

		}

	}

	class BarChart : Chart
    {
        public BarChart(string xAxis, string yAxis, string title = "")
        {
			this.titles = new Titles(xAxis, yAxis, title);
			this.bars = new List<Bar>();
        }
    }

	class ChartEngine {

		public BlobReader reader { get; set;}
		public Chart chart { get; set; }

		public ChartEngine() {
			this.reader = new BlobReader();
		}

		private string findValue(string uri) {

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

		private List<Bar> DirectoryList(string directory) {

			string setName = "sets";
			List<Bar> outputList = new List<Bar>();

			foreach (var value in reader.blobContainer.ListBlobs(directory))
			{
				string filePath = value.Uri.LocalPath.Substring(setName.Length + 2);
				outputList.Add(new Bar(value.Uri.LocalPath.Substring(setName.Length + 2), findValue(filePath)));
			}

			return outputList;
		}

		public void calculateHeights(string request) {

			chart.bars = DirectoryList(request);
			Task[] tasks = new Task[chart.bars.Count];
			int counter = 0;

			foreach (Bar directory in chart.bars)
			{
				tasks[counter++] = Task.Factory.StartNew(() => setHeight(directory));
			}

			Task.WaitAll(tasks);

		}

		private void setHeight(Bar bar) {
			bar.height = reader.GetSetSize(bar.uri);
		}

	}

    class Program
	{
     
        static void Main(string[] args)
        {

			ChartEngine engine = new ChartEngine();
			// Dictionary<int, string> firstListRandom = inter.RetreiveList("C:/Sets/Students/STU$gcse$A/11");
			// Dictionary<int, string> secondListRandom= inter.RetreiveList("C:/Sets/Students/STU$student_stats$attendance/p_1");

			// Dictionary<int, string> firstListFull = reader.ReadRequest("C:/Sets/Students/STU$gcse$A/11");
			// Dictionary<int, string> secondListFull = reader.ReadRequest("C:/Sets/Students/STU$student_stats$attendance/p_1");
			//string request = "student_a";
			engine.chart = new BarChart("Number of As", "Frequency");
			engine.calculateHeights("C:/Sets/Students/STU$gcse$A/");

			foreach (Bar b in engine.chart.bars) {
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
