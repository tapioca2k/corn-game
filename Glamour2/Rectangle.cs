// finally replacing that wretched Rectangle class

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glamour2
{
    public class Rectangle
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public Vector2 Center
        {
            get { return new Vector2((X + X + Width) / 2, (Y + Y + Height) / 2); }
        }

        public Rectangle(float x, float y, float w, float h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        public Rectangle(Point p, Point size)
        {
            X = p.X;
            Y = p.Y;
            Width = size.X;
            Height = size.Y;
        }

        public bool Intersects(Rectangle other)
        {
            return !(X + Width < other.X || other.X + other.Width < X || 
                Y + Height < other.Y || other.Y + other.Height < Y);
        }

        public bool Contains(Vector2 p)
        {
            return (p.X > X && p.X < (X + Width) &&
                    p.Y > Y && p.Y < (Y + Height));
        }

        public override String ToString()
        {
            return "{X: " + X + " Y: " + Y + " W: " + Width + " H: " + Height + "}";
        }

        public Microsoft.Xna.Framework.Rectangle ToIntRect()
        {
            return new Microsoft.Xna.Framework.Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }

    }
}
