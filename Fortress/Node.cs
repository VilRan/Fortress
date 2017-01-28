using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class Node : IComparable<Node>
    {
        static int currentPathfinderRun = 0;

        public readonly Point3 Position;
        List<Link> links = new List<Link>();
        PathfinderStatus status = PathfinderStatus.Unvisited;
        Node previous = null;
        int lastVisit;
        float pathCost = 0f;
        float heuristic = 0f;

        public Node(Point3 position)
        {
            Position = position;
        }

        public void UpdateLinks(Map map)
        {
            links.Clear();
            addWalkLinks(map);
            addFallLink(map);
        }

        public Stack<Node> FindPath(Node end, Unit unit)
        {
            if (this == end)
                return new Stack<Node>();

            currentPathfinderRun++;
            lastVisit = currentPathfinderRun;
            previous = null;

            var open = new BinaryHeap<Node>();
            open.Add(this);
            bool isUnsorted = false;

            while (open.Count > 0)
            {
                if (isUnsorted)
                {
                    open.Sort();
                    isUnsorted = false;
                }

                Node active = open.Remove();
                active.status = PathfinderStatus.Closed;

                foreach (Link link in active.links)
                {
                    Node neighbor = link.To;

                    if (neighbor == end)
                    {
                        end.previous = active;
                        var path = new Stack<Node>();
                        active = end;
                        while (active != null)
                        {
                            path.Push(active);
                            active = active.previous;
                        }
                        return path;
                    }

                    if (neighbor.lastVisit != currentPathfinderRun)
                    {
                        neighbor.status = PathfinderStatus.Unvisited;
                        neighbor.lastVisit = currentPathfinderRun;
                    }

                    if (neighbor.status != PathfinderStatus.Closed)
                    {
                        float cost = active.pathCost + link.GetCost(unit);

                        if (neighbor.status != PathfinderStatus.Open)
                        {
                            neighbor.previous = active;
                            neighbor.pathCost = cost;
                            neighbor.heuristic = neighbor.computeHeuristic(end, unit);
                            neighbor.status = PathfinderStatus.Open;
                            open.Add(neighbor);
                        }
                        else if (cost < neighbor.pathCost)
                        {
                            neighbor.previous = active;
                            neighbor.pathCost = cost;
                            isUnsorted = true;
                        }
                    }
                }
            }

            return new Stack<Node>();
        }

        public int CompareTo(Node other)
        {
            float valueSelf = pathCost + heuristic;
            float valueOther = other.pathCost + other.heuristic;
            if (valueSelf > valueOther)
                return 1;
            if (valueSelf < valueOther)
                return -1;
            return 0;
        }

        float computeHeuristic(Node end, Unit unit)
        {
            float heuristic = (end.Position - Position).Length;
            if (unit.Map.GetUnitAt(Position.ToVector3()) != null)
                heuristic += 4f;
            return heuristic;
        }

        void addWalkLinks(Map map)
        {
            if (Position.Y == 0 || map.GetStructure(Position + Point3.Down) == null)
                return;

            int minX = Math.Max(Position.X - 1, 0);
            int maxX = Math.Min(Position.X + 1, map.Width - 1);
            int minZ = Math.Max(Position.Z - 1, 0);
            int maxZ = Math.Min(Position.Z + 1, map.Depth - 1);
            int y = Position.Y;

            for (int x = minX; x <= maxX; x++)
                for (int z = minZ; z <= maxZ; z++)
                    if (x != Position.X || z != Position.Z)
                        if (map.GetStructure(x, y, z) == null)
                            links.Add(new Link(this, map.Nodes[x, y, z], Action.Walk));
        }

        void addFallLink(Map map)
        {
            int x = Position.X;
            int y = Position.Y;
            int z = Position.Z;

            while (y > 0)
            {
                if (map.GetStructure(x, y - 1, z) != null)
                    break;
                y--;
            }

            if (y != Position.Y)
            {
                links.Add(new Link(this, map.Nodes[x, y, z], Action.Fall));
            }
        }

        enum PathfinderStatus : byte
        {
            Unvisited,
            Open,
            Closed,
        }
    }
}
