using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class MapGameState : GameState
    {
        KeyboardState previousKeyboard = Keyboard.GetState();
        MouseState previousMouse = Mouse.GetState();
        Camera camera;
        MapCursor cursor;
        Unit selectedUnit;

        Map map { get { return Game.Session.Map; } }
        GraphicsDevice graphics { get { return Game.GraphicsDevice; } }
        Effect effect { get { return Game.DefaultShader; } }


        public MapGameState(FortressGame game)
            : base(game)
        {
            camera = new Camera(map, MathHelper.ToRadians(45), (float)Game.Width / Game.Height);
            cursor = new MapCursor(game);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboard.IsKeyDown(Keys.W))
                camera.Position += 10f * time * new Vector3(1, 0, 1);
            if (keyboard.IsKeyDown(Keys.S))
                camera.Position += 10f * time * new Vector3(-1, 0, -1);
            if (keyboard.IsKeyDown(Keys.A))
                camera.Position += 10f * time * new Vector3(1, 0, -1);
            if (keyboard.IsKeyDown(Keys.D))
                camera.Position += 10f * time * new Vector3(-1, 0, 1);
            
            int scrollDelta = mouse.ScrollWheelValue - previousMouse.ScrollWheelValue;
            if (scrollDelta > 0)
                camera.Floor++;
            if (scrollDelta < 0)
                camera.Floor--;

            camera.Update();

            Vector3? worldPosition = getWorldPosition(mouse.Position.ToVector2());
            if (worldPosition != null)
            {
                cursor.Position = getGridPosition(worldPosition.Value);
                if (keyboard.IsKeyDown(Keys.LeftAlt))
                {
                    if (mouse.LeftButton == ButtonState.Pressed)
                        map.AddStructure(new Block(map, cursor.Position));
                    if (mouse.RightButton == ButtonState.Pressed)
                        map.RemoveStructure(cursor.Position);
                }
                else
                {
                    if (mouse.LeftButton == ButtonState.Pressed)
                        selectedUnit = map.GetUnitAt(cursor.Position.ToVector3());
                    if (mouse.RightButton == ButtonState.Released && previousMouse.RightButton == ButtonState.Pressed)
                        if (selectedUnit != null)
                            selectedUnit.QueueOrder(new MoveOrder(selectedUnit, cursor.Position));
                }
            }

            previousKeyboard = keyboard;
            previousMouse = mouse;

            map.Update(time);
        }

        public override void Draw()
        {
            map.Draw(camera);
            cursor.Draw(camera);
        }

        Vector3? getWorldPosition(Vector2 screenPosition)
        {
            screenPosition -= Game.Window.ClientBounds.Location.ToVector2();
            Vector3 nearScreenPosition = new Vector3(screenPosition.X, screenPosition.Y, 0);
            Vector3 farScreenPosition = new Vector3(screenPosition.X, screenPosition.Y, 1);
            Vector3 nearWorldPosition = graphics.Viewport.Unproject(nearScreenPosition, camera.Projection, camera.View, Matrix.Identity);
            Vector3 farWorldPosition = graphics.Viewport.Unproject(farScreenPosition, camera.Projection, camera.View, Matrix.Identity);
            Vector3 direction = Vector3.Normalize(farWorldPosition - nearWorldPosition);
            Ray ray = new Ray(nearWorldPosition, direction);
            Plane plane = new Plane(Vector3.Up, 0.5f - camera.Floor);
            float? distance = ray.Intersects(plane);
            if (distance != null)
                return nearWorldPosition + distance.Value * direction + 0.5f * Vector3.Up;
            return null;
        }

        Point3 getGridPosition(Vector3 worldPosition)
        {
            int x = (int)Math.Round(worldPosition.X, MidpointRounding.AwayFromZero);
            int y = (int)Math.Round(worldPosition.Y, MidpointRounding.AwayFromZero);
            int z = (int)Math.Round(worldPosition.Z, MidpointRounding.AwayFromZero);
            x = MathHelper.Clamp(x, 0, map.Width - 1);
            y = MathHelper.Clamp(y, 0, map.Height - 1);
            z = MathHelper.Clamp(z, 0, map.Depth - 1);
            return new Point3(x, y, z);
        }
    }
}
