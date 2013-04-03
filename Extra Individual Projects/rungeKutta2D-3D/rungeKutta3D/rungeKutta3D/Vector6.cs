using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rungeKutta3D
{
    class Vector6
    {
        //X and Y Coordinates for the Vector class
        public double X;
        public double Y;
        public double Z;
        public double VX;
        public double VY;
        public double VZ;

        //give a starting x and y coordinate
        public Vector6(double xComp, double yComp, double zComp, double vXComp, double vYComp, double vZComp)
        {
            X = xComp;
            Y = yComp;
            Z = zComp;
            VX = vXComp;
            VY = vYComp;
            VZ = vZComp;
        }

        //give it nothing, it will return (0,0)
        public Vector6()
        {
            X = 0;
            Y = 0;
            Z = 0;
            VX = 0;
            VY = 0;
            VZ = 0;
        }

        //subtract two vectors
        public static Vector6 operator -(Vector6 value1, Vector6 value2)
        {
            Vector6 newVector = new Vector6(0, 0, 0, 0, 0, 0);

            newVector.X = value1.X - value2.X;
            newVector.Y = value1.Y - value2.Y;
            newVector.Z = value1.Z - value2.Z;
            newVector.VX = value1.VX - value2.VX;
            newVector.VY = value1.VY - value2.VY;
            newVector.VZ = value1.VZ - value2.VZ;

            return newVector;
        }

        //add two vectors
        public static Vector6 operator +(Vector6 value1, Vector6 value2)
        {
            Vector6 newVector = new Vector6(0, 0, 0, 0, 0, 0);

            newVector.X = value1.X + value2.X;
            newVector.Y = value1.Y + value2.Y;
            newVector.Z = value1.Z + value2.Z;
            newVector.VX = value1.VX + value2.VX;
            newVector.VY = value1.VY + value2.VY;
            newVector.VZ = value1.VZ + value2.VZ;

            return newVector;
        }

        //multiply two vectors
        public static Vector6 operator *(Vector6 value1, Vector6 value2)
        {
            Vector6 newVector = new Vector6(0, 0, 0, 0, 0, 0);

            newVector.X = value1.X * value2.X;
            newVector.Y = value1.Y * value2.Y;
            newVector.Z = value1.Z * value2.Z;
            newVector.VX = value1.VX * value2.VX;
            newVector.VY = value1.VY * value2.VY;
            newVector.VZ = value1.VZ * value2.VZ;

            return newVector;
        }

        //multiply a scalar by a vector
        public static Vector6 operator *(double scaleFactor, Vector6 value)
        {
            Vector6 newVector = new Vector6(0, 0, 0, 0, 0, 0);

            newVector.X = scaleFactor * value.X;
            newVector.Y = scaleFactor * value.Y;
            newVector.Z = scaleFactor * value.Z;
            newVector.VX = scaleFactor * value.VX;
            newVector.VY = scaleFactor * value.VY;
            newVector.VZ = scaleFactor * value.VZ;

            return newVector;
        }

        //multiply a vector by a scalar
        public static Vector6 operator *(Vector6 value, double scaleFactor)
        {
            Vector6 newVector = new Vector6(0, 0, 0, 0, 0, 0);

            newVector.X = scaleFactor * value.X;
            newVector.Y = scaleFactor * value.Y;
            newVector.Z = scaleFactor * value.Z;
            newVector.VX = scaleFactor * value.VX;
            newVector.VY = scaleFactor * value.VY;
            newVector.VZ = scaleFactor * value.VZ;

            return newVector;
        }

        public static Vector6 operator /(Vector6 value, double divisor)
        {
            Vector6 newVector = new Vector6(0, 0, 0, 0, 0, 0);
            if (divisor != 0)
            {
                newVector.X = value.X / divisor;
                newVector.Y = value.Y / divisor;
                newVector.Z = value.Z / divisor;
                newVector.VX = value.VX / divisor;
                newVector.VY = value.VY / divisor;
                newVector.VZ = value.VZ / divisor;
            }
            else
                newVector = new Vector6(0, 0, 0, 0, 0, 0);

            return newVector;
        }

        public static Vector6 operator /(Vector6 value1, Vector6 value2)
        {
            Vector6 newVector = new Vector6(0, 0, 0, 0, 0, 0);
            if (value2.X != 0) newVector.X = value1.X / value2.X;
            else newVector.X = 0;
            if (value2.Y != 0) newVector.Y = value1.Y / value2.Y;
            else newVector.Y = 0;
            if (value2.Z != 0) newVector.Z = value1.Z / value2.Z;
            else newVector.Z = 0;
            if (value2.VX != 0) newVector.VX = value1.VX / value2.VX;
            else newVector.VX = 0;
            if (value2.VY != 0) newVector.VY = value1.VY / value2.VY;
            else newVector.VY = 0;
            if (value2.VZ != 0) newVector.VZ = value1.VZ / value2.VZ;
            else newVector.VZ = 0;
            
            return newVector;
        }

        //be able to convert to string to print out on console
        public override string ToString()
        {

            return "{" + X + "," + Y + "," + Z + "}" + "  Velocity: " + Math.Sqrt(Math.Pow(VX, 2) + Math.Pow(VY, 2) + Math.Pow(VZ, 2)) + Environment.NewLine +
                   "                                       Circular Period: " + (2*Math.PI*(Math.Pow(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2), 1.5))) + Environment.NewLine +
                   "                                       Energy Consv: " + (.5 * (Math.Pow(VX, 2) + Math.Pow(VY, 2) + Math.Pow(VZ, 2)) - (1 / (Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2))))); // + " {" + VX + "," + VY + "}";
        }
    }
}
