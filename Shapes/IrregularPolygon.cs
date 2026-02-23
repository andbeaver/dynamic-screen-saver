using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adv_Assignment2
{
    public class IrregularPolygon : Shape
    {
        public Point[] Points { get; set; }
        public int Size { get; set; }

        private static readonly Random rng = new Random();

        public IrregularPolygon()
        {
            // Create 5-7 random points for an irregular shape
            Points = GenerateRandomPoints();
            Size = 60;

            UpdateBoundingBox();
            Color = Color.Magenta;
        }

        private Point[] GenerateRandomPoints()
        {
            int numPoints = rng.Next(5, 8); // 5 to 7 points
            Point[] points = new Point[numPoints];

            for (int i = 0; i < numPoints; i++)
            {
                double angle = i * 2.0 * Math.PI / numPoints;
                int radius = rng.Next(20, 40);
                int offsetX = (int)(radius * Math.Cos(angle));
                int offsetY = (int)(radius * Math.Sin(angle));
                points[i] = new Point(offsetX, offsetY);
            }

            return points;
        }

        // Public method to change shape (counts as "change shape" visual state)
        public void RegeneratePoints()
        {
            Points = GenerateRandomPoints();
            UpdateBoundingBox();
        }

        // Update Width and Height based on Points
        private void UpdateBoundingBox()
        {
            if (Points == null || Points.Length == 0)
            {
                Width = Height = Size;
                Points = new Point[] { new Point(0, 0) };
                return;
            }

            int minX = Points.Min(p => p.X);
            int maxX = Points.Max(p => p.X);
            int minY = Points.Min(p => p.Y);
            int maxY = Points.Max(p => p.Y);

            Points = Points.Select(p => new Point(p.X - minX, p.Y - minY)).ToArray();

            Width = Math.Max(1, maxX - minX);
            Height = Math.Max(1, maxY - minY);
        }

        public override void Draw(Graphics g)
        {
            if (Points == null || Points.Length == 0) return;

            using (Brush brush = new SolidBrush(this.Color))
            {
                Point[] drawingPoints = Points
                    .Select(p => new Point(X + p.X, Y + p.Y))
                    .ToArray();

                g.FillPolygon(brush, drawingPoints);
            }
        }

        public override void Move(int panelWidth, int panelHeight)
        {
            X += VelX;
            Y += VelY;

            DetectEdge(panelWidth, panelHeight);
        }

        public override bool CollidesWith(Shape other)
        {
            if (other == null) return false;

            // approximate polygon as bounding circle for circle checks
            int polyCenterX = X + (int)Points.Average(p => p.X);
            int polyCenterY = Y + (int)Points.Average(p => p.Y);
            int polyRadius = Math.Max(Width, Height) / 2;
            // Circle-circle collision for better bouncing accuracy
            if (other is Circle circle)
            {
                int dx = polyCenterX - circle.X;
                int dy = polyCenterY - circle.Y;
                int distanceSquared = dx * dx + dy * dy;
                int radiusSum = polyRadius + circle.Radius;
                return distanceSquared <= radiusSum * radiusSum;
            }

            // For other shapes, rely on the base AABB fallback (uses Width/Height).
            // Ensure Width/Height reflect the polygon extents
            UpdateBoundingBox();
            return base.CollidesWith(other);
        }
    }
    }
