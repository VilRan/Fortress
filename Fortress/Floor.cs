using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class Floor
    {
        readonly Map map;
        readonly int y;
        readonly int sectorWidth = 10;
        readonly int sectorDepth = 10;
        Sector[,] sectors;

        GraphicsDevice graphics { get { return map.Game.GraphicsDevice; } }
        int xSectors { get { return map.Width / sectorWidth; } }
        int zSectors { get { return map.Depth / sectorDepth; } }

        public Floor(Map map, int y)
        {
            this.map = map;
            this.y = y;
            
            sectors = new Sector[xSectors, zSectors];
            for (int x = 0; x < xSectors; x++)
            {
                for (int z = 0; z < zSectors; z++)
                {
                    int xOffset = x * sectorWidth;
                    int zOffset = z * sectorDepth;
                    int xLimit = xOffset + sectorWidth;
                    int zLimit = zOffset + sectorDepth;
                    sectors[x, z] = new Sector(map, xOffset, zOffset, xLimit, zLimit, y);
                }
            }
        }

        public void BuildVertexBuffer()
        {
            foreach (Sector sector in sectors)
                sector.BuildVertexBuffer();
        }

        public void BuildVertexBuffer(int x, int z, bool buildAdjacent = true)
        {
            int sectorX = x / sectorWidth;
            int sectorZ = z / sectorDepth;
            sectors[sectorX, sectorZ].BuildVertexBuffer();

            if (buildAdjacent)
            {
                if (sectorX > 0 && x % sectorWidth == 0)
                    sectors[sectorX - 1, sectorZ].BuildVertexBuffer();
                if (sectorX < xSectors - 1 && x % sectorWidth == sectorWidth - 1)
                    sectors[sectorX + 1, sectorZ].BuildVertexBuffer();

                if (sectorZ > 0 && z % sectorDepth == 0)
                    sectors[sectorX, sectorZ - 1].BuildVertexBuffer();
                if (sectorZ < zSectors - 1 && z % sectorDepth == sectorDepth - 1)
                    sectors[sectorX, sectorZ + 1].BuildVertexBuffer();

                if (y > 0)
                    map.Floors[y - 1].BuildVertexBuffer(x, z, false);
            }
        }

        public void Draw(Camera camera)
        {
            foreach (Sector sector in sectors)
                sector.Draw(camera);
        }

        /*
        public void BuildVertexBuffer()
        {
            if (vertexBuffer != null)
                vertexBuffer.Dispose();

            List<Vertex> vertices = new List<Vertex>();
            for (int x = 0; x < map.Width; x++)
            {
                for (int z = 0; z < map.Depth; z++)
                {
                    Structure structure = map.Structures[x, y, z];
                    if (structure != null)
                        vertices.AddRange(structure.Vertices);
                }
            }

            vertexBuffer = new VertexBuffer(graphics, typeof(Vertex), vertices.Count, BufferUsage.WriteOnly);
            if (vertices.Count > 0)
                vertexBuffer.SetData(vertices.ToArray());
        }

        public void Draw()
        {
            if (vertexBuffer.VertexCount > 0)
            {
                graphics.SetVertexBuffer(vertexBuffer);
                graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, vertexBuffer.VertexCount / 3);
            }
        }
        */
    }
}
