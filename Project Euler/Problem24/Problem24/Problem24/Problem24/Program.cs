using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Problem24
{
    class Permute
    {
        List<string> permutationList = new List<string>();

        private void swap(ref char a, ref char b)
        {
            if (a == b) return;
            a ^= b;
            b ^= a;
            a ^= b;
        }

        public List<string> setper(char[] list)
        {
            int x = list.Length - 1;
            go(list, 0, x);

            return permutationList;
        }

        private void go(char[] list, int k, int m)
        {
            int i;
            if (k == m)
            {
                char[] insertion = new char[list.Length];
                Array.Copy(list, insertion, list.Length);
                permutationList.Add(new string(insertion));
            }
            else
                for (i = k; i <= m; i++)
                {
                    swap(ref list[k], ref list[i]);
                    go(list, k + 1, m);
                    swap(ref list[k], ref list[i]);
                }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            //create permutations in lexicographic order... 

            int whichPermutation = 0;

            char[] permutationTemplate = {'9','8','7','6','5','4','3','2','1','0'};
            List<string> permutations = new List<string>();
            permutations.Add(new string(permutationTemplate));
            whichPermutation++;
            permutations.Add("8976543210");
            whichPermutation++;
            int nextPosition = 3;
            Permute p;
            
            //StreamWriter sw = new StreamWriter("output");

            while (nextPosition <= permutationTemplate.Length + 1)
            {

                char[] set_firstHalf = GetSet_FirstHalf(nextPosition, permutationTemplate);
                char reservedNumber = set_firstHalf[set_firstHalf.Length - 1];

                //List<char[]> permutationList = GetPermutations(set_firstHalf);
                p = new Permute();
                /*calling the permute*/
                List<string> listOfPermutations = new List<string>();
                listOfPermutations = p.setper(set_firstHalf);
                listOfPermutations.Sort();

                foreach (string permutation in listOfPermutations)
                {
                    if (permutation[0] != reservedNumber)
                    {
                        string insertion = new string(permutation.Reverse().ToArray<char>());

                        if (nextPosition != 11)
                        {
                            char[] set_secondHalf = GetSet_SecondHalf(nextPosition, permutationTemplate);
                            char[] combined = insertion.Union(set_secondHalf).ToArray<char>();
                            //sw.WriteLine(combined.Reverse().ToArray<char>());
                            if (!permutations.Contains(new string(combined)))
                            {
                                whichPermutation++;
                                permutations.Add(new string(combined));
                            }
                        }
                        else
                        {
                            //sw.WriteLine(insertion.Reverse().ToArray<char>());
                            if (!permutations.Contains(insertion))
                            {
                                whichPermutation++;
                                permutations.Add(insertion);
                            }
                        }

                        if (whichPermutation == 1000000)
                        {
                            Console.WriteLine(permutations[whichPermutation - 1]);
                            Console.ReadLine();
                        }
                    }

                }
                //sort
                //for each permutation in permutation list, only take the ones that don't end in reserved number

                /* Stuff
                
                int outsideOffSet = 0;
                int insideOffSet = 0;
                bool once = true;
                bool many = false;

                

                while (outsideOffSet < set_firstHalf.Length)
                {
                    while (insideOffSet < set_firstHalf.Length - 1)
                    {
                        for (int i = 0; i < set_firstHalf.Length; i++)
                        {
                            for (int j = i; j < set_firstHalf.Length; j++)
                            {
                                //do flips   
                                char[] permutation = new char[set_firstHalf.Length];
                                if (many)
                                    Array.Copy(set_firstHalf, permutation, set_firstHalf.Length);
                                for (int n = 0; n <= outsideOffSet; n++)
                                    for (int p = 0; p <= insideOffSet; p++)
                                        if (j + p < set_firstHalf.Length && i + n < set_firstHalf.Length)
                                        {
                                            if (once)
                                                permutation = FlipNumbers(i + n, j + p, set_firstHalf);
                                            if (many)
                                                permutation = FlipNumbers(i + n, j + p, permutation);
                                        }

                                if (permutation[set_firstHalf.Length - 1] != reservedNumber)
                                {
                                    //TRY PERMUTING BY ONE FLIP, THEN ROTATE IT ALL THE WAY AROUND, CHECKING SAME THINGS EACH TIME

                                    string insertion = new string(permutation.Reverse().ToArray<char>());
                                    if (!listOfPermutations.Contains(insertion))
                                        listOfPermutations.Add(insertion);
                                }

                                char[] rotationPermutation = new char[permutation.Length];
                                Array.Copy(permutation, rotationPermutation, permutation.Length);

                                //do rotations, k amount of times   if length is 3, rotate 2 times
                                for (int k = 1; k < set_firstHalf.Length; k++)
                                {
                                    rotationPermutation = RotateNumbers(k, permutation);
                                    if (rotationPermutation[set_firstHalf.Length - 1] != reservedNumber)
                                    {
                                        string insertion = new string(rotationPermutation.Reverse().ToArray<char>());
                                        if (!listOfPermutations.Contains(insertion))
                                            listOfPermutations.Add(insertion);

                                    }
                                }
                            }
                        }
                        insideOffSet++;
                        many = true;
                        once = false;
                    }
                    outsideOffSet++;
                    insideOffSet = 0;
                }

                many = false;
                once = true;

                listOfPermutations.Sort();

                foreach (string permutation in listOfPermutations)
                {
                    string insertion = new string(permutation.Reverse().ToArray<char>());

                    if (nextPosition != 11)
                    {
                        char[] set_secondHalf = GetSet_SecondHalf(nextPosition, permutationTemplate);
                        char[] combined = insertion.Union(set_secondHalf).ToArray<char>();
                        //sw.WriteLine(combined.Reverse().ToArray<char>());
                        if (!permutations.Contains(new string(combined)))
                            permutations.Add(new string(combined));
                    }
                    else
                    {
                        //sw.WriteLine(insertion.Reverse().ToArray<char>());
                        if (!permutation.Contains(insertion))
                            permutations.Add(insertion);
                    }
                    whichPermutation++;
                    if (whichPermutation == 1000000)
                    {
                        Console.WriteLine(permutations[whichPermutation - 1]);
                        Console.ReadLine();
                    }
                }

                listOfPermutations.Clear();
                */

                nextPosition++;
            }

            Console.WriteLine(whichPermutation);
            Console.WriteLine(permutations[whichPermutation - 1]);
            Console.ReadLine();
        }

        private static char[] GetSet_FirstHalf(int position, char[] template)
        {
            char[] set = template.Take(position).ToArray<char>();
            return set;
        }

        private static char[] GetSet_SecondHalf(int position, char[] template)
        {
            char[] set = new char[template.Length - position];
            for (int i = position; i < template.Length; i++)
            {
                set[i - position] = template[i];
            }

            return set;
        }

        private static char[] RotateNumbers(int amount, char[] template)
        {
            char[] set = new char[template.Length];
            Array.Copy(template, set, template.Length);

            for (int i = 0; i < amount; i++)
            {
                for (int j = set.Length - 1; j > 0; j--)
                {
                    char numberToMove = set[j];

                    set[j] = set[j - 1];

                    set[j - 1] = numberToMove;
                }
            }

            return set;
        }

        private static char[] MoveNumber(int position, int howMuch, char[] permutationTemplate)
        {
            char[] template = permutationTemplate;

            for (int i = position; i < howMuch + position; i++)
            {
                char numberToMove = template[position];

                template[position] = template[position + 1];

                template[position + 1] = numberToMove;
            }

            return template;

        }

        private static char[] FlipNumbers(int position, int position2, char[] permutationTemplate)
        {
            char[] template = new char[permutationTemplate.Length];
            Array.Copy(permutationTemplate, template, permutationTemplate.Length);

            char number1 = template[position];
            char number2 = template[position2];

            template[position] = number2;
            template[position2] = number1;

            return template;
        }
    }
}
