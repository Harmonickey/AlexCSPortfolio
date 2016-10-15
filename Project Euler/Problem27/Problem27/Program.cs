using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem27
{
    class Program
    {
        static List<int> otherPrimes = new List<int>() { 2, 3, 5, 7, 9 };

        static void Main(string[] args)
        {
            var answerNumberConsecutivePrimes = 0;
            var answerCoefficientProduct = 0;
            var checkedResults = new Dictionary<int, bool>();
            for (int a = -999; a < 1000; a++)
            {
                for (int b = -1000; b <= 1000; b++)
                {
                    var n = 0;
                    var numberConsecutivePrimes = 0;
                    var isPrime = true;
                    do
                    {
                        var result = GetAbsValQuadraticResult(n, a, b);

                        if (checkedResults.ContainsKey(result)) // check cache
                        {
                            if (!checkedResults[result]) break;
                            
                            numberConsecutivePrimes++;
                            if (numberConsecutivePrimes > answerNumberConsecutivePrimes)
                            {
                                answerNumberConsecutivePrimes = numberConsecutivePrimes;
                                answerCoefficientProduct = a * b;
                            }
                            n++;
                        }
                        else
                        {
                            // get all primes below the number we want to check
                            otherPrimes.GetOtherPrimes(result);

                            // check if result is prime
                            isPrime = result.IsPrime(otherPrimes);

                            checkedResults.Add(result, isPrime);

                            if (isPrime)
                            {
                                numberConsecutivePrimes++;
                                if (numberConsecutivePrimes > answerNumberConsecutivePrimes)
                                {
                                    answerNumberConsecutivePrimes = numberConsecutivePrimes;
                                    answerCoefficientProduct = a * b;
                                }
                                n++;
                            }
                        }
                    } while (isPrime);
                }
            }

            Console.WriteLine("AnswerCoefficientProduct: " + answerCoefficientProduct);
            Console.WriteLine("AnswerConsecutivePrimes: " + answerNumberConsecutivePrimes);
            Console.ReadLine();
        }

        static int GetAbsValQuadraticResult(int n, int a, int b)
        {
            // n^2 + an + b (two multiplications, two additions)
            // n(n + a) + b (one multiplications, two additions)
            return Math.Abs(n * (n + a) + b);
        }
    }
}
