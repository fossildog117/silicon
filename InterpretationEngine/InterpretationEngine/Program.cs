using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nav = Navigator;

namespace InterpretationEngine
{
    class Program
    {
        static void Main(string[] args)
        {

            Interpreter inter = new Interpreter();
            foreach( KeyValuePair<int, string> pair in inter.RetreiveList("0_school_public"))
            {
                Console.WriteLine(pair.Key + ": " + pair.Value);
            }

            Console.ReadKey(); 
        }
    }
}
