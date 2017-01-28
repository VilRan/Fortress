using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public struct Link
    {
        public readonly Node From;
        public readonly Node To;
        public readonly Action Action;

        public Link(Node from, Node to, Action action)
        {
            From = from;
            To = to;
            Action = action;
        }

        public float GetCost(Unit unit)
        {
            float cost = 0f;

            switch (Action)
            {
                case Action.Walk:
                    cost += (To.Position - From.Position).Length;
                    break;

                case Action.Fall:
                    cost += 0.5f * (From.Position.Y - To.Position.Y);
                    break;

                case Action.Climb:
                    break;

                case Action.UpRamp:
                    break;

                case Action.DownRamp:
                    break;

                case Action.Fly:
                    break;

                case Action.Mine:
                    cost += 30f;
                    goto case Action.Walk;
            }

            return cost;
        }
    }

    public enum Action : byte
    {
        Walk,
        Fall,
        Climb,
        UpRamp,
        DownRamp,
        Fly,
        Mine,
    }
}
