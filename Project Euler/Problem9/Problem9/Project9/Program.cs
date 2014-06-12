using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project9
{
    class Program
    {
        /*
         * A Pythagorean triplet is a set of three natural numbers, a  b  c, for which,
         *           a2 + b2 = c2
         *       For example, 32 + 42 = 9 + 16 = 25 = 52.
         * 
         * There exists exactly one Pythagorean triplet for which a + b + c = 1000.
         * Find the product abc.
         */

        static void Main(string[] args)
        {
            //store the variables a, b, c
            double a = 0, b = 0, c = 0;

            //we only need to check for a and b here, c comes from the result of a and b
            //therefore run two loops that start at 1 and go to 1000 respectively
            for (int i = 1; i < 1000; i++)
            {
                for (int j = 1; j < 1000; j++)
                {
                    //if they satisfy my reduced formula then stop
                    if (((1000 * i) + (1000 * j) - (i * j) - 500000) == 0)  //reduced form of combining the two eqns (product and sum eqns)
                    {
                        a = i;
                        b = j;
                        c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
                        break;
                    }
                }
            }

            //read off a, b, and c
            Console.WriteLine(a + " + " + b + " + " + c);
            Console.WriteLine("abc= " + (a * b * c));
            Console.ReadLine();

        }
    }
}
