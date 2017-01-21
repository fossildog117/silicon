using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

        //Initialize values
        private void Initialize()
        {
            server = "127.0.0.1";
            database = "schools";
            uid = "root";
            password = "0x38be";

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
            String query = "show tables from schools";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            List<String> tables = new List<String>();

            while (dataReader.Read())
            {
                tables.Add(dataReader.GetString(0));
            }

            dataReader.Close();

            return tables;
        }


        // Get list of school IDs
        // Randomly select 400 school IDs and insert them into the sampleDB

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
    }
}
