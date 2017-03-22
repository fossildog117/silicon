using System;
namespace CE
{
	public static class GraphRetriever
	{

		static ChartEngine engine = new ChartEngine();

		public static BarChart GetBarChart(string request, string title)
		{

			engine.chart = new BarChart(title, "Frequency");

			// request = C:/Sets/Students/STU$gcse$A/ for now
			engine.calculateHeights(request);

			return (BarChart)engine.chart;
		}

		public static PieChart GetPieChart(string request)
		{
			engine.chart = new PieChart();

			// request = C:/Sets/Students/STU$gcse$A/
			engine.calculateHeights(request);

			return (PieChart)engine.chart;
		}

		public static LineGraph GetLineGraph(string request, string xAxis = "", string yAxis = "", string title = "")
		{

			engine.chart = new LineGraph(xAxis, yAxis, title);

			// request = C:/Sets/Students/STU$gcse$A/
			engine.calculateHeights(request);

			return (LineGraph)engine.chart;
		}

	}
}

