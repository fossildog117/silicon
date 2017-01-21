using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Navigator
{
    public class Navigator
    {
        readonly String rootPath = @"C:\Sets\";
        readonly String schoolPath = @"C:\Sets\Schools\";
        readonly String studentPath = @"C:\Sets\Students\";

        public String[] ShowStudents()
        {
            return Directory.GetFiles(studentPath);
        }

        public String[] ShowSchools()
        {
            return Directory.GetFiles(schoolPath);
        }

        public String GetStudentFilePath()
        {
            return studentPath;
        }

        public String GetSchoolFilePath()
        {
            return schoolPath;
        }

    }

    class Program
    {


        static void Main(string[] args)
        {

            Navigator n = new Navigator();
            foreach (string s in n.ShowStudents())
            {
                Console.WriteLine(s);
            }
            Console.ReadKey();

        }
    }
}
