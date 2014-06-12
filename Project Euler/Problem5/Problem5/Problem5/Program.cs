using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Problem5
{
    class Program
    {
        /*
         * 2520 is the smallest number that can be divided by each of the numbers from 1 to 10 without any remainder.
         * What is the smallest positive number that is evenly divisible by all of the numbers from 1 to 20?
         */


        static void Main(string[] args)
        {
            //we start with 20 as the first factor to check with 
            //because 20 is the largest factor we need to contend with
            int highestTestNumber = 20;

            //we start by testing if 1-20 go evenly into 20
            int testNumber = 20;
            bool divisible = false;

            do
            {
                //only need to check from 20 to 11 because all the numbers from 1-10 are half of 11-20
                //so... if the number is evenly divisible into 18, then it is also true for 9
                for (int i = 20; i > 10; i--)
                {
                    if ((testNumber % i) == 0)
                        divisible = true;
                    else
                    {
                        divisible = false;
                        break;  //stop here because we found one that wasn't divisible
                    }
                }
                //if we found one that wasn't divisible, then increase by 20 because we want to at least be evenly divisible by 20
                if (!divisible)
                {
                    testNumber += highestTestNumber;
                }

                /*  This is a modified version that may work faster...
                 *  for (int i = 20; i > 10; i--)
                 *  {
                 *      if ((testNumber % i) != 0)
                 *      {
                 *          divisible = false;
                 *          testNumber += highestTestNumber;
                 *          break;
                 *      }
                 *      else
                 *          divisible = true;
                 *  }
                 */

              //keep going until we find when we were able to check 20-11 without setting the divisible flag
            } while (!divisible);

            //read that number that passed the tests
            Console.Write(testNumber);
            Console.Read();

        }
    }
}
