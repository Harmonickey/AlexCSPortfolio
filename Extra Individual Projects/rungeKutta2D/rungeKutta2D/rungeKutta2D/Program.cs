using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace rungekutta2D
{
    class Program
    {
        static double t = 0;
        static double h = 0;
        static double moreInput;

        static public double GM = 1;

        static public int iterations = 200;

        static void Main(string[] args)
        {
            do
            {
                //initial x and y coordinate of the orbiting object
                double initialX = readInt("\nInput an initial value for X: ");
                double initialY = readInt("\nInput an initial value for Y: ");

                //the radius of the oribiting object
                double radius = Math.Sqrt(Math.Pow(initialX, 2) + Math.Pow(initialY, 2));

                //the semimajoraxis
                double semiMajorAxis = readInt("Input semi major axis: ");

                //find angle from general horizontal which is the 0degree of the 'sun'
                double numerator = initialX * radius;
                double denomenator = Math.Sqrt(Math.Pow(initialX, 2) + Math.Pow(initialY, 2)) * Math.Sqrt(Math.Pow(radius, 2));
                double angle = Math.Acos(numerator / denomenator) * (180 / Math.PI);

                //get an extra angle to add to 90 for ellipses
                Console.Write("Input optional angle from 90 degrees: ");
                string lineRead = Console.ReadLine();
                if (lineRead == "" || lineRead == " ") lineRead = "0";
                double extraAngle = double.Parse(lineRead);
                
                double initialVX, initialVY;
                                    //90 degrees plus additional (pos extraAngle goes towards origen)
                double directionX = Math.Cos((angle + 90 + extraAngle) * (Math.PI / 180));//direction
                double directionY = Math.Sin((angle + 90 + extraAngle) * (Math.PI / 180));//direction
                //the magnitude is the velocity equation, it then gets multiplied by a direction
                //double magnitude2 = Math.Sqrt(GM / radius);  //magnitude for circular orbits

                //magnitude of the inital velocity for an elliptical orbit
                double magnitude = Math.Sqrt(GM * ((2 / radius) - (1 / semiMajorAxis)));

                //initial velocities
                initialVX = directionX * magnitude;
                initialVY = directionY * magnitude;
                
                Vector4 y = new Vector4(initialX, initialY, initialVX, initialVY);

                t = readInt("Input a initial value for time 't': ");

                do
                {
                    h = readInt("\nInput a Step Size Value: ");
                    if (h > 1) Console.WriteLine("Step Size must be less than or equal to 1");
                } while (h > 1);

                Vector4[] k = new Vector4[5];
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

                string path = encryptionFilePath + directories[whichOne] + "\\Desktop\\" + FN + ".gr";

                if (File.Exists(path))
                {
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        sw.WriteLine("[saved-setup]" + Environment.NewLine + Environment.NewLine +
                                     "[labels]" + Environment.NewLine +
                                     "xaxis=x" + Environment.NewLine +
                                     "yaxis=x" + Environment.NewLine + Environment.NewLine +
                                     "[grid]" + Environment.NewLine +
                                     "left=-2" + Environment.NewLine +
                                     "right=2" + Environment.NewLine +
                                     "top=auto" + Environment.NewLine +
                                     "bottom=auto" + Environment.NewLine + Environment.NewLine +
                                     "[options]" + Environment.NewLine +
                                     "detail=grid" + Environment.NewLine +
                                     "axes=1808" + Environment.NewLine +
                                     "showdataplot=on" + Environment.NewLine + Environment.NewLine +
                                     "[functions]" + Environment.NewLine + Environment.NewLine +
                                     "[dataplot]" + Environment.NewLine +
                                     "name=Data plot 1" + Environment.NewLine +
                                     "numpoints=" + iterations + Environment.NewLine +
                                     "color=3" + Environment.NewLine +
                                     "shape=3");

                        int i = 1;
                        do
                        {
                            //k_1 = h * f(x_n, y_n)
                            k[1] = h * deriv(t, y);

                            k[2] = h * deriv(t + h / 2, y + (k[1] / 2));

                            k[3] = h * deriv(t + h / 2, y + (k[2] / 2));

                            k[4] = h * deriv(t + h, y + k[3]);
                            //y_n+1 = y_n + (k_1)/6    + (k_2)/3  + (k_3)/3  +  (k_3)/6
                            y = y + k[1] / 6 + k[2] / 3 + k[3] / 3 + k[4] / 6;

                            //y = "{X, Y, VX, VY}"
                            sw.WriteLine(y.X + "\t" + y.Y);
                            //Console.WriteLine(y);
                            //Console.WriteLine();
                            //Console.ReadLine();
                            i++;

                        } while (i < iterations);

                        do
                        {
                            moreInput = readInt("Continue? 1:Yes 2:No : ");
                            if (moreInput != 1 && moreInput != 2)
                                Console.WriteLine("Enter a 1 or 2 to continue");
                        } while (moreInput != 1 && moreInput != 2);
                    }
                }
            } while(moreInput == 1);


        }
        
        static Vector4 deriv(double t, Vector4 bodyData)
        {
            //X = xCoor, Y = yCoor, VX = vxComp, VY = vyComp
            Vector4 f = new Vector4(0,0,0,0);
            
            f.X = bodyData.VX;
            f.Y = bodyData.VY;
            f.VX = ((-GM) / Math.Pow(Math.Pow(bodyData.X, 2) + Math.Pow(bodyData.Y, 2), 1.5)) * bodyData.X;
            f.VY = ((-GM) / Math.Pow(Math.Pow(bodyData.X, 2) + Math.Pow(bodyData.Y, 2), 1.5)) * bodyData.Y;

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
