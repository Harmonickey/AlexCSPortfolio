using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Problem15
{
    class Program
    {
        /*
         * Starting in the top left corner of a 2x2 grid, there are 6 routes (without backtracking) to the bottom right corner.
         * 
         * How many routes are there through a 20x20 grid?
         */

        const int dimensions = 21;  //21 = 20x20 grid
       
        static double numPaths = 0;
        static double[,] matrix = new double[dimensions, dimensions];

        static void Main(string[] args)
        {
            
            
            int dimension = dimensions - 1;
            /*
            //Brute Force
            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    matrix[i, j] = 0;
                }
            }
            FindPaths(dimension, 0, 0);
            ShowFlags();

            Console.WriteLine(numPaths);
            Console.Read();
            Console.WriteLine();
            */

            //Using Pascal's Triangle
            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        matrix[i, j] = 1;  //set up all the sides of pascal's triangle
                    }
                    else
                    {
                        matrix[i, j] = matrix[i - 1,j] + matrix[i,j - 1]; //just add up the sides to get the next cooefficient in pascal's triangle
                    }
                }
            }

            Console.WriteLine(matrix[dimension,dimension]); //return the final middle piece in Pascal's Triangle to get the # of routes
            Console.ReadLine();

            
        }

        public static void FindPaths(int n, int x, int y)
        {
            if (x == n && y == n)
            {
                numPaths++;
                matrix[x, y] = (int)numPaths;
            }
            else if (x > n || y > n)
            {
                return;
            }
            else
            {
                if (x <= n && y <= n)
                {
                    matrix[x, y]++;
                }
                //ShowFlags();

                FindPaths(n, x + 1, y);
                FindPaths(n, x, y + 1);
            }
        }

        public static void ShowFlags()
        {
            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    Console.Write(" " + matrix[i, j]);
                }
                Console.Write(Environment.NewLine);
            }
            Console.Clear();
        }
    }
}
