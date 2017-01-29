using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.Common;
using System.Data;

namespace LinqTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DbProviderFactory dbf = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
                using (DbConnection dbcn = dbf.CreateConnection())
                {
                    dbcn.ConnectionString = "Server=localhost;Database=schools;Uid=root;Pwd=password;";
                    dbcn.Open();
                    using (DbCommand dbcmd = dbcn.CreateCommand())
                    {
                        dbcmd.CommandType = CommandType.Text;
                        dbcmd.CommandText = "SHOW TABLES;";
                        using (DbDataReader dbrdr = dbcmd.ExecuteReader())
                        {
                            while (dbrdr.Read())
                            {
                                Console.WriteLine(dbrdr[0]);
                            }
                        }
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadKey();
        }
    }
}
