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

            string blobConnString = ConfigurationManager.ConnectionStrings["azureStorageConnection"].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(blobConnString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(setName);

            List<Bar> outputList = new List<Bar>();

            foreach (var value in container.ListBlobs(directory)) 
            {
                string filePath = value.Uri.LocalPath.Substring(setName.Length + 2);
                Console.WriteLine(filePath);  // for 2x '/' since local path is /sets/container_file
                outputList.Add(new Bar(value.Uri.LocalPath.Substring(setName.Length + 2), filePath));

            }

            return outputList;

        }

        static void calculateHeights(string request)
        {
            BlobReader reader = new BlobReader();
            
            foreach (Bar directory in DirectoryList(request))
            {
				directory.height = reader.GetSetSize(directory.uri);
            }
            
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
			Console.WriteLine("Done");
			//BlobReader reader = new BlobReader();
			//Dictionary<int, string> normalSet = reader.GetSet("C:/Sets/Students/STU$gcse$A/5");
			//Console.WriteLine("Original set size  = " + normalSet.Count);
			//reader.GetSetSize("C:/Sets/Students/STU$gcse$A/5");
            Console.ReadKey(); 
        }
    }
}
