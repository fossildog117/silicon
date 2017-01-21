using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            DBConnection connection = new DBConnection();
            connection.CloseConnection();
            Console.ReadKey();
        }
    }
}
