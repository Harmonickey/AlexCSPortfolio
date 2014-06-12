using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem25
{
    class Program
    {
        static void Main(string[] args)
        {
            //0
            //1 + (0)
            //1 1 (1 + 1)
            //1 1 2 (1 + 2)

            string currentNumber = "1";
            string previousNumber = "0";
            int term = 1;

            string answer = FiboNumber(currentNumber, previousNumber, ref term);

            Console.WriteLine(term);
            Console.ReadLine();
        }

        static string FiboNumber(string curr, string prev, ref int term)
        {
            string fiboNumber = Add(curr,prev);  //add the numbers to get the next one
            term++;
            if (fiboNumber.Length == 1000) 
                return fiboNumber; //will stop the loop
            
            return FiboNumber(fiboNumber, curr, ref term); //the new one becomes the current and the old one becomes the previous
        }

        static string Add(string num1, string num2)
        {
            char[] num1_arr = num1.ToCharArray();
            char[] num2_arr = num2.ToCharArray();

            char[] first = new char[num1_arr.Length];
            char[] second = new char[first.Length];

            first = num1_arr.Reverse().ToArray<char>();
            char[] to_insert = num2_arr.Reverse().ToArray<char>();

            for (int i = 0; i < second.Length; i++)
            {
                if (i < to_insert.Length)
                    second[i] = to_insert[i];
                else
                    second[i] = '0';
            }

            //   875   +    623
            //   578   +    326
            //      5 + 3 = 8  no remainder
            //      7 + 2 = 9  no remainder
            //      8 + 6 = 4  1 as remainder

            string result = "";
            int carry = 0;

            for (int i = 0; i < first.Length; i++)
            {
                int digit = Convert.ToInt32(first[i].ToString()) + Convert.ToInt32(second[i].ToString()) + carry;
                carry = 0;
                               
                if (digit > 9)
                {
                    carry = Convert.ToInt32(digit.ToString().ToCharArray()[0].ToString());
                    digit = Convert.ToInt32(digit.ToString().ToCharArray()[1].ToString());
                    result += digit.ToString();
                }
                else
                {
                    result += digit.ToString();
                }
            }

            if (carry > 0)
                result += carry.ToString();

            string answer = new string(result.Reverse().ToArray<char>());
            return answer;
        }

    }
}
