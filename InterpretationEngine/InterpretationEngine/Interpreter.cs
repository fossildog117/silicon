﻿using System;
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
			this.ConfigureSet(values);
        }

		private void ConfigureSet(Dictionary<int, string> values) {

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
    }
}
