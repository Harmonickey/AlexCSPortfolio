using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Problem20
{
    class Program
    {
        /*
         * n! means n  (n  1)  ...  3  2  1

         * For example, 10! = 10  9  ...  3  2  1 = 3628800,
         * and the sum of the digits in the number 10! is 3 + 6 + 2 + 8 + 8 + 0 + 0 = 27.

         * Find the sum of the digits in the number 100!
         * 
         */

        static void Main(string[] args)
        {
            //Only need to do up to three digit multiplication in the 2nd product
            //adjustable loop to multiply 2nd product against the running factorial

            int product_seed = 100;
            string total = Large_Factorial(product_seed);

            List<int> total_parts = new List<int>();

            foreach (char part in total)
            {
                total_parts.Add(Convert.ToInt32(part.ToString()));
            }

            int sumOfParts = total_parts.Sum();

            Console.WriteLine(total);
            Console.WriteLine("Sum of Parts: " + sumOfParts);
            Console.Read();

        }

        static string Large_Factorial(int multiply_factor)
        {
            if (multiply_factor <= 1)
                return "1";
            return Multiply_String(Large_Factorial(multiply_factor - 1), multiply_factor);
        }

        static string Multiply_String(string factor1, int multiply_factor)
        {
            //factor2 will only be a max of 3 digits
            List<string> sum_list = new List<string>(); //all the parts of the multiplication that we add together at the end
            char[] factor1_parts = factor1.ToCharArray(); //separate all the parts of the first factor for separate multiplication
            List<int> factor1_parts_i = new List<int>();  //all the integer parts of factor1

            //create a list of only integer versions of the parts of factor1
            foreach (char factor_part in factor1_parts)
            {
                factor1_parts_i.Add(Convert.ToInt32(factor_part.ToString()));
            }

            factor1_parts_i.Reverse(); //need to do this reversed so that placeholding "0"s are inserted correctly
            int count = factor1_parts_i.Count; //the amount of numbers in factor1

            //Create a list of sum parts that come from multiplying each part of factor1 by the multiply_factor
            sum_list = CreateSumList(factor1_parts_i, multiply_factor, count);

            return Sum_String(sum_list);  //return the sum as a string

        }

        static string Sum_String(List<string> sum_parts)
        {
            string finalSum = "";  //this is to be the final string representing the summed up elements

            //we need lists of all the integers of the same places, and a list to hold all these lists
            List<List<int>> string_lists = new List<List<int>>();  

            //populate the overall list with an empty list at element [0]
            string_lists.Add(new List<int>());

            /*Populate each list in string_lists with respective places
             * With this algorithm the number  9560  has place representations as    9   5   6   0
             *                                                                      [3] [2] [1] [0]
             * For example... if we have this...
             * sum_parts = {0, 120, 600}
             * then we want this...
             * string_lists[0] = {0,   string_lists[1] = {2,      string_lists[2] = {1,              
             *                    0,                      0}                         6}
             *                    0}
             * Note: Each list will be summed and concatenated, 
             *       hence the vertical placement to show summing order (the concat gets reversed at the end...)
            */
            string_lists = PopulateStringLists(sum_parts, string_lists);
           
            //finally do the summation of each list, and then concatenate the sums into a finalSum
            finalSum = Sum_and_Concatenate(string_lists, finalSum);
            
            //just reverse the answer and return
            char[] finalSum_array = finalSum.ToCharArray();
            Array.Reverse(finalSum_array);
            finalSum = new string(finalSum_array);
            return finalSum;
        }

        static List<string> CreateSumList(List<int> factor1_parts, int multiply_factor, int count)
        {
            List<string> sum_list = new List<string>();

            for (int i = 0; i < count; i++)
            {
                int aProduct = factor1_parts[i] * multiply_factor;  //get a product by multiplying the multiply factor to the factor part

                string sum_item = aProduct.ToString();  //make it a string, it could be big

                for (int j = 0; j < i; j++)
                {
                    sum_item += "0"; //compensate by adding in placeholding zeros  (i.e. if we multiplied at [1] then we need to add one "0")
                }

                sum_list.Add(sum_item);  //add this sum part into a list of them, to be added together, finishing the mulitplication 
            }
            return sum_list;
        }

        static List<List<int>> PopulateStringLists(List<string> sum_parts, List<List<int>> string_lists)
        {
            foreach (string sum_part in sum_parts)
            {
                char[] sum_array = sum_part.ToCharArray();  //make an array of the sum_part

                int length = sum_array.Length; //get the length of the sum_array

                for (int i = 0; i < length; i++)
                {
                    //we may need to add a new list if another 'place' is necessary
                    if (string_lists.Count == i)
                    {
                        string_lists.Add(new List<int>());
                    }

                    //add each element into the right list, depending on it's "place"
                    string_lists[i].Add(Convert.ToInt32(sum_array[length - i - 1].ToString()));
                }
            }

            return string_lists;
        }

        static string Sum_and_Concatenate(List<List<int>> string_lists, string finalSum)
        {
            int carry_over = 0;  //initialize the carry over variable to hold all our carries when adding

            foreach (List<int> string_list in string_lists)
            {
                int sum = string_list.Sum();  //store the sum
                int sum_carry = sum + carry_over;  //add it to the possible carry

                string sum_string = sum_carry.ToString();  //take that answer and turn into a string 

                finalSum += sum_string.Substring(sum_string.Length - 1);  //add in only the last place into the final sum and...

                if (sum_string.Length - 1 != 0)
                    carry_over = Convert.ToInt32(sum_string.Substring(0, sum_string.Length - 1));  //the carry over is the rest of the string
                else
                    carry_over = 0;

            }

            if (carry_over != 0)
                finalSum += carry_over.ToString();  //add in the extra carry over if there was one

            return finalSum;
        }
    }
}
