using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class Block : Structure
    {
        public override IEnumerable<Vertex> Vertices
        {
            get
            {
                Cube cube = new Cube(GridPosition.ToVector3());
                IEnumerable<Vertex> vertices = cube.Up.Vertices;
                if (Map.IsObstructed(GridPosition + Point3.Up) == true)
                    vertices = VertexUtility.Color(vertices, new Color(0.25f, 0.25f, 0.25f));
                if (Map.IsObstructed(GridPosition + Point3.Forward) == false)
                    vertices = vertices.Concat(cube.Forward.Vertices);
                if (Map.IsObstructed(GridPosition + Point3.Right) == false)
                    vertices = vertices.Concat(cube.Right.Vertices);
                if (Map.IsObstructed(GridPosition + Point3.Backward) == false)
                    vertices = vertices.Concat(cube.Backward.Vertices);
                if (Map.IsObstructed(GridPosition + Point3.Left) == false)
                    vertices = vertices.Concat(cube.Left.Vertices);
                return vertices;
            }
        }

        public Block(Map map, Point3 position)
            : base(map, position)
        {

        }
    }
}
