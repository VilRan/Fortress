using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class Cube
    {
        public Quad Up, Forward, Right, Backward, Left, Down;

        public IEnumerable<Vertex> Vertices
        {
            get
            {
                return
                    Up.Vertices
                    .Concat(Forward.Vertices)
                    .Concat(Right.Vertices)
                    .Concat(Backward.Vertices)
                    .Concat(Left.Vertices)
                    .Concat(Down.Vertices);
            }
        }

        public Cube(Vector3 offset, float scale = 1.0f, bool isInverted = false)
        {
            Vector3 luf = scale * 0.5f * new Vector3(-1, 1, -1) + offset;
            Vector3 lub = scale * 0.5f * new Vector3(-1, 1, 1) + offset;
            Vector3 ruf = scale * 0.5f * new Vector3(1, 1, -1) + offset;
            Vector3 rub = scale * 0.5f * new Vector3(1, 1, 1) + offset;
            Vector3 ldf = scale * 0.5f * new Vector3(-1, -1, -1) + offset;
            Vector3 ldb = scale * 0.5f * new Vector3(-1, -1, 1) + offset;
            Vector3 rdf = scale * 0.5f * new Vector3(1, -1, -1) + offset;
            Vector3 rdb = scale * 0.5f * new Vector3(1, -1, 1) + offset;

            if (isInverted)
            {
                Up = new Quad(luf, ruf, lub, rub);
                Forward = new Quad(ldf, rdf, luf, ruf);
                Right = new Quad(rdf, rdb, ruf, rub);
                Backward = new Quad(rdb, ldb, rub, lub);
                Left = new Quad(ldb, ldf, lub, luf);
                Down = new Quad(rdb, rdf, ldb, ldf);
            }
            else
            {
                Up = new Quad(luf, lub, ruf, rub);
                Forward = new Quad(ldf, luf, rdf, ruf);
                Right = new Quad(rdf, ruf, rdb, rub);
                Backward = new Quad(rdb, rub, ldb, lub);
                Left = new Quad(ldb, lub, ldf, luf);
                Down = new Quad(rdb, ldb, rdf, ldf);
            }
        }
    }
}
