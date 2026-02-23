using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adv_Assignment2
{
    public class Rectangle : Shape
    {

        private bool useGradient;
        private Color gradientA = Color.LightPink;
        private Color gradientB = Color.HotPink;

        public Rectangle()
        {
            Width = 60;
            Height = 40;
            Color = Color.Red;
            useGradient = false;
        }

        public void ToggleGradient()
        {
            useGradient = !useGradient;
            if (useGradient)
            {
                // pick two random colors for the gradient
                var rnd = new Random();
                gradientA = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                gradientB = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
        }

        public override void Draw(Graphics g)
        {
            if (useGradient)
            {
                using (var brush = new LinearGradientBrush(new RectangleF(X, Y, Width, Height), gradientA, gradientB, LinearGradientMode.ForwardDiagonal))
                {
                    g.FillRectangle(brush, X, Y, Width, Height);
                }
            }
            else
            {
                using (Brush brush = new SolidBrush(this.Color))
                {
                    g.FillRectangle(brush, X, Y, Width, Height);
                }
            }
        }

        public override void Move(int panelWidth, int panelHeight)
        {
            X += VelX;
            Y += VelY;

            // Bounce within panel using Width/Height
            if (X < 0 || X + Width > panelWidth)
            {
                VelX = -VelX;
            }
            if (Y < 0 || Y + Height > panelHeight)
            {
                VelY = -VelY;
            }
        }

        public override bool CollidesWith(Shape other)
        {
            if (other is Rectangle otherRect)
            {
                return X < otherRect.X + otherRect.Width &&
                       X + Width > otherRect.X &&
                       Y < otherRect.Y + otherRect.Height &&
                       Y + Height > otherRect.Y;
            }

            if (other is Circle otherCircle)
            {
                int closestX = Math.Max(X, Math.Min(otherCircle.X, X + Width));
                int closestY = Math.Max(Y, Math.Min(otherCircle.Y, Y + Height));

                int distanceX = otherCircle.X - closestX;
                int distanceY = otherCircle.Y - closestY;

                return (distanceX * distanceX + distanceY * distanceY) <= (otherCircle.Radius * otherCircle.Radius);
            }
            return base.CollidesWith(other);
        }
           
    }
}
