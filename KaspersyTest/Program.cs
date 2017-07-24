using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaspersyTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var data = SampleData(100, 10);

			PrintResult(OptimizedGetEqualPairs(data, 15), data, 15);
			Console.ReadLine();
		}

		static void PrintResult(IEnumerable<Tuple<int, int>> results, int[] data, int X)
		{
			Console.WriteLine("Исходный массив:");

			for (int i = 0; i < data.Length; i++)
			{
				Console.WriteLine("{0}: {1}", i, data[i]);
			}
			Console.WriteLine("---------------------------------------------");
			Console.WriteLine("Пары чисел, сумма которых равна X={0}", X);

			foreach (var result in results)
			{
				Console.WriteLine("data[{0}]={1} и data[{2}]={3}", result.Item1, data[result.Item1], result.Item2, data[result.Item2]);
			}
		}

		static IEnumerable<Tuple<int, int>> SimpleGetEqualPairs(int[] arr, int X)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				for (int j = 0; j < arr.Length; j++)
				{
					if (i == j) continue;
					if (arr[i] + arr[j] == X) yield return new Tuple<int, int>(arr[i], arr[j]);
				}
			}
		}

		static IEnumerable<Tuple<int, int>> OptimizedGetEqualPairs(int[] arr, int X)
		{
			Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
			for (int i = 0; i < arr.Length; i++)
			{
				if (!dict.ContainsKey(arr[i]))
				{
					dict.Add(arr[i], new List<int>() { i });
				}
				else
				{
					dict[arr[i]].Add(i);
				}
			}

			for (int i = 0; i < arr.Length; i++)
			{
				int expected = X - arr[i];

				if (dict.ContainsKey(expected))
				{
					foreach (var expectedIndex in dict[expected])
					{
						if (expectedIndex != i)
						{
							yield return new Tuple<int, int>(i, expectedIndex);
						}
					}
				}
			}
		}

		static int[] SampleData(int count, int power)
		{
			var random = new Random();
			int[] result = new int[count];

			for (int i = 0; i < count; i++)
			{
				result[i] = random.Next(power);
			}
			return result;
		}
	}
}
