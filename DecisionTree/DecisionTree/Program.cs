using System;
using System.Collections;
using System.Collections.Generic;

namespace DecisionTree
{
	class MainClass
	{

		public static void parse(Dictionary<int, string> inputSet) {

			HashSet<string> values = new HashSet<string>();
			Boolean allNumeric = true;
			double number;
			int numericThreshold = 20;
			double stringThreshold = 0.20; 

			foreach (KeyValuePair<int, string> pair in inputSet) {
				if (!values.Contains(pair.Value)) {
					values.Add(pair.Value);
				
				}

				if (!double.TryParse(pair.Value, out number)) {
					allNumeric = false;
				}

			}

			Console.WriteLine("Unique Values:" + values.Count);

			// If Set only has 2 unique elements
			if (values.Count <= 2) {
				
				Console.WriteLine("Boolean");

			// If set only has numeric values
			} else if (allNumeric) {

				// Numeric values
				Console.WriteLine("Numbers");

				// Check if unique sets can be created 
				if (values.Count < numericThreshold) {
					Console.WriteLine("Should be categorised");	
				} else {
					// Create sets based on percentile
					Console.WriteLine("Should be categorised based on percentage");
				}

			} else {

				// String values
				Console.WriteLine("Strings");

				Console.WriteLine("Ratio = " + ((double)values.Count / (double)inputSet.Count));

				if ((double)values.Count/(double)inputSet.Count < stringThreshold) {

					// Create unique sets
					Console.WriteLine("Create unique sets");

				} else {

					// Do nothing
					Console.WriteLine("Do nothing");

				}
			}
		}

		public static void Main(string[] args)
		{

			Dictionary<int, string> someSet = new Dictionary<int, string>();

			someSet.Add(20, "1");
			someSet.Add(30, "1");
			someSet.Add(40, "1");
			someSet.Add(50, "1");
			someSet.Add(60, "1");
			someSet.Add(70, "1");
			someSet.Add(80, "1");
			someSet.Add(90, "1");
			someSet.Add(100, "1");
			someSet.Add(110, "1");
			someSet.Add(1, "Nathan");
			someSet.Add(3, "Nathan");
			someSet.Add(4, "Nathan");
			someSet.Add(5, "Nathan");
			someSet.Add(6, "Nathan");
			someSet.Add(7, "Nathan");
			someSet.Add(11, "Nathan");
			someSet.Add(421, "Nathan");
			someSet.Add(43, "Nathan");
			someSet.Add(44, "Nathan");
			someSet.Add(45, "Nathan");
			someSet.Add(46, "Nathan");
			someSet.Add(47, "Nathan");
			someSet.Add(411, "Nathan");
			someSet.Add(31, "Haran");
			someSet.Add(41, "Haran");
			someSet.Add(51, "Haran");
			someSet.Add(61, "Haran");
			someSet.Add(71, "Haran");

			parse(someSet);

		}
	}
}
