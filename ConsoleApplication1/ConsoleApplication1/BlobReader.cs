using System;
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

		public int GetSetSize(string request, bool marker = false)
		{
			return Int32.Parse(GetSetInfo(request, marker)["size"]);
		}

		public Dictionary<string, string> GetSetInfo(string request, bool marker = false) {

			Dictionary<string, string> blobData = new Dictionary<string, string>();

			string data;

			if (marker) {
				data = Encoding.UTF8.GetString(DownloadData(request + "-size"));
			} else {
				data = Encoding.UTF8.GetString(DownloadData(request));
			}

			string[] values = data.Split(',');

			for (int i = 0; i < values.Length; i++)
			{
				string s = Regex.Replace(values[i], "\"|.*{|}.*", "");
				string[] pair = s.Split(':');
				blobData.Add(pair[0], pair[1]);
			}

			return blobData;

		}

		public Dictionary<int, string> GetSet(string request)
		{
			Dictionary<int, string> blobData = new Dictionary<int, string>();

			string data = Encoding.UTF8.GetString(DownloadData(request));
			string[] values = data.Split(',');

			for (int i = 0; i < values.Length; i++)
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

			for (int i = 0; i < values.Length; i++)
			{
				string s = Regex.Replace(values[i], "\"|.*{|}.*", "");
				string[] pair = s.Split(':');
				blobData.Add(pair[0], pair[1]);
			}

			return blobData;

		}

	}

}

