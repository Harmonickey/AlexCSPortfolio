using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Problem18
{
    class Program
    {
        static List<int> levels = new List<int>();

        static void Main(string[] args)
        {
            //The code I use to find files on the Desktop no matter from what location this program is at
            #region Find File
            Console.WriteLine("Enter name of file on Desktop : ");
            string fileName = Console.ReadLine();
            string path = Directory.GetCurrentDirectory();
            DirectoryInfo parent = Directory.GetParent(path);
            List<string> directories = new List<string>();
            
            //Keep stripping off the directory names and storing into a list until we get to the common directory name that most
            //windows operating systems have as "Users"
            while (parent.Name != "Users")
            {
                parent = parent.Parent;
                directories.Add(parent.Name);
            }
            int length = directories.Count;
            string filePath = "C:\\"; //we know that the filepath must start with this...
            
            //Append on the last two item of directories, which should be "Users" and your username 
            filePath += directories[length - 1] + "\\" + directories[length - 2] + "\\";

            //Then append on "Desktop" and the actual file name and it's extension
            string file = filePath + "\\Desktop\\" + fileName + ".txt";

#endregion

            List<List<int>> levelList = new List<List<int>>();

            //Sort all the numberes into lists cooresponding to each level in the pyramid
            if (File.Exists(file))
            {
                using (StreamReader sr = new StreamReader(file)) //read from the file to get the numbers...
                {
                    while (sr.Peek() > 0)
                    {
                        string line = sr.ReadLine();

                        string[] numbers = line.Split(' ');

                        List<int> numberList = new List<int>();
                        int number;

                        for (int i = 0; i < numbers.Length; i++)
                        {
                            number = Convert.ToInt32(numbers[i]);
                            numberList.Add(number);  //add the numbers to a list...
                        }

                        levelList.Add(numberList);  //add that list to become a new level
                    }
                }

                int result = TraverseTree(0, 0, levelList);  //start at the top most level and first index, so (0,0)
                Console.WriteLine(result);
            }

            //for (int i = 0; i < levels.Count; i++)
            //{
            //    Console.WriteLine("Level: " + i + " -> " + levels[i]);
            //}

            Console.ReadLine();

            
        }

        //Traverse the pyramid as if it were a binary tree
        static public int TraverseTree(int index, int level, List<List<int>> levelList)
        {
            if (level == levelList.Count)
            {
                return 0; //if we are at the bottom of the pyramid, return back 0 so it doesn't add up anything more when it recurses backward
            }
            //Go left in the pyramid
            int sum1 = TraverseTree(index, level+1, levelList) + levelList[level][index];

            //Go right in the pyramid
            int sum2 = TraverseTree(index+1, level+1, levelList) + levelList[level][index];

            return Math.Max(sum1, sum2);  //only return the max of the running sums to get the largest edge-weighted path
        }
    }
}
