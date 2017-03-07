using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem29
{
    class Program
    {
        static void Main(string[] args)
        {
            List<List<KeyValuePair<int, int>>> solutionList = new List<List<KeyValuePair<int, int>>>();

            for (var a = 2; a <= 5; a++)
            {
                Console.WriteLine("a: " + a);
                // first get the prime factors of the number
                List<int> AprimeFactors = GetPrimeFactors(a);

                // now we can go through all the powers of the base
                for (var b = 2; b <= 5; b++)
                {
                    List<int> BprimeFactors = GetPrimeFactors(b);
                    // store a list of all the prime factors to their base as key value pairs
                    // prime factors of 20 => 2, 2, 5
                    // 20^8 = 2^8 * 2^8 * 5^8 = List((2,8),(2,8),(5,8))
                    int smallestBPrimeFactor = BprimeFactors[0];
                    BprimeFactors.RemoveAt(0);
                    int howManyToAdd = BprimeFactors.Aggregate(1, (acc, val) => acc * val);
                    List<KeyValuePair<int, int>> powerList = new List<KeyValuePair<int, int>>();
                    foreach (int AprimeFactor in AprimeFactors)
                    {
                        for (var i = 0; i < howManyToAdd; i++)
                        {
                            powerList.Add(new KeyValuePair<int, int>(AprimeFactor, smallestBPrimeFactor));
                        }
                    }

                    // now we need to check if the solutionList already contains the same powerList
                    // if it doesn't then we can add the powerList to the solutionList
                    bool foundOne = false;
                    foreach (List<KeyValuePair<int, int>> solutionPowerList in solutionList)
                    {
                        if (IsEqual(solutionPowerList, powerList))
                        {
                            foundOne = true;
                            break;
                        }
                    }
                    if (!foundOne)
                    {
                        // add to solution list
                        solutionList.Add(new List<KeyValuePair<int, int>>(powerList));
                    }
                }
            }

            Console.WriteLine(solutionList.Count);
            Console.ReadLine();
        }

        static bool IsEqual(List<KeyValuePair<int, int>> powerList1, List<KeyValuePair<int, int>> powerList2)
        {
            if (powerList1.Count != powerList2.Count)
            {
                return false;
            }

            for (var i = 0; i < powerList1.Count; i++)
            {
                if (powerList1[i].Key != powerList2[i].Key
                 || powerList1[i].Value != powerList2[i].Value)
                {
                    return false;
                }
            }

            return true;
        }

        static List<int> GetPrimeFactors(int a)
        {
            var primes = new List<int>();

            for (int div = 2; div <= a; div++)
            {
                while (a % div == 0)
                {
                    primes.Add(div);
                    a = a / div;
                }
            }
            return primes;
        }
    }
}
