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
        public BlobReader() { }
        public Dictionary<int, string> ReadRequest(string request)
        {

            Dictionary<int, string> blobData = new Dictionary<int, string>();
            string blobConnString = ConfigurationManager.ConnectionStrings["azureStorageConnection"].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(blobConnString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("sets");
            CloudBlockBlob block = blobContainer.GetBlockBlobReference(request);
        
            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    block.DownloadToStream(memoryStream);
                    string data = Encoding.UTF8.GetString(memoryStream.ToArray());
                    string[] values = data.Split(',');

                    for (int i = 1; i < values.Length - 1; i++)
                    {
                        string s = Regex.Replace(values[i], "\"", "");
                        string[] pair = Regex.Replace(s, @"\\t", ";").Split(';');
                        blobData.Add(Int32.Parse(pair[0]), pair[1]);
                    }

                } catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            return blobData;

        }
    }
}
