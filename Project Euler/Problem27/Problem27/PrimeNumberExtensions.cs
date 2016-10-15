using System.Collections.Generic;
using System.Linq;

namespace Problem27
{
    public static class PrimeNumberExtensions
    {
        public static void GetOtherPrimes(this List<int> otherPrimes, int top)
        {
            for (var i = otherPrimes.Last() + 1; i < top / 2; i++)
            {
                bool hasFactor = false;
                foreach (int number in otherPrimes)
                {
                    if ((i % number) == 0)
                    {
                        hasFactor = true;
                        break;
                    }
                }
                if (hasFactor)
                {
                    continue;
                }
                else
                {
                    otherPrimes.Add(i);
                }
            }
        }

        public static bool IsPrime(this int n, List<int> otherPrimes)
        {
            var isPrime = true;
            if (n == 0 || n == 1) return isPrime;
            
            foreach (int number in otherPrimes)
            {
                if ((n % number) == 0 && n != number)
                {
                    isPrime = false;
                    break;
                }

            }

            return isPrime;
        }
    }
}
