using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public struct Tri
    {
        public Vertex A, B, C;

        public IEnumerable<Vertex> Vertices
        {
            get
            {
                yield return A;
                yield return B;
                yield return C;
            }
        }

        public Tri(Vertex a, Vertex b, Vertex c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
