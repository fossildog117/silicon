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
    public class BlobReader
    {

        public string blobConnString { get; }
        public CloudStorageAccount storageAccount { get; }
        public CloudBlobClient blobClient { get; }
        public CloudBlobContainer blobContainer { get; }

        public BlobReader()
        {
            this.blobConnString = ConfigurationManager.ConnectionStrings["azureStorageConnection"].ConnectionString;
            this.storageAccount = CloudStorageAccount.Parse(blobConnString);
            this.blobClient = storageAccount.CreateCloudBlobClient();
            this.blobContainer = blobClient.GetContainerReference("sets");
        }

        public byte[] DownloadData(string request)
        {
			byte[] arrayOutput;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					blobContainer.GetBlockBlobReference(request).DownloadToStream(memoryStream);
				}
				catch
				{
					Console.WriteLine("Failed to download data from blob.");
				}
				finally 
				{
					memoryStream.Position = 0;
					arrayOutput = memoryStream.ToArray();
					memoryStream.Flush();
				}
			}

			return arrayOutput;

        }

		public string GetJson(string request)
        {
			string v = Encoding.UTF8.GetString(DownloadData(request));
			string s = Regex.Replace(v, ".*{", "{");
			string t = Regex.Replace(s, "}.*", "}");
			return t;
		}

        public int GetSetSize(string request)
        {          
			int length = GetJson(request).Split(',').Length;
            return length > 0 ? length - 1 : 0;
        }

        public Dictionary<int, string> GetSet(string request)
        {
            Dictionary<int, string> blobData = new Dictionary<int, string>();

			string data = Encoding.UTF8.GetString(DownloadData(request));
            string[] values = data.Split(',');

            for (int i = 1; i < values.Length; i++)
            {
                string s = Regex.Replace(values[i], "\"|.*{|}.*", "");
                string[] pair = s.Split(':');
                blobData.Add(Int32.Parse(pair[0]), pair[1]);
            }

            return blobData;

        }

		public Dictionary<string, string> GetMap()
		{
			Dictionary<string, string> blobData = new Dictionary<string, string>();

			string data = GetJson("SetMap");
			string[] values = data.Split(',');

			for (int i = 1; i < values.Length; i++)
			{
				string s = Regex.Replace(values[i], "\"|.*{|}.*", "");
				string[] pair = s.Split(':');
				blobData.Add(pair[0], pair[1]);
			}

			return blobData;

		}

    }

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
                outputList.Add(new Bar(value.Uri.LocalPath.Substring(setName.Length + 2), findValue(filePath)));
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
            bar.height = reader.GetSetSize(bar.uri);
        }
    }
}
