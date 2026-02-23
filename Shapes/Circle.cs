using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adv_Assignment2
{
    public class Circle : Shape
    {
        public int Radius { get; set; }

        // Timer to change color periodically
        private readonly Timer colorChangeTimer;

        private static readonly Random rng = new Random();

        // Default constructor
        public Circle()
        {
            // sensible default
            Radius = 30;
            Width = Radius * 2;
            Height = Radius * 2;

            // default color
            this.Color = Color.Blue;

            colorChangeTimer = new Timer { Interval = 500 };
            colorChangeTimer.Tick += (s, e) => ChangeColor();
            colorChangeTimer.Start();
        }

        // Convenience constructor that calls the base Shape constructor
        public Circle(int x, int y, int radius, Color color, int velX = 0, int velY = 0)
            : base(x, y, radius * 2, radius * 2, color, velX, velY)
        {
            Radius = radius;
            Width = radius * 2;
            Height = radius * 2;

            colorChangeTimer = new Timer { Interval = 500 };
            colorChangeTimer.Tick += (s, e) => ChangeColor();
            colorChangeTimer.Start();
        }
        private void ChangeColor()
        {
            //Random rand = new Random();
            this.Color = Color.FromArgb(rng.Next(256), rng.Next(256), rng.Next(256));
        }
        public override void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(this.Color))
            {
                g.FillEllipse(brush, X - Radius, Y - Radius, Radius * 2, Radius * 2);
            }
        }

        public override void Move(int panelWidth, int panelHeight)
        {
            X += VelX;
            Y += VelY;

            // Bounce using radius-aware bounds
            if (X - Radius < 0 || X + Radius > panelWidth)
            {
                VelX = -VelX;
            }
            if (Y - Radius < 0 || Y + Radius > panelHeight)
            {
                VelY = -VelY;
            }
        }

        public override bool CollidesWith(Shape other)
        {
            // Circle-circle collision
            if (other is Circle c)
            {
                int dx = X - c.X;
                int dy = Y - c.Y;
                int rSum = Radius + c.Radius;
                return dx * dx + dy * dy <= rSum * rSum;
            }

            // Rect-like shapes (Rectangle, PictureBoxShape)
            if (other is Shape rectLike)
            {
                int closestX = Math.Max(rectLike.X, Math.Min(X, rectLike.X + rectLike.Width));
                int closestY = Math.Max(rectLike.Y, Math.Min(Y, rectLike.Y + rectLike.Height));
                int dx = X - closestX;
                int dy = Y - closestY;
                return dx * dx + dy * dy <= Radius * Radius;
            }

            return base.CollidesWith(other);
        }

        public void StopColorTimer()
        {
            colorChangeTimer?.Stop();
            colorChangeTimer?.Dispose();
        }
    }
}
