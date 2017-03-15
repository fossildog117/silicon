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

		public MemoryStream DownloadData(string request) {

			string blobConnString = ConfigurationManager.ConnectionStrings["azureStorageConnection"].ConnectionString;
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(blobConnString);
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
			CloudBlobContainer blobContainer = blobClient.GetContainerReference("sets");
			CloudBlockBlob block = blobContainer.GetBlockBlobReference(request);

			MemoryStream memoryStream = new MemoryStream();

			try {
				block.DownloadToStream(memoryStream);
			} catch {
				Console.WriteLine("Failed to download data from blob.");
			}

			return memoryStream;

		}

		public string GetJson(string request) {
			return Encoding.UTF8.GetString(DownloadData(request).ToArray());
		}

		public int GetSetSize(string request) {
			int length = Encoding.UTF8.GetString(DownloadData(request).ToArray()).Split(',').Length;
			return length > 0 ? length : 0 ;		
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
