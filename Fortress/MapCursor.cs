using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class MapCursor
    {
        public Point3 Position = Point3.Zero;
        FortressGame game;
        VertexBuffer vertexBuffer;

        Effect effect { get { return game.DefaultShader; } }
        GraphicsDevice graphics { get { return game.GraphicsDevice; } }

        public MapCursor(FortressGame game)
        {
            this.game = game;
            Cube cube = new Cube(Vector3.Zero, 1.2f);
            Vertex[] vertices = cube.Vertices.ToArray();
            vertexBuffer = new VertexBuffer(graphics, typeof(Vertex), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }

        public void Draw(Camera camera)
        {
            Matrix cursorWorld = Matrix.CreateTranslation(Position.ToVector3());
            effect.Parameters["WorldViewProjection"].SetValue(cursorWorld * camera.View * camera.Projection);
            effect.Parameters["Texture"].SetValue(game.TransparentTexture);
            effect.Parameters["AmbientColor"].SetValue(Color.Yellow.ToVector4());
            effect.Parameters["LightLevel"].SetValue(0.5f);
            graphics.SetVertexBuffer(vertexBuffer);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, vertexBuffer.VertexCount / 3);
            }
        }
    }
}
