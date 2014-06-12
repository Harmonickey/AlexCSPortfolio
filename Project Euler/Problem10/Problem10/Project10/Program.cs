using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project10
{
    class Program
    {
        /*
         * The sum of the primes below 10 is 2 + 3 + 5 + 7 = 17.
         * Find the sum of all the primes below two million.
         */

        static void Main(string[] args)
        {
            //a better way would have been to just keep a running sum instead of storing everything in a list...

            int primeNumberIndex = 2000000;  //how high we need to go
            bool hasFactor = false;

            //a list of all the primes we find
            List<double> primes = new List<double>();

            int i = 8;  //easiest starting point

            primes.Add(2);
            primes.Add(3);
            primes.Add(5);
            primes.Add(7);

            //use the same process as described in Problem 7
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
                                    foreach (int number in primes)
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
                                        primes.Add(i);
                                    }
                                }
                            }
                        }
                    }
                }

                i++;

            } while (i < primeNumberIndex);

            //now just get the sum of the primes found
            Console.Write(primes.Sum());
            Console.Read();

        }
    }
}
