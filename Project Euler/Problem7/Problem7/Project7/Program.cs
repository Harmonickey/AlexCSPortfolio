using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project7
{
    class Program
    {
        /*
         * By listing the first six prime numbers: 2, 3, 5, 7, 11, and 13, we can see that the 6th prime is 13.
         * What is the 10001st prime number?
         * 
         */

        static void Main(string[] args)
        {
            int count = 4;  //count to primeNumberIndex
            int targetPrime;  //the number itself
            int primeNumberIndex = 10001;  //how high we need to go
            bool hasFactor = false;
            List<int> otherPrimes = new List<int>();

            int i = 8;  //easiest starting point

            targetPrime = i;  //assign the targetprime to the test number
            otherPrimes.Add(2);
            otherPrimes.Add(3);
            otherPrimes.Add(5);
            otherPrimes.Add(7);

            /*
             * My idea is basically as follows...
             * If the number isn't divisible by 2, 3, 5, 7, 9, or previous primes 
             *    then it is prime.
             * Then add that number to the list of primes to check for more primes
             */ 
            
            do
            {

               if ((i % 2) != 0)
               {
                   if ((i % 3) != 0)
                   {
                       if ((i % 5) != 0)
                       {
                           if ((i % 7) != 0)
                           {
                               if ((i % 9) != 0)
                               {
                                   //here we're checking if the test number is divisible by any of the other primes
                                   foreach (int number in otherPrimes)
                                   {
                                       if ((i % number) == 0)
                                       {
                                           hasFactor = true;
                                           break;
                                       }
                                       
                                   }

                                   if (hasFactor)
                                   {
                                       hasFactor = false;
                                   }
                                   else
                                   {
                                       if (i - targetPrime == 6)
                                       {
                                           //for debugging...
                                           Console.Write(targetPrime);
                                           Console.Read();
                                       }

                                       //if there was no factor then we can add it to the list of primes
                                       count += 1;
                                       targetPrime = i; //store this while we check for other primes using 'i'
                                       otherPrimes.Add(i);
                                   }
                               }
                           }
                       }
                   }
               }

               i++;

               //keep going until the number of primes we found exceeds 10001 primes
            } while (count < primeNumberIndex);

            //read off that final target prime
            Console.Write(targetPrime);
            Console.Read();
        }
    }
}
