using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adv_Assignment2
{
    public abstract class Shape
    {

        // Position
        public int X { get; set; }
        public int Y { get; set; }

        // Velocity
        public int VelX { get; set; }
        public int VelY { get; set; }

        public Color Color { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        // Implement rendering logic for each shape
        public abstract void Draw(Graphics g);

        // Update position based on velocity and handle edge detection
        public virtual void Move(int panelWidth, int panelHeight)
        {
            X += VelX;
            Y += VelY;

            DetectEdge(panelWidth, panelHeight);
        }
        //----Collides with shape detection---///
        // Axis-Aligned Bounding Box (AABB) collision detection 
        public virtual bool CollidesWith(Shape other)
        {
            if (other == null) return false;

            int thisMinX = X;
            int thisMinY = Y;
            int thisMaxX = X + Width;
            int thisMaxY = Y + Height;

            int otherMinX = other.X;
            int otherMinY = other.Y;
            int otherMaxX = other.X + other.Width;
            int otherMaxY = other.Y + other.Height;

            return thisMinX < otherMaxX &&
                   thisMaxX > otherMinX &&
                   thisMinY < otherMaxY &&
                   thisMaxY > otherMinY;
        }
        //---Collides with shape response---//
        // Default collision response that inverts velocities for both shapes so they bounce off each other
        public virtual void ShapeCollide(Shape other)
        {
            if (other == null || ReferenceEquals(this, other)) return;
            // Invert velocities upon collision
            if (this.CollidesWith(other))
            {
                this.VelX = -this.VelX;
                this.VelY = -this.VelY;

                other.VelX = -other.VelX;
                other.VelY = -other.VelY;
            }
        }

        // Parameterless constructor so a shape can be created without initial values
        protected Shape() { }

        // Constructor that sets all properties
        protected Shape(int X, int Y, int width, int height, Color color, int velX = 0, int velY = 0)
        {
            this.X = X;
            this.Y = Y;
            this.Width = width;
            this.Height = height;
            this.Color = color;
            this.VelX = velX;
            this.VelY = velY;
            InitializeVelocityIfZero();
        }

        // Shared Random instance to avoid identical sequences when creating many shapes
        private static readonly Random rng = new Random();

        // Initialize velocity with small random values if both components are zero
        protected void InitializeVelocityIfZero()
        {
            if (VelX == 0 && VelY == 0)
            {
                // ensure we don't end up with (0,0)
                do
                {
                    VelX = rng.Next(-3, 4);
                    VelY = rng.Next(-3, 4);
                } while (VelX == 0 && VelY == 0);
            }
        }

        // Bounce off edges of the panel based on Width/Height
        public virtual void DetectEdge(int panelWidth, int panelHeight)
        {
            if (X <= 0 || X + Width >= panelWidth) VelX *= -1;
            if (Y <= 0 || Y + Height >= panelHeight) VelY *= -1;
        }
    }
}
