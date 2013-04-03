using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Encryption_Decryption_Program
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void ChooseFolder(TextBox textBox)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = openFileDialog1.FileName;
            }
        }

        private void fileSearch_Click(object sender, EventArgs e)
        {
            ChooseFolder(fileBox_Encrypt);
        }

        private void fileSearch_2_Click(object sender, EventArgs e)
        {
            ChooseFolder(fileBox_Decrypt);
        }

        private void encrypt_Click(object sender, EventArgs e)
        {
            string Str_en = encryptKey.Text.Trim();
            string Str_mo = encryptModulus.Text.Trim();
            int Num_en;
            int Num_mo;
            bool isNum_en = int.TryParse(Str_en, out Num_en);
            bool isNum_mo = int.TryParse(Str_mo, out Num_mo);
            if (!isNum_en || !isNum_mo && Num_en > 0 && Num_mo > 0)
                MessageBox.Show("Key or Modulus must be a valid number! (Numeric Integer and > 0)");
            else
            {
                EncryptMessage();
            }
        }

        private void decrypt_Click(object sender, EventArgs e)
        {
            string Str_en = decryptKey.Text.Trim();
            string Str_mo = decryptModulus.Text.Trim();
            int Num_en;
            int Num_mo;
            bool isNum_en = int.TryParse(Str_en, out Num_en);
            bool isNum_mo = int.TryParse(Str_mo, out Num_mo);
            if (!isNum_en || !isNum_mo && Num_en > 0 && Num_mo > 0)
                MessageBox.Show("Key or Modulus must be a valid number! (Numeric Integer and > 0)");
            else
            {
                DecryptMessage();
            }
        }

        private void EncryptMessage()
        {
            List<string> lines = new List<string>();

            string encryptionPath = fileBox_Encrypt.Text;

            if (File.Exists(encryptionPath) && Path.GetExtension(encryptionPath) == ".txt")
            {
                    
                #region encryption

                int k = int.Parse(encryptKey.Text);
                int modulus = int.Parse(encryptModulus.Text);

                using (StreamReader sr = new StreamReader(encryptionPath))
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
                                int remainder = ((((int)wordList[i][j]) - 33) + k) % modulus;  //calculate remainder through k shift
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
                using (StreamWriter sw = new StreamWriter(encryptionPath))
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        sw.WriteLine(lines[i]);
                    }
                }

                #endregion

                MessageBox.Show("COMPLETED ENCRYPTION!", "ENCRYPTION", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("ERROR: File " + encryptionPath + " does not exist. \n Please enter existing file name.", "ERROR in File", MessageBoxButtons.OK);
            }
        }

        private void DecryptMessage()
        {
            List<string> lines = new List<string>();

            string decryptionPath = fileBox_Decrypt.Text;

            if (File.Exists(decryptionPath) && Path.GetExtension(decryptionPath) == ".txt")
            {

                #region encryption

                int k = int.Parse(decryptKey.Text);
                int modulus = int.Parse(decryptModulus.Text);

                using (StreamReader sr = new StreamReader(decryptionPath))
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

                                int remainder = ((((int)wordList[i][j]) - 33) - k) % modulus;  //calculate remainder through k shift
                                if (remainder < 0) remainder += modulus;
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
                using (StreamWriter sw = new StreamWriter(decryptionPath))
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        sw.WriteLine(lines[i]);
                    }
                }

                #endregion

                MessageBox.Show("COMPLETED DECRYPTION!", "DECRYPTION", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("ERROR: File " + decryptionPath + " does not exist. \n Please enter existing file name.", "ERROR in File", MessageBoxButtons.OK);
            }
        }
        
    }
}
