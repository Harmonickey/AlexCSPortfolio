using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem23
{
    class Program
    {
        static void Main(string[] args)
        {
            //first go through a list of numbers that is always going to be less than 28123
            //get all the divisors of that number, then add up the sum
            //check if that sum is greater than the number, then it is abundant, add it to the abundant list
            //then go through and find all the numbers under 28123 again, but this time
            //     try to add two numbers together to try and get that number
            //if you cannot find a sum, then add it to the list of numbers that cannot be written as a sum of two abundant numbers
            //then add up that list of numbers

            List<int> abundantNumbers = new List<int>();
            List<int> numbersWithNoSum = new List<int>();

            for (int i = 1; i <= 28123; i++)
            {
                //get all the divisors so we can compare to the original number
                List<int> divisors = GetDivisors(i);
                int sum = divisors.Sum();

                //if that sum of the divisors is greater, that means it's an abundant number
                if (sum > i)
                    abundantNumbers.Add(i);

            }

            bool sumFound = false;
            bool tooLarge = false;

            for (int i = 1; i <= 28123; i++)
            {
                //LINQ Method...
                //if (!abundantNumbers.TakeWhile(item => item < i)
                //    .Any(item => (abundantNumbers.Any(item2 => item + item2 == i))))
                //    numbersWithNoSum.Add(i);
                
                for (int j = 0; j < abundantNumbers.Count; j++)
                {
                    //no need to search further into the list if we're already too big...
                    if (abundantNumbers[j] >= i)
                    {
                        tooLarge = true;
                        break;
                    }

                    for (int k = 0; k < abundantNumbers.Count; k++)
                    {
                        
                        //if we do find a sum, then move on to the next number...
                        if (abundantNumbers[j] + abundantNumbers[k] == i)
                        {
                            sumFound = true;
                            break;
                        }

                    }
                    if (sumFound || tooLarge) break;
                }
                
                //if nothing was found then add to the final list
                //also, if the loop stopped while it found that the sum part(s) were too large, 
                //      then add that too because we didn't find a sum
                if (!sumFound || tooLarge) 
                    numbersWithNoSum.Add(i);
                //reset the flag
                sumFound = false;
                tooLarge = false;
            }

            int finalAnswer = numbersWithNoSum.Sum();

            Console.WriteLine(finalAnswer);
            Console.ReadLine();

        }

        static private List<int> GetDivisors(int number)
        {
            List<int> divisors = new List<int>();
            divisors.Add(1);
            for (int i = 2; i < (number / 2) + 1; i++)
            {
                if (number % i == 0)
                    divisors.Add(i);
            }

            return divisors;
        }
    }
}
