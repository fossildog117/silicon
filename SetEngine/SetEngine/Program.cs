using DBConnector;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace SetEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            DBConnection connection = new DBConnection();
            GenerateSets(connection);
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
                String primaryKeyColumn = String.Empty;
                while (dataReader.Read())
                {
                    if (dataReader.GetString(3).Equals("PRI"))
                    {
                        primaryKeyColumn = dataReader.GetString(0);
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
                    String statement = "select `" + primaryKeyColumn + "`, `" + column + "` from schools." + table + ";";
                    Console.WriteLine(statement);

                    // Set time out to something huge
                    MySqlCommand setTimers = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", connection);
                    setTimers.ExecuteNonQuery();

                    // Scoping problems for the finally statement
                    MySqlCommand command = null;
                    MySqlDataReader dataReader2 = null;

                    // Unique name of set 
                    // Defined by: ID - Column_name
                    String key = primaryKeyColumn + "-" + column;

                    try
                    {
                        // Get all values of the column
                        command = new MySqlCommand(statement, connection);
                        command.CommandTimeout = 0;
                        dataReader2 = command.ExecuteReader();

                        BinaryWriter file;

                        if (key.ToUpper().Contains("STUDENT"))
                        {
                            file = new BinaryWriter(new FileStream(navigator.GetStudentFilePath() + FilterSpecialCharacters(key), FileMode.Create));
                        } else
                        {
                            file = new BinaryWriter(new FileStream(navigator.GetSchoolFilePath() + FilterSpecialCharacters(key), FileMode.Create));
                        }

                        while (dataReader2.Read())
                        {
                            String output = dataReader2.GetString(0) + "\t" + dataReader2.GetString(1);
                            file.Write(output);
                        }
                        file.Close();
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
