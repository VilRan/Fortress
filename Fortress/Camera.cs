using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class Camera
    {
        public Matrix View;
        public Matrix Projection;
        public BoundingFrustum Frustrum;
        readonly Map map;
        Vector3 position = new Vector3(0, 13, 0);
        int floor = 3;

        public int Floor
        {
            get
            {
                return floor;
            }

            set
            {
                value = MathHelper.Clamp(value, 0, map.Height - 1);
                int delta = value - floor;
                position += Vector3.Up * delta;
                floor = value;
            }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = Vector3.Clamp(value, minPosition, maxPosition); }
        }

        Vector3 minPosition { get { return new Vector3(-5, -10, -5); } }
        Vector3 maxPosition { get { return new Vector3(map.Width + 5, map.Height + 10, map.Depth + 5); } }

        public Camera(Map map, float fieldOfView, float aspectRatio)
        {
            this.map = map;
            Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, 0.01f, 100f);
        }

        public void Update()
        {
            Vector3 front = Vector3.Normalize(new Vector3(1, -3, 1));
            Vector3 target = position + front;
            Vector3 up = Vector3.Normalize(new Vector3(1, 1 / 3f, 1));
            View = Matrix.CreateLookAt(position, target, up);
            Frustrum = new BoundingFrustum(View * Projection);
        }
    }
}
