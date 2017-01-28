using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class Map
    {
        public Session Session;
        public Node[,,] Nodes;
        public Floor[] Floors;
        public List<Unit> Units = new List<Unit>();
        Structure[,,] structures;

        public FortressGame Game { get { return Session.Game; } }
        public int Width { get { return structures.GetLength(0); } }
        public int Height { get { return structures.GetLength(1); } }
        public int Depth { get { return structures.GetLength(2); } }
        Effect effect { get { return Game.DefaultShader; } }
        Random random { get { return Game.Random; } }

        public Map(Session session, int width, int height, int depth)
        {
            Session = session;

            Floors = new Floor[height];
            Nodes = new Node[width, height, depth];
            structures = new Structure[width, height, depth];
            for (int y = 0; y < height; y++)
            {
                Floors[y] = new Floor(this, y);
                for (int x = 0; x < width; x++)
                    for (int z = 0; z < depth; z++)
                    {
                        Nodes[x, y, z] = new Node(new Point3(x, y, z));
                        if (y < 3)
                            structures[x, y, z] = new Block(this, new Point3(x, y, z));
                    }
            }
            foreach (Floor floor in Floors)
                floor.BuildVertexBuffer();
            foreach (Node node in Nodes)
                node.UpdateLinks(this);

            for (int x = 0; x < width; x++)
                for (int z = 0; z < depth; z++)
                    if (random.Next(100) == 1)
                        Units.Add(new Unit(this, new Vector3(x, 3, z)));
        }

        public void Update(float time)
        {
            foreach (Unit unit in Units)
                unit.Update(time);
        }

        public void Draw(Camera camera)
        {
            effect.Parameters["WorldViewProjection"].SetValue(Matrix.Identity * camera.View * camera.Projection);
            effect.Parameters["Texture"].SetValue(Game.MatterTexture);
            effect.Parameters["AmbientColor"].SetValue(Color.Aqua.ToVector4());
            effect.Parameters["LightLevel"].SetValue(0.5f + camera.Floor);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                for (int y = 0; y <= camera.Floor; y++)
                    Floors[y].Draw(camera);
            }

            foreach (Unit unit in Units)
            {
                unit.Draw(camera);
            }
        }
        
        public IEnumerable<Node> GetNodes(Point3 min, Point3 max)
        {
            for (int x = min.X; x <= max.X; x++)
                for (int y = min.Y; y <= max.Y; y++)
                    for (int z = min.Z; z <= max.Z; z++)
                        yield return Nodes[x, y, z];
        }

        public IEnumerable<Node> GetNodesSafe(Point3 min, Point3 max)
        {
            Point3 boundsMin = new Point3(0, 0, 0);
            Point3 boundsMax = new Point3(Width, Height, Depth) - Point3.One;
            min = Point3.Clamp(min, boundsMin, boundsMax);
            max = Point3.Clamp(max, min, boundsMax);
            return GetNodes(min, max);
        }

        public void AddStructure(Structure structure)
        {
            int x = structure.GridPosition.X;
            int y = structure.GridPosition.Y;
            int z = structure.GridPosition.Z;

            if (structures[x, y, z] == null)
            {
                structures[x, y, z] = structure;
                Floors[y].BuildVertexBuffer(x, z);
            }

            foreach (Node node in GetNodesSafe(new Point3(x - 1, y, z - 1), new Point3(x + 1, y, z + 1)))
                node.UpdateLinks(this);

            foreach (Node node in GetNodesSafe(new Point3(x, y + 1, z), new Point3(x, Height - 1, z)))
                node.UpdateLinks(this);
        }

        public void RemoveStructure(Point3 position)
        {
            int x = position.X;
            int y = position.Y;
            int z = position.Z;

            structures[x, y, z] = null;
            Floors[y].BuildVertexBuffer(x, z);

            foreach (Node node in GetNodesSafe(new Point3(x - 1, y, z - 1), new Point3(x + 1, y, z + 1)))
                node.UpdateLinks(this);

            foreach (Node node in GetNodesSafe(new Point3(x, y + 1, z), new Point3(x, Height - 1, z)))
                node.UpdateLinks(this);
        }

        public Structure GetStructure(int x, int y, int z)
        {
            return structures[x, y, z];
        }

        public Structure GetStructure(Point3 position)
        {
            return structures[position.X, position.Y, position.Z];
        }

        public Unit GetUnitAt(Vector3 position)
        {
            foreach (Unit unit in Units)
                if (Vector3.Distance(unit.Position, position) < 1)
                    return unit;
            return null;
        }

        public Stack<Node> FindPath(Point3 from, Point3 to, Unit unit)
        {
            return Nodes[from.X, from.Y, from.Z].FindPath(Nodes[to.X, to.Y, to.Z], unit);
        }

        public bool IsObstructed(Point3 position)
        {
            int x = position.X;
            int y = position.Y;
            int z = position.Z;

            if (x < 0 || x >= Width
                || y < 0 || y >= Height
                || z < 0 || z >= Depth)
                return false;

            if (structures[x, y, z] == null)
                return false;

            return true;
        }
    }
}
