using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public struct Vertex : IVertexType
    {
        public Vector3 Position;
        public Vector2 Texture;
        public Color Color;
        public static readonly VertexDeclaration VertexDeclaration;

        public Vertex(Vector3 position, Vector2 texture, Color color)
        {
            Position = position;
            Texture = texture;
            Color = color;
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexDeclaration;
            }
        }

        public override int GetHashCode()
        {
            // TODO: Fix gethashcode
            return 0;
        }

        public override string ToString()
        {
            return string.Format("{{Position:{0} Texture:{1} Color:{2}}}", new object[] { Position, Texture, Color });
        }

        public static bool operator ==(Vertex left, Vertex right)
        {
            return ((left.Position == right.Position) 
                && (left.Texture == right.Texture) 
                && (left.Color == right.Color));
        }

        public static bool operator !=(Vertex left, Vertex right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != base.GetType())
            {
                return false;
            }
            return (this == ((Vertex)obj));
        }

        static Vertex()
        {
            VertexElement[] elements = new VertexElement[] {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(20, VertexElementFormat.Color, VertexElementUsage.Color, 0) };
            VertexDeclaration declaration = new VertexDeclaration(elements);
            VertexDeclaration = declaration;
        }
    }
}
