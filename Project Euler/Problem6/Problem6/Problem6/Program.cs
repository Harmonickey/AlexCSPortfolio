using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Problem6
{
    /*
     * Find the difference between the sum of the squares of the first one hundred natural numbers and the square of the sum.
     */

    class Program
    {
        //start at 100 to check for sum of squares
        static int upperBound = 100;
        static int difference = 0;

        static void Main(string[] args)
        {
            //go from 1 to 100
            for (int i = 1; i < upperBound + 1; i++)
            {
                //this does the sum of squares part
                difference += i * SumOfRest(i);
            }

            //this acccounts for the simplification of the mathematics done on paper
            difference *= 2;

            //read off the difference
            Console.Write(difference);
            Console.Read();

        }

        static int SumOfRest(int extra)
        {
            //this gets the sum of the numbers for the 'square of sums' part
            int tempSum = 0;
            for (int j = extra + 1; j < upperBound + 1; j++)
            {
                tempSum += j;
            }

            return tempSum;
        }
    }
}
