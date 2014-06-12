using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Problem14
{
    class Program
    {
        /*
         * The following iterative sequence is defined for the set of positive integers:
         *
         *   n  n/2 (n is even)
         *   n  3n + 1 (n is odd)
         *
         *   Using the rule above and starting with 13, we generate the following sequence:
         *
         *   13  40  20  10  5  16  8  4  2  1
         *   It can be seen that this sequence (starting at 13 and finishing at 1) contains 10 terms. Although it has not been proved yet (Collatz Problem), it is thought that all starting numbers finish at 1.
         *
         *   Which starting number, under one million, produces the longest chain?
         *
         *   NOTE: Once the chain starts the terms are allowed to go above one million       
         */


        static void Main(string[] args)
        {

            double n = 1000000;  //testing number
            double startingNumber = 1000000;  //startingNumber
            double chain = 1;  //chain of numbers
            List<double> listOfChains = new List<double>();
            List<double> correspondingNumber = new List<double>();

            //following rules of Collatz Conjecture...
            do
            {
                //if even, divide by two, if not, multiply by 3 and add 1
                if ((n % 2) == 0)
                {
                    n /= 2;
                }
                else
                {
                    n = (3 * n) + 1;
                }

                //increment counter
                chain++;
                if (startingNumber == 2)
                {
                    //for debugging (?)
                }
                if (n == 1)
                {
                    //we found the end of the sequence
                    listOfChains.Add(chain);  //add the counter result to the list
                    correspondingNumber.Add(startingNumber);  //add the starting number that worked for that result
                    startingNumber--;   //decrease the starting number
                    n = startingNumber;  //reset n as the new starting number
                    chain = 0;  //reset the chain counter
                }

            } while (n > 1);

            //now get the max and match it up with the corresponding starting number
            double max = listOfChains.Max();
            int index = 0;
            for (int i = 0; i < listOfChains.Count; i++)
            {
                if (max == listOfChains[i])
                {
                    index = i;
                }

            }

            //write the length of the chain and which number produced it
            Console.WriteLine(listOfChains[index]);
            Console.WriteLine(correspondingNumber[index]);
            Console.Read();
        }
    }
}
