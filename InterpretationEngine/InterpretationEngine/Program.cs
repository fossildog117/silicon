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
            Interpreter interpreter = new Interpreter();
            Dictionary<int, string> first = interpreter.RetreiveList("0_school_city");
            Dictionary<int, string> second = interpreter.RetreiveList("0_school_region");

            foreach (KeyValuePair<int, string> pair in first)
            {
                Console.WriteLine(pair.Key + " : " + pair.Value);
            }


            Console.WriteLine("--------");
            Console.ReadKey();

            foreach (KeyValuePair<int, string> pair in second)
            {
                Console.WriteLine(pair.Key + " : " + pair.Value);
            }

            // var results = l1.Zip(l2, (x, y) => x + y);
            Console.ReadKey();
            Dictionary < int, string> resultDict = first.Keys.Intersect(second.Keys)
                              .ToDictionary(t => t, t => first[t]);


            foreach (KeyValuePair<int, string> pair in resultDict)
            {
                Console.WriteLine(pair.Key + " = " + pair.Value);
            }


            Console.ReadKey();
        }
    }
}
