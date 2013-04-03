using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace rungeKutta3D
{
    class Program
    {
        static double t = 0;
        static double h = 0;
        static double moreInput;

        static public double GM = 1;

        static public int iterations = 500;

        static void Main(string[] args)
        {
            do
            {
                //initial x and y coordinate of the orbiting object
                double initialX = readInt("\nInput an initial value for X: ");
                double initialY = readInt("\nInput an initial value for Y: ");
                double initialZ = readInt("\nInput an initial value for Z: ");

                double semiMajorAxis = readInt("\nInput a possible semi major axis length: ");

                //the radius of the oribiting object
                double radius = Math.Sqrt(Math.Pow(initialX, 2) + Math.Pow(initialY, 2) + Math.Pow(initialZ, 2));

                //initial velocities
                double initialVX, initialVY, initialVZ;

                //double magnitude = Math.Sqrt(GM / radius);

                double magnitude = Math.Sqrt(GM * ((2 / radius) - (1 / semiMajorAxis)));

                initialVX = readInt("\nInput an initial unit X direction: ") * magnitude;
                initialVY = readInt("\nInput an initial unit Y direction: ") * magnitude;
                initialVZ = readInt("\nInput an initial unit Z direction: ") * magnitude;

                Vector6 y = new Vector6(initialX, initialY, initialZ, initialVX, initialVY, initialVZ);

                t = readInt("Input a initial value for time 't': ");

                do
                {
                    h = readInt("\nInput a Step Size Value: ");
                    if (h > 1) Console.WriteLine("Step Size must be less than or equal to 1");
                } while (h > 1);

                Vector6[] k = new Vector6[5];
                k[0] = null;

                Console.WriteLine(y);

                Console.Write("Enter the name of the text file " + Environment.NewLine + "on your desktop to be written: ");

                string FN = Console.ReadLine();
                string P = Directory.GetCurrentDirectory();
                DirectoryInfo parent = Directory.GetParent(P);
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

                string path = encryptionFilePath + directories[whichOne] + "\\Desktop\\" + FN + ".txt";

                if (File.Exists(path))
                {
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        int j = 1;
                        do
                        {
                            //k_1 = h * f(x_n, y_n)
                            k[1] = h * deriv(t, y);

                            k[2] = h * deriv(t + h / 2, y + (k[1] / 2));

                            k[3] = h * deriv(t + h / 2, y + (k[2] / 2));

                            k[4] = h * deriv(t + h, y + k[3]);
                            //y_n+1 = y_n + (k_1)/6    + (k_2)/3  + (k_3)/3  +  (k_3)/6
                            y = y + k[1] / 6 + k[2] / 3 + k[3] / 3 + k[4] / 6;

                            //y = "{X, Y, Z, VX, VY, VZ}"
                            sw.WriteLine(y.X + "\t" + y.Y + "\t" + y.Z);
                            //Console.WriteLine(y);
                            //Console.WriteLine();
                            //Console.ReadLine();
                            j++;

                        } while (j < iterations);
                    }
                }

                do
                {
                    moreInput = readInt("Continue? 1:Yes 2:No : ");
                    if (moreInput != 1 && moreInput != 2)
                        Console.WriteLine("Enter a 1 or 2 to continue");
                } while (moreInput != 1 && moreInput != 2);


            } while (moreInput == 1);


        }

        static Vector6 deriv(double t, Vector6 bodyData)
        {
            //X = xCoor, Y = yCoor, Z = zCoor, VX = vxComp, VY = vyComp, VZ = vzComp
            Vector6 f = new Vector6(0, 0, 0, 0, 0, 0);
            double component = (-GM) / Math.Pow(Math.Pow(bodyData.X, 2) + Math.Pow(bodyData.Y, 2) + Math.Pow(bodyData.Z, 2), 1.5);

            f.X = bodyData.VX;
            f.Y = bodyData.VY;
            f.Z = bodyData.VZ;
            f.VX = component * bodyData.X;
            f.VY = component * bodyData.Y;
            f.VZ = component * bodyData.Z;

            return f;
        }

        static double readInt(string p)
        {
            Console.Write(p);
            string line = Console.ReadLine();
            return double.Parse(line);
        }
    }
}
