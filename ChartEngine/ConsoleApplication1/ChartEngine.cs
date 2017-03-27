using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace CE
{

    public class ChartEngine
    {

        public BlobReader reader { get; set; }
        public Chart chart { get; set; }

        public ChartEngine()
        {
            this.reader = new BlobReader();
        }

        private string findValue(string uri)
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

        private List<Bar> DirectoryList(string directory)
        {

            string setName = "sets";
            List<Bar> outputList = new List<Bar>();

			foreach (var value in reader.blobContainer.ListBlobs(directory))
			{
				string filePath = value.Uri.LocalPath.Substring(setName.Length + 2);
				if (!filePath.Contains("-size"))
				{
					outputList.Add(new Bar(value.Uri.LocalPath.Substring(setName.Length + 2), findValue(filePath)));
				}
			}

            return outputList;
        }

        public void calculateHeights(string request)
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

        private void setHeight(Bar bar)
        {
			bar.height = reader.GetSetSize(bar.uri, true);
        }
    }
}
