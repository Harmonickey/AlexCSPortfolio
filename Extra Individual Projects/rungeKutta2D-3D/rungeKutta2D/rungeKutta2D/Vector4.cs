using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rungekutta2D
{
    class Vector4
    {
        //X and Y Coordinates for the Vector class
        public double X;
        public double Y;
        public double VX;
        public double VY;

        //give a starting x and y coordinate
        public Vector4(double xComp, double yComp, double vXComp, double vYComp)
        {
            X = xComp;
            Y = yComp;
            VX = vXComp;
            VY = vYComp;
        }

        //give it nothing, it will return (0,0)
        public Vector4()
        {
            X = 0;
            Y = 0;
            VX = 0;
            VY = 0;
        }

        //subtract two vectors
        public static Vector4 operator -(Vector4 value1, Vector4 value2)
        {
            Vector4 newVector = new Vector4(0, 0, 0, 0);

            newVector.X = value1.X - value2.X;
            newVector.Y = value1.Y - value2.Y;
            newVector.VX = value1.VX - value2.VX;
            newVector.VY = value1.VY - value2.VY;

            return newVector;
        }

        //add two vectors
        public static Vector4 operator +(Vector4 value1, Vector4 value2)
        {
            Vector4 newVector = new Vector4(0, 0, 0, 0);

            newVector.X = value1.X + value2.X;
            newVector.Y = value1.Y + value2.Y;
            newVector.VX = value1.VX + value2.VX;
            newVector.VY = value1.VY + value2.VY;

            return newVector;
        }

        //multiply two vectors
        public static Vector4 operator *(Vector4 value1, Vector4 value2)
        {
            Vector4 newVector = new Vector4(0, 0, 0, 0);

            newVector.X = value1.X * value2.X;
            newVector.Y = value1.Y * value2.Y;
            newVector.VX = value1.VX * value2.VX;
            newVector.VY = value1.VY * value2.VY;

            return newVector;
        }

        //multiply a scalar by a vector
        public static Vector4 operator *(double scaleFactor, Vector4 value)
        {
            Vector4 newVector = new Vector4(0, 0, 0, 0);

            newVector.X = scaleFactor * value.X;
            newVector.Y = scaleFactor * value.Y;
            newVector.VX = scaleFactor * value.VX;
            newVector.VY = scaleFactor * value.VY;


            return newVector;
        }

        //multiply a vector by a scalar
        public static Vector4 operator *(Vector4 value, double scaleFactor)
        {
            Vector4 newVector = new Vector4(0, 0, 0, 0);

            newVector.X = scaleFactor * value.X;
            newVector.Y = scaleFactor * value.Y;
            newVector.VX = scaleFactor * value.VX;
            newVector.VY = scaleFactor * value.VY;

            return newVector;
        }

        public static Vector4 operator /(Vector4 value, double divisor)
        {
            Vector4 newVector = new Vector4(0, 0, 0, 0);
            if (divisor != 0)
            {
                newVector.X = value.X / divisor;
                newVector.Y = value.Y / divisor;
                newVector.VX = value.VX / divisor;
                newVector.VY = value.VY / divisor;
            }
            else
                newVector = new Vector4(0, 0, 0, 0);

            return newVector;
        }

        public static Vector4 operator /(Vector4 value1, Vector4 value2)
        {
            Vector4 newVector = new Vector4(0, 0, 0, 0);
            if (value2.X != 0) newVector.X = value1.X / value2.X;
            else newVector.X = 0;
            if (value2.Y != 0) newVector.Y = value1.Y / value2.Y;
            else newVector.Y = 0;
            if (value2.VX != 0) newVector.VX = value1.VX / value2.VX;
            else newVector.VX = 0;
            if (value2.VY != 0) newVector.VY = value1.VY / value2.VY;
            else newVector.VY = 0;
            
            return newVector;
        }

        //be able to convert to string to print out on console
        public override string ToString()
        {


            return "{" + X + "," + Y + "}" + "  Velocity: " + Math.Sqrt(Math.Pow(VX, 2) + Math.Pow(VY, 2)) + Environment.NewLine +
                   "                                       Circular Period: " + (2*Math.PI*(Math.Pow(Math.Pow(X, 2) + Math.Pow(Y, 2), 1.5))) + Environment.NewLine +
                   "                                       Energy Consv: " + (.5 * (Math.Pow(VX, 2) + Math.Pow(VY, 2)) - (1 / (Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2))))); // + " {" + VX + "," + VY + "}";
        }

    }
}
