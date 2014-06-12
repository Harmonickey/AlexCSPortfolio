using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Problem22
{
    class Program
    {
        static void Main(string[] args)
        {

            //first open and read file
            //while reading store into the list using a sorting algorithm
            //put each item into a function that calculates it's number
            //multiply by the place in the list

            //Then working out the alphabetical value for each name, multiply this value by its alphabetical position in the list to obtain a name score.

            //For example, when the list is sorted into alphabetical order, COLIN, which is worth 3 + 15 + 12 + 9 + 14 = 53, is the 938th name in the list. So, COLIN would obtain a score of 938  53 = 49714.

            //What is the total of all the name scores in the file?

            //use partitioning and quicksort
            //http://en.wikipedia.org/wiki/Quicksort

            string file = "names.txt";

            //cotains all our names from names.txt
            List<string> nameList = new List<string>();
            
            if (File.Exists(file))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    while (sr.Peek() > 0)
                    {
                        //read the whole file actually!
                        string line = sr.ReadLine();

                        //split up everything by the commas (quotation marks are kept, but okay)
                        string[] names = line.Split(',');
                        
                        //add each name to the list...
                        foreach (string name in names)
                            nameList.Add(name);

                        //now sort them all using quicksort (unstable but still okay since the names will not be the same)
                        nameList.Sort();
                     
                    }
                }

                double sum = 0;

                for (int i = 0; i < nameList.Count; i++)
                {
                    //mark the literal place in the array we're at
                    int place = i + 1;

                    //Remove the quotation marks
                    string actualName = nameList[i].Substring(1, nameList[i].Length - 2);

                    //get the values of the letters in the name and return sum
                    int value = GetValue(actualName);

                    //multiply the value by the place the name's at in the array
                    int multiplyByPlace = value * place;

                    //add the answer to the running sum
                    sum += multiplyByPlace;
                }

                Console.WriteLine(sum);

                Console.ReadLine();
            }
        }

        public static int GetValue(string name)
        {

            char[] letters = name.ToCharArray();
            int sum = 0;
            foreach (char letter in letters)
            {
                //take each letter and get it's 'alphabetical' numeral equivalent
                //A = 1  B = 2....
                //since all capitals, no need to do modulus 33
                int convert = (int)letter - 64;
                sum += convert;
            }

            return sum;

        }
    }
}
