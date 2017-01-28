using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class Unit
    {
        public Vector3 Position;
        public Map Map;
        readonly VertexBuffer vertexBuffer;
        BinaryHeap<Order> Orders = new BinaryHeap<Order>();

        public Point3 GridPosition { get { return (Position + 0.5f * Vector3.One).ToPoint3(); } }
        FortressGame game { get { return Map.Game; } }
        GraphicsDevice graphics { get { return game.GraphicsDevice; } }

        public Unit(Map map, Vector3 position)
        {
            Position = position;
            Map = map;
            Cube cube = new Cube(Vector3.Zero, 0.8f);
            Vertex[] vertices = cube.Vertices.ToArray();
            vertexBuffer = new VertexBuffer(graphics, typeof(Vertex), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }

        public void Update(float time)
        {
            if (GridPosition.Y > 0 && Map.GetStructure(GridPosition + Point3.Down) == null)
            {
                Position += Vector3.Down;
                return;
            }

            if (Orders.Count == 0)
            {
                return;
            }

            Order order = Orders.Peek();
            OrderStatus status = order.Execute(time);
            if (status == OrderStatus.Completed)
                Orders.Remove();
        }

        public void Draw(Camera camera)
        {
            if (GridPosition.Y > camera.Floor)
                return;

            BoundingBox visibilityBox = new BoundingBox(Position - 0.5f * Vector3.One, Position + 0.5f * Vector3.One);
            if (camera.Frustrum.Contains(visibilityBox) == ContainmentType.Disjoint)
                return;

            Effect effect = game.DefaultShader;
            Matrix world = Matrix.CreateTranslation(Position);
            effect.Parameters["WorldViewProjection"].SetValue(world * camera.View * camera.Projection);
            effect.Parameters["AmbientColor"].SetValue(Color.Aqua.ToVector4());
            effect.Parameters["LightLevel"].SetValue(0.5f + camera.Floor - Position.Y);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.SetVertexBuffer(vertexBuffer);
                graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, vertexBuffer.VertexCount / 3);
            }
        }

        public void QueueOrder(Order order)
        {
            Orders.Add(order);
        }
    }
}
