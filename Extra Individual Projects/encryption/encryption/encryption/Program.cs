using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>();

            int loopedFindFile;
            do
            {
                loopedFindFile = 1;
                Console.Write("Enter the name of the text file " + Environment.NewLine + "on your desktop to be encrypted: ");

                #region Find File
                string fileName = Console.ReadLine();
                string path = Directory.GetCurrentDirectory();
                DirectoryInfo parent = Directory.GetParent(path);
                List<string> directories = new List<string>();
                while (parent.Name != "Users")
                {
                    parent = parent.Parent;
                    directories.Add(parent.Name);
                }
                int length = directories.Count;
                string encryptionFilePath = "C:\\";
                int whichOne = 0;
                for (int i = length - 1; i > -1; i--)
                {
                    if (directories[i] == "Users")
                    {
                        whichOne = i - 1;
                        encryptionFilePath = encryptionFilePath + directories[i] + "\\";
                        break;
                    }
                    encryptionFilePath = encryptionFilePath + directories[i] + "\\";
                }

                string encryptionFile = encryptionFilePath + directories[whichOne] + "\\Desktop\\" + fileName + ".txt";

                #endregion

                if (File.Exists(encryptionFile))
                {
                    
                    #region encryption

                    Console.Write("Enter k shift value: ");
                    int k = int.Parse(Console.ReadLine());
                    Console.WriteLine("Modulus will be 93");


                    using (StreamReader sr = new StreamReader(encryptionFile))
                    {
                        while (sr.Peek() > 0)
                        {
                            string line = sr.ReadLine();

                            string[] words = line.Split(' ');

                            List<char[]> wordList = new List<char[]>();
                            char[] word;

                            for (int i = 0; i < words.Length; i++)
                            {
                                word = words[i].ToCharArray();
                                wordList.Add(word);
                            }

                            string aline = String.Empty;

                            for (int i = 0; i < wordList.Count; i++)
                            {
                                for (int j = 0; j < wordList[i].Length; j++)
                                {
                                    //32 is the first character (space)
                                    int remainder = ((((int)wordList[i][j]) - 33) + k) % 93;  //calculate remainder through k shift
                                    wordList[i][j] = (char)(remainder + 33);     //convert to char again
                                    
                                    aline = aline + wordList[i][j];
                                }
                                aline = aline + " ";
                            }

                            Console.Write(Environment.NewLine);

                            lines.Add(aline);

                        }
                    }

                    //File.Delete(encryptionFile);
                    using (StreamWriter sw = new StreamWriter(encryptionFile))
                    {
                        for (int i = 0; i < lines.Count; i++)
                        {
                            sw.WriteLine(lines[i]);
                        }
                    }

                    #endregion

                    Console.Write("Press Enter to Exit...");
                    Console.Read();
                }
                else
                {
                    Console.WriteLine("ERROR: File does not exist");
                    Console.WriteLine("Please enter existing file name");
                    loopedFindFile = 0;
                }
            } while (loopedFindFile == 0);
        }
    }
}
