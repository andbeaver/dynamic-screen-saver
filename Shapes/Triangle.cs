using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adv_Assignment2
{
    internal class Triangle : Shape
    {
        public int BaseLength
        {
            get => base.Width;
            set => base.Width = value;
        }
        public new int Height
        {
            get => base.Height;
            set => base.Height = value;
        }

        // pulsing state
        private bool pulsing;
        private int pulseFramesLeft;
        private double pulsePhase;

        public Triangle()
        {
            BaseLength = 60;
            Height = 40;
            Color = Color.Orange;
            pulsing = false;
            pulseFramesLeft = 0;
            pulsePhase = 0.0;
        }

        // Start a short pulse animation (frames)
        public void StartPulse(int frames = 60)
        {
            pulsing = true;
            pulseFramesLeft = frames;
            pulsePhase = 0.0;
        }

        public override void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(this.Color))
            {
                double scale = 1.0;
                if (pulsing)
                {
                    scale = 1.0 + 0.25 * Math.Sin(pulsePhase);
                }
                int drawBase = Math.Max(1, (int)(BaseLength * scale));
                int drawHeight = Math.Max(1, (int)(Height * scale));

                // Keep apex centered horizontally on X + BaseLength/2
                int dx = (drawBase - BaseLength) / 2;

                Point[] points = new Point[3];
                // apex
                points[0] = new Point(X + BaseLength / 2 - dx, Y);
                // bottom left
                points[1] = new Point(X - dx, Y + drawHeight);
                // bottom right
                points[2] = new Point(X + drawBase - dx, Y + drawHeight);

                g.FillPolygon(brush, points);
            }
        }

        public override void Move(int panelWidth, int panelHeight)
        {
            X += VelX;
            Y += VelY;

            // Pulse animation
            if (pulsing)
            {
                pulsePhase += 0.35;
                pulseFramesLeft--;
                if (pulseFramesLeft <= 0) pulsing = false;
            }

            // Bounce using bounding box of the triangle
            if (X < 0 || X + BaseLength > panelWidth)
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
            // keep circle-triangle approximation for better precision
            if (other is Circle circle)
            {
                int triangleCenterX = X + BaseLength / 2;
                int triangleCenterY = Y + Height / 2;
                int triangleRadius = Math.Max(BaseLength, Height) / 2;

                int dx = triangleCenterX - circle.X;
                int dy = triangleCenterY - circle.Y;
                int distanceSquared = dx * dx + dy * dy;
                int radiusSum = triangleRadius + circle.Radius;

                return distanceSquared <= radiusSum * radiusSum;
            }

            // defer to the base AABB fallback for other shapes
            return base.CollidesWith(other);
        }
    }
}
