using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem28
{
    class Program
    {
        static void Main(string[] args)
        {
            var overallSum = 0;
            var x = 1001;
            var y = 1001;

            // top left to bottom right
            for (int i = 1, count = 0; count < x; i += count * 2)
            {
                overallSum += i;
                count++;
            }

            // bottom left to top right
            var isFirst = true;
            for (int i = 1, count = 0; count < Math.Round((double)y / 2, MidpointRounding.AwayFromZero); i += GetNext(i, y, ref isFirst, ref count))
            {
                overallSum += i;
            }

            overallSum -= 1; // because we double counted from the intersection of the diagonals

            Console.WriteLine("Sum: " + overallSum);
            Console.ReadLine();
        }

        private static int GetNext(int i, int y, ref bool isFirst, ref int count)
        {
            if (isFirst) // need to run this two times for the second diagonal
            {
                isFirst = false;
                count++;
            }
            else
            {
                isFirst = true;
            }
            
            return count * 4; // now we can 
        }
    }
}
