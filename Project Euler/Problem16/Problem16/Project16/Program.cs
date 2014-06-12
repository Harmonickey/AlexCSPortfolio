using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project16
{
    class Program
    {
        /*
         *  2^15 = 32768 and the sum of its digits is 3 + 2 + 7 + 6 + 8 = 26.
         *
         *  What is the sum of the digits of the number 2^1000?
         * 
         */

        static void Main(string[] args)
        {
            
            char[] powerNumber = new char[1];
            powerNumber[0] = '1';
            int iterations = 0;
            do
            {
                char[] nums = powerNumber;
                char carry = '0';
                List<char> charList = new List<char>();
                for (int i = nums.Length - 1; i >= 0; i--)
                {
                    //keep multiplying each part of the power number by 2
                    int iNum = Convert.ToInt32(nums[i].ToString());
                    iNum *= 2;
                    //add in the carry if there was one
                    iNum += Convert.ToInt32(carry.ToString());
                    carry = '0'; //reset the carry now
                    //if the number is greater than 9 then there must be carry
                    if (iNum > 9)
                    {
                        //remove the carry part and store it
                        char[] cNum = iNum.ToString().ToCharArray();
                        iNum = Convert.ToInt32(cNum[1].ToString());
                        carry = cNum[0];
                    }
                    //add the result in a character list to multiply with 2 again
                    charList.Add(iNum.ToString().ToCharArray()[0]);
                }
                //if the final operation had a carry, then add it in
                if (carry != '0') charList.Add(carry);
                charList.Reverse(); //since we used arrays, everything is reversed, so reverse it back
                powerNumber = new char[charList.Count];
                for (int j = 0; j < charList.Count; j++) powerNumber[j] = charList[j];
                charList.Clear();
                //Console.WriteLine(powerNumber);
                iterations++;

                //keep doing this until we have multiplyed the running number 1000 times by 2
            } while (iterations < 1000);

            //now sum up the individual parts of the array
            int totalSum = 0;
            foreach (char num in powerNumber)
            {
                totalSum += Convert.ToInt32(num.ToString());
            }

            //report the result
            Console.WriteLine(totalSum);
            Console.Read();
            
            
        }
    }
}
