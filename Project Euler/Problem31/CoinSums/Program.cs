using System;
using System.Collections.Generic;
using System.Linq;

namespace CoinSums
{
    class Program
    {
        static void Main()
        {
            IEnumerable<int> possibleCoinsToUse = new List<int> { 200, 100, 50, 20, 10, 5, 2, 1 };

            int totalPossible = TotalPossible(possibleCoinsToUse, 200, 200); // last coin and total starts at 200

            Console.WriteLine(totalPossible);
            Console.ReadLine();
        }

        static int TotalPossible(IEnumerable<int> possibleCoinsToUse, int totalLeft, int lastCoinUsed)
        {
            // only the possible coins based on the total left
            var newPossibleCoins = GetPossibleCoins(possibleCoinsToUse, totalLeft);

            if (newPossibleCoins.Count == 0 || totalLeft == 0) // no more total, **base case**
            {
                return 1;
            }

            return newPossibleCoins.Where(newCoin => newCoin <= lastCoinUsed) // only use coins that are smaller than the last coin used
                .Sum(newCoin => TotalPossible(newPossibleCoins, totalLeft - newCoin, newCoin)); // sum up all the below recursive calls
        }

        static List<int> GetPossibleCoins(IEnumerable<int> possibleCoins, int totalLeft)
        {
            var reverseOfCoins = new List<int> (possibleCoins);
            reverseOfCoins.Reverse();
            // grab all coins left that are still less than or equal to the total
            return reverseOfCoins.TakeWhile(c => c <= totalLeft).Reverse().ToList();
        }
    }
}
