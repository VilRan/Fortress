using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public static class VertexUtility
    {
        public static IEnumerable<Vertex> Color(IEnumerable<Vertex> vertices, Color color)
        {
            foreach (Vertex vertex in vertices)
                yield return new Vertex(vertex.Position, vertex.Texture, color);
        }
    }
}
