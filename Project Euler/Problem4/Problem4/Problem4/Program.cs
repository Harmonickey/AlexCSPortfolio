using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Problem4
{
    class Program
    {
        /*
         * A palindromic number reads the same both ways. The largest palindrome made from the product of two 2-digit numbers is 9009 = 91 99.
         * Find the largest palindrome made from the product of two 3-digit numbers.
         */ 

        static void Main(string[] args)
        {
            //keep a list of palindromes
            List<int> palindromes = new List<int>();

            //start at the highest three digit numbers first (will get biggest products first)
            int firstNumber = 999;
            int secondNumber = 999;

            //this is the character array that will be used to check if the number is a palidrome
            char[] resultSeparated;
            bool isPalindrome = true;

            do
            {
                //turn the result of the product into a character array
                resultSeparated = ((firstNumber * secondNumber).ToString()).ToCharArray();

                //check if it's a palindrome by checking equivalence to opposite ends of the array and moving inwards
                for (int i = 0; i < resultSeparated.Length / 2; i++)
                {
                    if (resultSeparated[i] != resultSeparated[(resultSeparated.Length - 1) - i])
                    {
                        isPalindrome = false;
                        break;  //break the for loop
                    }
                    else
                        isPalindrome = true;
                }

                //if it's a palindrome add it to the palindrome list for now
                if (isPalindrome)
                {
                    palindromes.Add(firstNumber * secondNumber);
                }
                
                //keep decreasing the first number until it reaches two digits
                firstNumber -= 1;
                if (firstNumber == 99)
                {
                    //then reset the first number and decrease the second only by 1
                    firstNumber = 999;
                    secondNumber -= 1;
                }

                //keep going until the second number reaches two digits
            } while (secondNumber > 100);

            //take the max of the palindromes
            int biggestPalindrome = palindromes.Max();

            Console.WriteLine(biggestPalindrome);
            Console.Read();
        }
    }
}
