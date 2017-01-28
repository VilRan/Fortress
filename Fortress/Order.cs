using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public enum OrderStatus
    {
        Active,
        Interrupted,
        Completed,
    }

    public abstract class Order : IComparable<Order>
    {
        public readonly Unit Unit;
        float priority;

        public Order(Unit unit, float priority)
        {
            Unit = unit;
            this.priority = priority;
        }

        public abstract OrderStatus Execute(float time);

        public int CompareTo(Order other)
        {
            if (priority > other.priority)
                return 1;
            if (priority < other.priority)
                return -1;
            return 0;
        }
    }

    public class MoveOrder : Order
    {
        Point3 destination;
        Stack<Node> path = null;

        public MoveOrder(Unit unit, Point3 destination)
            : base(unit, 1f)
        {
            this.destination = destination;
        }

        public override OrderStatus Execute(float time)
        {
            if (path == null)
            {
                path = Unit.Map.FindPath(Unit.GridPosition, destination, Unit);
                if (path.Count == 0)
                    return OrderStatus.Completed;
            }

            Point3 target = path.Pop().Position;
            Unit.Position = target.ToVector3();

            if (path.Count > 0)
                return OrderStatus.Active;
            return OrderStatus.Completed;
        }
    }
}
