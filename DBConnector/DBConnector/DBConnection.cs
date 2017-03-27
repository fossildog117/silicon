using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DBConnector
{
    public class DBConnection
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password; 

        //Constructor
        public DBConnection()
        {
            Initialize();
        }

        public DBConnection(string server, string database, string uid, string password)
        {
            Initialize(server, database, uid, password);
        }

        //Initialize values
        private void Initialize(string server = "127.0.0.1", string database = "database", string uid = "root", string password = "password")
        {
            this.server = server;
            this.database = database;
            this.uid = uid;
            this.password = password;

            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
            OpenConnection();
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<String> GetTables()
        {
            List<String> tables = new List<String>();
            try
            {
                String query = "show tables from " + database;
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    tables.Add(dataReader.GetString(0));
                }

                dataReader.Close();
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return tables;
        }


		// Get list of school IDs
		// Randomly select 400 school IDs and insert them into the sampleDB

		public void GetFile(String filename = "")
		{

			// select convert(na using utf8) from hello.new_table;

			try
			{

				int[] values = { 1, 2, 3, 4, 5, 6, 10, 11 };

				Task[] tasks = new Task[values.Length];
				int counter = 0;

				foreach (int val in values)
				{
					tasks[counter++] = Task.Factory.StartNew(() => printBarSize(val));
				}

				Task.WaitAll(tasks);
			} catch (Exception e) {
				Console.WriteLine(e);
			}
		}

        // Redundent function, should be depreciated ASAP
		public void printBarSize(int val) {

			string query = "select convert (na using utf8) from hello.new_table where idnew_table = " + val;

			MySqlCommand cmd = new MySqlCommand(query, new DBConnection().connection);
			MySqlDataReader reader = cmd.ExecuteReader();
			string output = String.Empty;
			while (reader.Read())
			{
				output += reader.GetString(0);
			}

			Console.WriteLine(output.Split(',').Length);

			reader.Close();

		}

        public void ExecuteNonQuery(String query)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
        }

        public void ExecuteNonQuery(String query, DBConnection mysqlConnection)
        {
            MySqlCommand command = mysqlConnection.connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }

        public string GetHost()
        {
            return this.server;
        }

        public string GetDatabaseName()
        {
            return this.database;
        }

        public string GetUsername()
        {
            return this.uid;
        }
    }
}
