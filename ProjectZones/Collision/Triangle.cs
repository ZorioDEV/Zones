using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectZones.Collision
{
    public struct Triangle
    {
        public Vector2 Vertex1;
        public Vector2 Vertex2;
        public Vector2 Vertex3;

        public Triangle(Vector2 v1, Vector2 v2, Vector2 v3)
        {
            Vertex1 = v1;
            Vertex2 = v2;
            Vertex3 = v3;
        }

        public Vector2[] GetVertices()
        {
            return new Vector2[] { Vertex1, Vertex2, Vertex3 };
        }
    }
}
