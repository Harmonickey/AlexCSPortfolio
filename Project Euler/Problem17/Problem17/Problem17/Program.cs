using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Problem17
{
    /*
     * If the numbers 1 to 5 are written out in words: one, two, three, four, five, then there are 3 + 3 + 5 + 4 + 4 = 19 letters used in total.
     *
     *  If all the numbers from 1 to 1000 (one thousand) inclusive were written out in words, how many letters would be used?
     *
     *  NOTE: Do not count spaces or hyphens. For example, 342 (three hundred and forty-two) contains 23 letters and 115 (one hundred and fifteen) 
     *  contains 20 letters. The use of "and" when writing out numbers is in compliance with British usage. 
     */



    enum Place
    {
        Ones,
        Tens,
        Hundreds,
        Thousands
    }

    class Program
    {
        static Place place = Place.Ones;

        static string[] ones = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        static string[] teens = { "null", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        static string[] tens = {"zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        static void Main(string[] args)
        {
            //do we keep going?
            bool keepGoing = true;
            
            //running sum
            double sum = 0;

            for (int number = 1; keepGoing == true; number++)
            {
                //when we're at the end then we need to stop
                if (number == 1000) keepGoing = false;

                //create the list of word for the number inputed
                List<string> words = OutputWords(number);
                
                foreach (string word in words)
                {
                    //convert the word to separate letters
                    char[] letters = word.ToCharArray();

                    //simply add in the length of the word to the running sum
                    sum += letters.Length;
                }
            }

            //don't feel like fixing the "zero" problem for the hundreds... so 4 * 9 = 36
            sum -= 36;
            Console.WriteLine("Answer: " + sum);

            Console.Read();
        }

        public static List<string> OutputWords(int input)
        {
            //get the string equivalent
            string inputString = input.ToString();

            //convert to separate parts
            char[] numberParts = inputString.ToCharArray();

            //the list of words to become the number
            List<string> wordParts = new List<string>();

            //we are starting at the ones place hence the for-loop format
            for (int i = numberParts.Length - 1; i >= 0; i--)
            {
                //we loop through, changing the place as we go along the number
                switch (place)
                {
                    case Place.Ones:
                        wordParts.Add(ones[int.Parse(numberParts[i].ToString())]); //add in the ones place
                        if (i > 0) //we're only concerned about advanced formatting if we have more places than 1 (indexed at 0)
                        {
                            //if we do have a larger number then we need to check if the ones place is actually a zero
                            if (wordParts[wordParts.Count - 1] == "zero")
                            {
                                //if so, then remove the word
                                wordParts.RemoveAt(wordParts.Count - 1);
                            }
                        }
                        break;
                    case Place.Tens:
                        wordParts.Add(tens[int.Parse(numberParts[i].ToString())]); //add in the tens place
                        if (i > 0) //we're only concerned if we have more than one more place by the time we get to tens
                        {
                            //if we do then we need to check if it's a zero
                            if (wordParts[wordParts.Count - 1] == "zero")
                            {
                                if (i == 1) //there is one more place to do so we replace "zero" with "and" to format with the ones place
                                {
                                    if (wordParts.Count == 2)
                                        wordParts[wordParts.Count - 1] = "and";
                                }
                                else //if not then we just remove the "zero"
                                {
                                    wordParts.RemoveAt(wordParts.Count - 1);
                                }
                            }
                            else //if not a zero then we need to check for "teens"
                            {
                                if (wordParts[wordParts.Count - 1] == "ten" && wordParts.Count >= 2)
                                {
                                    //specific formatting for the insertion of the word "--teen" instead of something like "ten and eight" 
                                    wordParts.RemoveAt(wordParts.Count - 1);
                                    wordParts.RemoveAt(wordParts.Count - 1);
                                    wordParts.Add(teens[int.Parse(numberParts[i + 1].ToString())]); //add in the teens
                                }

                                //then we still need an "and" for formatting
                                wordParts.Add("and");

                            }
                        }
                        else if (wordParts[wordParts.Count - 1] == "ten" && wordParts.Count >= 2) //even if this is the last place to check we still need to see if it's a "teen"
                        {
                            //specific formatting for the insertion of the word "--teen" instead of something like "ten and eight"
                            wordParts.RemoveAt(wordParts.Count - 1);
                            wordParts.RemoveAt(wordParts.Count - 1);
                            if (wordParts.Count == 1)
                                wordParts.RemoveAt(wordParts.Count - 1);
                            wordParts.Add(teens[int.Parse(numberParts[i + 1].ToString())]); //add in the teens
                        }

                        break;

                    case Place.Hundreds:
                        wordParts.Add("hundred"); //automatically needed everytime
                        wordParts.Add(ones[int.Parse(numberParts[i].ToString())]); //add in the prefix word (comes from 'ones' list)
                        
                        //if the hundreds place is actually a zero then do formatting
                        if (wordParts[wordParts.Count - 1] == "zero")
                        {
                            wordParts.RemoveAt(wordParts.Count - 1);
                            wordParts.RemoveAt(wordParts.Count - 1);
                            if (wordParts.Count == 1)
                                wordParts.RemoveAt(wordParts.Count - 1);
                        }
                        break;

                    case Place.Thousands:
                        wordParts.Add("thousand"); //there is only one time when this happens so nothing too fancy here
                        wordParts.Add(ones[int.Parse(numberParts[i].ToString())]);
                        break;
                }

                place++;

            }
            
            //put them in readable order
            wordParts.Reverse();

            //reinitialize place to do again
            place = Place.Ones;

            //return the list
            return wordParts;
        }
    }
}
