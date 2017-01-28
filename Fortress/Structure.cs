using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public abstract class Structure
    {
        public readonly Map Map;
        public readonly Point3 GridPosition;

        public abstract IEnumerable<Vertex> Vertices { get; }

        public Structure(Map map, Point3 position)
        {
            Map = map;
            GridPosition = position;
        }
    }
}
