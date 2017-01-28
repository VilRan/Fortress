using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public static class VectorExtensions
    {
        public static Point3 ToPoint3(this Vector3 vector)
        {
            return new Point3((int)vector.X, (int)vector.Y, (int)vector.Z);
        }
    }
}
