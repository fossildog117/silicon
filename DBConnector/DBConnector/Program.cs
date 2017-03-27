using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnector
{
    class Program
    {
		static void Read() {

			string[] fileBytes = File.ReadAllLines("filepath");
			foreach (string s in fileBytes) {
				Console.WriteLine(s);
			}
			//StringBuilder sb = new StringBuilder();

			//foreach (byte b in fileBytes)
			//{
			//	sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
			//}
		
		}

        static void Main(string[] args)
        {
			//Read();
            DBConnection connection = new DBConnection();
			connection.GetFile();
            connection.CloseConnection();
            Console.ReadKey();
        }
    }
}
