using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class Sector
    {
        readonly Map map;
        readonly int xOffset;
        readonly int zOffset;
        readonly int xLimit;
        readonly int zLimit;
        readonly int y;
        VertexBuffer vertexBuffer;

        GraphicsDevice graphics { get { return map.Game.GraphicsDevice; } }

        public Sector(Map map, int xOffset, int zOffset, int xLimit, int zLimit, int y)
        {
            this.map = map;
            this.xOffset = xOffset;
            this.zOffset = zOffset;
            this.xLimit = xLimit;
            this.zLimit = zLimit;
            this.y = y;
        }

        public void BuildVertexBuffer()
        {
            if (vertexBuffer != null)
                vertexBuffer.Dispose();

            List<Vertex> vertices = new List<Vertex>();
            for (int x = xOffset; x < xLimit; x++)
            {
                for (int z = zOffset; z < zLimit; z++)
                {
                    Structure structure = map.GetStructure(x, y, z);
                    if (structure != null && structure.GridPosition == new Point3(x, y, z))
                        vertices.AddRange(structure.Vertices);
                }
            }

            vertexBuffer = new VertexBuffer(graphics, typeof(Vertex), vertices.Count, BufferUsage.WriteOnly);
            if (vertices.Count > 0)
                vertexBuffer.SetData(vertices.ToArray());
        }

        public void Draw(Camera camera)
        {
            if (vertexBuffer.VertexCount == 0)
                return;

            BoundingBox visibilityBox = new BoundingBox(new Vector3(xOffset, y - 0.5f, zOffset), new Vector3(xLimit, y + 0.5f, zLimit));
            if (camera.Frustrum.Contains(visibilityBox) == ContainmentType.Disjoint)
                return;
            
            graphics.SetVertexBuffer(vertexBuffer);
            graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, vertexBuffer.VertexCount / 3);
        }
    }
}
