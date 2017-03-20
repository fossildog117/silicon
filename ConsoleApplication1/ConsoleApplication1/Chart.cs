using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE
{
    public abstract class Chart
    {
        public Titles titles;
        public List<Bar> bars;
        public List<Line> lines;
    }

    public class PieChart : Chart
    {
        public PieChart()
        {
            this.bars = new List<Bar>();
        }
    }


    public class LineGraph : Chart
    {
        public LineGraph(string xAxis, string yAxis, string title = "")
        {
            this.titles = new Titles(xAxis, yAxis, title);
            this.bars = new List<Bar>();
        }
    }

    public class BarChart : Chart
    {
        public BarChart(string xAxis, string yAxis, string title = "")
        {
            this.titles = new Titles(xAxis, yAxis, title);
            this.bars = new List<Bar>();
        }
    }
}
