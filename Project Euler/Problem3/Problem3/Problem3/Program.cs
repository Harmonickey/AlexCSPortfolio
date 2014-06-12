using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Problem3
{
    class Program
    {
        /*
         *  The prime factors of 13195 are 5, 7, 13 and 29.
         *  What is the largest prime factor of the number 600851475143 ? 
         */

        static void Main(string[] args)
        {
            //a better way would be to just keep a running max of the prime factors instead
            //of storing a large list of them and returning the final one

            //list of factors and primefactors
            List<double> factors = new List<double>();
            List<double> primeFactors = new List<double>();

            //the number we're getting the factors from
            double testNumber = 600851475143;

            //if the number divides evenly, add it to the list of factors
            for (double i = 2; i < (testNumber / 2); i++)
            {
                if ((testNumber % i) == 0)
                {
                    factors.Add(i);
                }
            }
            
            //same process as above but do it to each factor in the 'factors' list
            //if it is prime, then add it to the 'primeFactors' list
            foreach (double number in factors)
            {
                //keep a boolean to keep track of if it ended up having a factor or not
                bool hasFactor = false;
                for (double j = 2; j < (number / 2); j++)
                {
                    if ((number % j) == 0)
                    {
                        hasFactor = true;
                    }
                }
                if (!hasFactor)
                {
                    primeFactors.Add(number);
                }
            }

            //return the final prime factor because it's the largest
            Console.WriteLine(primeFactors.ElementAt(primeFactors.Count - 1));
            Console.Read();


        }
    }
}
