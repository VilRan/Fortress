using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public struct Quad
    {
        public Tri ABC;
        public Tri CBD;

        public IEnumerable<Vertex> Vertices
        {
            get
            {
                return 
                    ABC.Vertices
                    .Concat(CBD.Vertices);
            }
        }

        public Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            Vertex aVertex = new Vertex(a, new Vector2(0, 0), Color.White);
            Vertex bVertex = new Vertex(b, new Vector2(0, 1), Color.White);
            Vertex cVertex = new Vertex(c, new Vector2(1, 0), Color.White);
            Vertex dVertex = new Vertex(d, new Vector2(1, 1), Color.White);

            ABC = new Tri(aVertex, bVertex, cVertex);
            CBD = new Tri(cVertex, bVertex, dVertex);
        }
    }
}
