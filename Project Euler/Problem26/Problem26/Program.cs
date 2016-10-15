using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Problem26
{
    class Program
    {
        private const int MAX_DIVIDE = 2000; // how many times we want to divide out the ratio
        private const float NUMERATOR = 1.0f; // always 1 since unit fractions

        static void Main(string[] args)
        {
            int answerLength = 6;  //largest we know from the problem set
            int answer = 7; // the d value
            List<int> result = new List<int>();
            List<int> answerResult = new List<int>();
            var timer = new Stopwatch();
            for (float d = 11.0f; d < 1000; d++)   // O(n)
            {
                timer.Restart();
                result = new List<int>();
                var length = GetDivisionResult(d, result);
                Console.WriteLine("Elapsed Time for {0}: {1} seconds", d, timer.ElapsedMilliseconds / 1000);
                timer.Stop();
                if (length == 0) continue;

                if (length > answerLength)
                {
                    answerLength = length;
                    answer = (int)d;
                    answerResult = result.ToList();
                }
            }

            Console.WriteLine("d: " + answer + " answerLength: " + answerLength);
            Console.Write("d: ");
            foreach(var res in answerResult)
            {
                Console.Write(res);
            }
            Console.ReadLine();
        }

        private static int GetDivisionResult(float d, List<int> result)
        {
            float numerator = NUMERATOR;
            int matchLength = 0;
            int answerLength = 0;
            bool found = false;
            int sameWholeNumber = 0;
            int numSingleCycles = 0;
            do
            {
                if (d > numerator) // set up for next iteration so it can divide out
                {
                    numerator *= 10;
                }
                else if (d < numerator) // now try to divide out
                {
                    var remainder = numerator % d;
                    if (remainder != 0)
                    {
                        var wholeNumber = (int)(numerator / d); // get the whole number part of dividing the numerator by divisor
                        numerator -= wholeNumber * d; // then subtract the multiplicative result to start again
                        
                        result.Add(wholeNumber); // next add to our list

                        if (wholeNumber == sameWholeNumber)
                        {
                            numSingleCycles++;
                            if (numSingleCycles == 10)
                                break;
                        }
                        else
                        {
                            numSingleCycles = 0;
                            sameWholeNumber = 0;
                        }

                        sameWholeNumber = wholeNumber;

                        // now check for a cycle!
                        var ending = 2;
                        var offset = 0;
                        matchLength = 0;
                        for (var j = 2; j < result.Count(); j++)
                        {
                            if (result[offset] == result[j])
                            {
                                if (offset == 0) // mark where the cycle would end
                                    ending = j;

                                matchLength++;
                                offset++;
                                    
                                if (offset == ending)
                                {
                                    answerLength = matchLength;
                                    found = true;
                                    break;
                                }
                            }
                            else
                            {
                                offset = 0;
                            }
                        }
                        if (found)
                        {
                            break;
                        }
                    }
                    else
                    {
                        result = null;
                        break;
                    }
                }
                else // it will divide out evenly, we don't care about these fractions
                {
                    result = null;
                    break;
                }
            } while (result.Count() <= MAX_DIVIDE);

            return answerLength;
        }
    }
}
