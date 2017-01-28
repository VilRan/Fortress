using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public struct Point3
    {
        public int X;
        public int Y;
        public int Z;

        public static Point3 Zero { get { return new Point3(0, 0, 0); } }
        public static Point3 One { get { return new Point3(1, 1, 1); } }
        public static Point3 Up { get { return new Point3(0, 1, 0); } }
        public static Point3 Forward { get { return new Point3(0, 0, -1); } }
        public static Point3 Right { get { return new Point3(1, 0, 0); } }
        public static Point3 Backward { get { return new Point3(0, 0, 1); } }
        public static Point3 Left { get { return new Point3(-1, 0, 0); } }
        public static Point3 Down { get { return new Point3(0, -1, 0); } }
        public float Length { get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); } }

        public Point3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", X, Y, Z);
        }

        public static Point3 Clamp(Point3 value, Point3 min, Point3 max)
        {
            value.X = MathHelper.Clamp(value.X, min.X, max.X);
            value.Y = MathHelper.Clamp(value.Y, min.Y, max.Y);
            value.Z = MathHelper.Clamp(value.Z, min.Z, max.Z);
            return value;
        }

        public static bool operator ==(Point3 left, Point3 right)
        {
            return left.X == right.X
                && left.Y == right.Y
                && left.Z == right.Z;
        }

        public static bool operator !=(Point3 left, Point3 right)
        {
            return !(left == right);
        }

        public static Point3 operator +(Point3 left, Point3 right)
        {
            return new Point3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Point3 operator -(Point3 left, Point3 right)
        {
            return new Point3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != base.GetType())
                return false;
            return (this == ((Point3)obj));
        }

        public override int GetHashCode()
        {
            return X + Y + Z;
        }
    }
}
