using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Problem21
{
    class Program
    {
        /*
        Let d(n) be defined as the sum of proper divisors of n (numbers less than n which divide evenly into n).
        If d(a) = b and d(b) = a, where a  b, then a and b are an amicable pair and each of a and b are called amicable numbers.

        For example, the proper divisors of 220 are 1, 2, 4, 5, 10, 11, 20, 22, 44, 55 and 110; therefore d(220) = 284. The proper divisors of 284 are 1, 2, 4, 71 and 142; so d(284) = 220.

        Evaluate the sum of all the amicable numbers under 10000.
        */
        static void Main(string[] args)
        {
            //Go from 1 to 10000 with i
            //Get the sum of the divisors of i (sum1)

            //if i and sum1 aren't in amicableSumList
                //Get sum of divisiors of sum1 (sum2)
                //if sum2 = i
                //add to amicableSumList

            //From the project euler problem above we would have d(i) = sum1   Then    d(sum1) = sum2     Check if sum2 == i and we're golden!!

            double sum1;
            double sum2;
            int sqrtI, sqrtSum;
            List<double> amicableSumList = new List<double>();

            for (int i = 1; i < 10001; i++)
            {
                //reset the temp sums
                sum1 = 1;
                sum2 = 1;

                sqrtI = (int)Math.Sqrt(i);
                for (int j = 2; j <= sqrtI; j++)
                {
                    if (i % j == 0) //check if it's a divisor
                    {
                        sum1 += j; //add to the running sum of divisors of i
                        if (i / j > sqrtI) //since we only go to the square root of i we need to add in the other divisors!!
                            sum1 += i / j;
                    }
                }


                //does the list contain the current number or it's sum?  also check if the sum and the current number are equal...
                if (!(amicableSumList.Contains(i) || amicableSumList.Contains(sum1)) && sum1 != i)
                {

                    //same process as above
                    sqrtSum = (int)Math.Sqrt(sum1);

                    for (int k = 2; k <= sqrtSum; k++)
                    {
                        if (sum1 % k == 0)
                        {
                            sum2 += k;
                            if (sum1 / k > sqrtSum)
                                sum2 += sum1 / k;
                        }
                    }

                    //if we found an amicable number then sum2 should be equal to our original i
                    if (sum2 == i)
                    {
                        //add them both in
                        amicableSumList.Add(sum1);
                        amicableSumList.Add(i);
                    }
                }
            }

            Console.WriteLine(amicableSumList.Sum());
            Console.ReadLine();
        }
    }
}
