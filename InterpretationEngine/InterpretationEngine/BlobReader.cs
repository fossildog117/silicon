using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace InterpretationEngine
{
    public class BlobReader
    {

		public string blobConnString { get; }
		public CloudStorageAccount storageAccount { get; }
		public CloudBlobClient blobClient { get; }
		public CloudBlobContainer blobContainer { get; }

		public BlobReader() {
			this.blobConnString = ConfigurationManager.ConnectionStrings["azureStorageConnection"].ConnectionString;
			this.storageAccount = CloudStorageAccount.Parse(blobConnString);
			this.blobClient = storageAccount.CreateCloudBlobClient();
			this.blobContainer = blobClient.GetContainerReference("sets");
		}

		public MemoryStream DownloadData(string request) {
			
			MemoryStream memoryStream = new MemoryStream();

			try {
				blobContainer.GetBlockBlobReference(request).DownloadToStream(memoryStream);
			} catch {
				Console.WriteLine("Failed to download data from blob.");
			}

			return memoryStream;

		}

		public string GetJson(string request) {
			return Encoding.UTF8.GetString(DownloadData(request).ToArray());
		}


		//ms.Position = 0;
  		//var sr = new StreamReader(ms);
		//var myStr = sr.ReadToEnd();
		//Console.WriteLine(myStr);

		public int GetSetSize(string request) {
			MemoryStream s = DownloadData(request);
			s.Position = 0;
			int length = new StreamReader(s).ReadToEnd().Split(',').Length;
			return length > 0 ? length-1 : 0 ;
		}

        public Dictionary<int, string> GetSet(string request)
        {
            Dictionary<int, string> blobData = new Dictionary<int, string>();

			MemoryStream memoryStream = DownloadData(request);

            string data = Encoding.UTF8.GetString(memoryStream.ToArray());
            string[] values = data.Split(',');

            for (int i = 1; i < values.Length; i++)
            {
                string s = Regex.Replace(values[i], "\"|.*{|}.*", "");
                string[] pair = s.Split(':');
                blobData.Add(Int32.Parse(pair[0]), pair[1]);
            }

            return blobData;

        }
    }
}
