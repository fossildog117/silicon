using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using DBConnector;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace SetEngine
{

    class BlobPusher
    {
        public BlobPusher() { }
        public void push(Dictionary<int, string> objToPush, string filePath)
        {
            string blobConnString = ConfigurationManager.ConnectionStrings["azureStorageConnection"].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(blobConnString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("sets");
            blobContainer.CreateIfNotExists();

            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();

            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(filePath);

            var binFormatterSize = new BinaryFormatter();
            var mStreamSize = new MemoryStream();
            CloudBlockBlob setSize = blobContainer.GetBlockBlobReference(filePath + "-size");

            try
            {
                string json = JsonConvert.SerializeObject(objToPush);
                binFormatter.Serialize(mStream, json);
                mStream.Position = 0;
                blob.UploadFromStream(mStream);

                string setSizeJson = JsonConvert.SerializeObject(GenerateSetInformaiton(objToPush));
                binFormatterSize.Serialize(mStreamSize, setSizeJson);
                mStreamSize.Position = 0;
                setSize.UploadFromStream(mStreamSize);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void pushMap(Dictionary<string, string> objToPush, string setMap = "SetMap")
        {
            string blobConnString = ConfigurationManager.ConnectionStrings["azureStorageConnection"].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(blobConnString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("sets");
            blobContainer.CreateIfNotExists();

            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();

            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(setMap);
            
            try
            {
                string json = JsonConvert.SerializeObject(objToPush);
                binFormatter.Serialize(mStream, json);
                mStream.Position = 0;
                blob.UploadFromStream(mStream);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private string setSize(Dictionary<int, string> inputSet)
        {
            return inputSet.Count.ToString();
        }

        private Dictionary<string, string> GenerateSetInformaiton(Dictionary<int, string> inputSet)
        {
            Dictionary<string, string> setInformaiton = new Dictionary<string, string>();
            setInformaiton["size"] = setSize(inputSet);

            return setInformaiton;
        }


    }

    class Program
    {

        static BlobPusher blobPusher = new BlobPusher();
        static Dictionary<string, string> setMappings = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            DBConnection connection = new DBConnection();
            GenerateSets(connection);
            blobPusher.pushMap(setMappings);
            Console.WriteLine("Program finished, press any key to continue...");
            Console.ReadKey();

        }

        static void GenerateSets(DBConnection conn)
        {

            List<String> tables = conn.GetTables();
            Navigator.Navigator navigator = new Navigator.Navigator();

            MySqlConnection connection = conn.GetConnection();

            //show columns from schools.school;
            foreach (String table in tables)
            {

                // show keys from schools.school where Key_name = 'PRIMARY';

                String query = "show columns from schools." + table;

                // Get List of columns for each table
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                // Put the columns into a List
                // If column is primary key assign a separate variable to it
                List<String> columns = new List<String>();
                string primaryKey = String.Empty;
                while (dataReader.Read())
                {
                    if (dataReader.GetString(3).Equals("PRI"))
                    {
                        primaryKey = dataReader.GetString(0);
                    }
                    else
                    {
                        columns.Add(dataReader.GetString(0));
                    }
                }

                dataReader.Close();

                // Iterate through each column with the primary key (id) column
                foreach (String column in columns)
                {
                    //select idstudent, year from schools.gcse;
                    string statement = "select `" + primaryKey + "`, `" + column + "` from schools." + table + ";";

                    Console.WriteLine(statement);

                    // Set time out to something huge
                    MySqlCommand setTimers = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", connection);
                    setTimers.ExecuteNonQuery();

                    // Scoping problems for the finally statement
                    MySqlCommand command = null;
                    MySqlDataReader dataReader2 = null;

                    // Unique name of set 
                    // Defined by: ID - Column_name
                    String key = primaryKey + "-" + column;

                    try
                    {
                        // Get all values of the column
                        command = new MySqlCommand(statement, connection);
                        command.CommandTimeout = 0;
                        dataReader2 = command.ExecuteReader();

                        BinaryWriter file;

                        string path;
                        Dictionary<int, string> outputDictionary = new Dictionary<int, string>();
                        
                        if (key.ToUpper().Contains("STUDENT"))
                        {
                            path = "Sets/Student/" + table + "/" + column;
                        } else
                        {
                            path = "Sets/School/" + table + "/" + column;
                        }

                        setMappings.Add(table + "-" + column, path);

                        while (dataReader2.Read())
                        {
                            int key1 = dataReader2.GetInt32(0);
                            string value1 = dataReader2.GetString(1);
                            String output = key1 + "\t" + value1;
                            outputDictionary.Add(key1, value1);
                        }

                        Task[] tasks = new Task[2];
                        tasks[0] = Task.Factory.StartNew(() => blobPusher.push(outputDictionary, path));
                        tasks[1] = Task.Factory.StartNew(() => DecisionTree.parse(outputDictionary, path, blobPusher));

                        Task.WaitAll(tasks);

                        dataReader2.Close();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    finally
                    {
                        if (dataReader2 != null)
                        {
                            dataReader2.Close();
                        }
                    }
                }
            }
        }

        static String FilterSpecialCharacters(String line)
        {
            line = line.Replace('*', 'i');
            line = line.Replace('\\', 'i');
            return line;
        }
    }
}
