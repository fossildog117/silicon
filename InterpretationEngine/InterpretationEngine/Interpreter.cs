using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nav = Navigator;
using System.IO;
using System.Text.RegularExpressions;

namespace InterpretationEngine
{

    class ReturnStructure
    {
        static readonly private int MaxSize = 1000;
        private double scalar;
        private Dictionary<int, string> points;

        public ReturnStructure(Dictionary<int, string> values)
        {
            double originalSize = values.Count;

            if (originalSize > MaxSize)
            {
                // Randomly select 10 000 data points
                Random generator = new Random();
                int randomNumber = 0;
                points = new Dictionary<int, string>();

                while (points.Count < MaxSize)
                {
                    try
                    {
                        randomNumber = GetRandomPoint(values, generator); // O(1) time complexity for random number
                        points.Add(values.ElementAt(randomNumber).Key, values.ElementAt(randomNumber).Value); 
                        // Using the random number (KEY) to get a VALUE is also O(1)
                        // Removing a Key Value pair from a dictionary is also O(1)
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                // Create the scalar
                this.scalar = originalSize / MaxSize;
            }
            else
            {
                this.points = values;
                scalar = 1;
            }
        }

        private int GetRandomPoint(Dictionary<int, string> values, Random numberGenerator)
        {
            return numberGenerator.Next(0, values.Count - 1);
        }

        public Dictionary<int, string> GetPoints()
        {
            return this.points;
        }

        public double GetScalar()
        {
            return this.scalar;
        }

    }

    public class Interpreter
    {

        static Nav.Navigator navigator = new Nav.Navigator();

        // Pass in GET Request here
        // 0_tableName_columnName
        public Dictionary<int, string> RetreiveList(string line)
        {
            // Interpret meaning of line
            //string filePath = getFile(InferListProperties(line), getID(line));
            string filePath = line;
            Console.WriteLine(filePath);

            // Read binary file that has been retrieved
            //BinaryReader reader = new BinaryReader(new FileStream(filePath, FileMode.Open));
            BlobReader reader = new BlobReader();

            // Create return structure from data set 
			return (new ReturnStructure(reader.GetSet(filePath))).GetPoints();
 
        }
        /*
        private string getFile(Tuple<string, string> data, int helperID)
        {
            string[] files;

            // Set correct file path
            switch (helperID)
            {
                case 0:
                    files = navigator.ShowSchools();
                    break;
                case 1:
                    files = navigator.ShowStudents();
                    break;
                default:
                    files = navigator.ShowSchools();
                    break;
            }

            // prioritise checking for equality
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);

                // split file into individual components
                string[] components = fileName.Split('$');

                // compare each component
                // component[0] = STU or SCH
                // component[1] = tableName or vice versa
                // component[2] = columnName or vice versa

                if (components[1].ToUpper().Equals(data.Item1.ToUpper()) && components[2].ToUpper().Equals(data.Item2.ToUpper()))
                {
                    return file;
                }

                if (components[2].ToUpper().Equals(data.Item1.ToUpper()) && components[1].ToUpper().Equals(data.Item2.ToUpper()))
                {
                    return file;
                }

            }

            // check if substrings available
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);

                // split file into individual components
                string[] components = fileName.Split('$');

                if (components[1].ToUpper().Contains(data.Item1.ToUpper()) && components[2].ToUpper().Contains(data.Item2.ToUpper()))
                {
                    return file;
                }

                if (components[2].ToUpper().Contains(data.Item1.ToUpper()) && components[1].ToUpper().Contains(data.Item2.ToUpper()))
                {
                    return file;
                }
            }

            return String.Empty;
        }

        // This funciton will return the second and third elements of a string following the format a_b_c_d_ ... _n
        // e.g. InterListProperties("a_b_c") ==> <"b", "c">
        // e.g. InterListProperties("I_like_chocolate") ==> <"like", "chocolate">
        public Tuple<string, string> InferListProperties(string line)
        {
            // Tuple<string, string>
            // is meant to be the equivalent of
            // Tuple<tableName, columnName>
            // Then we can just retrieve the column from a table using this infomation :)

            string[] values = line.Split('_');
            return Tuple.Create(values[1], values[2]) ;
        }

        // This funciton will return the first element of a string following the format a_b_c_d_ ... _n
        // e.g. getID("a_b_c") ==> a
        // e.g. getID("I_like_chocolate") ==> I
        public int getID(String line)
        {
            return Int32.Parse(line.Split('_')[0]);
        }
        */
    }
}
