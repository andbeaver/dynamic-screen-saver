using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Adv_Assignment2
{
    public partial class Form1 : Form
    {
        //-----Polymorphic list of shapes-----//
        private List<Shape> shapes;
        private Timer animationTimer;
        private Random random;

        public Form1()
        {
            InitializeComponent();
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            // Set up form properties
            this.Text = "Screen Saver Simulation";
            this.Size = new Size(800, 600);
            this.DoubleBuffered = true; // Reduces flickering
            this.BackColor = Color.Black;

            // Initialize components
            shapes = new List<Shape>();
            random = new Random();

            AddCircle();
            AddRectangle();
            AddTriangle();
            AddIrregularPolygon();
            AddPictureBoxShape();

            // Set up animation timer
            animationTimer = new Timer();
            animationTimer.Interval = 16; // ~60 FPS
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();

            // Handle resize for proper bouncing
            this.Resize += Form_Resized;


            // Clean up resources on close
            this.FormClosing += (s, e) =>
            {
                foreach (var shape in shapes.ToList())
                {
                    if (shape is PictureBoxShape pic) pic.Dispose();
                    if (shape is Circle c) c.StopColorTimer();
                }
            };
            //---Add shapes at runtime---//
            // Add click event to create more shapes at runtime
            this.MouseClick += (s, e) =>
            {
                var clickPoint = e.Location;
                int shapeType = random.Next(5); // 0, 1, 2, 3, or 4
                
                switch (shapeType)
                {
                    case 0:
                        AddCircle(clickPoint);
                        break;
                    case 1:
                        AddRectangle(clickPoint);
                        break;
                    case 2:
                        AddTriangle(clickPoint);
                        break;
                    case 3:
                        AddPictureBoxShape(clickPoint);
                        break;
                    case 4:
                        AddIrregularPolygon(clickPoint);
                        break;
                }
            };
        } // end InitializeApplication

        //--------Bounce off walls-------//
        //-------Shapes respond to form resize------//
        private void Form_Resized(object sender, EventArgs e)
        {
            // When the form is resized we nudge shapes back into the client area
            NudgeShapesIntoBounds();
            // After nudging, resolve any overlaps created by nudging
            ResolveAllCollisions();
            Invalidate();
        } // end Form_Resized

        private int RandomSpeed() =>
            random.Next(2, 6) * (random.Next(2) == 0 ? 1 : -1);

        private void AddCircle(Point? location = null)
        {
            int radius = random.Next(20, 50);
            int x = location?.X ?? random.Next(100, Math.Max(120, this.ClientSize.Width - 100));
            int y = location?.Y ?? random.Next(100, Math.Max(120, this.ClientSize.Height - 100));

            // clamp center so circle stays fully on screen
            x = Math.Min(Math.Max(radius, x), Math.Max(radius, this.ClientSize.Width - radius));
            y = Math.Min(Math.Max(radius, y), Math.Max(radius, this.ClientSize.Height - radius));

            var circle = new Circle
            {
                X = x,
                Y = y,
                VelX = RandomSpeed(),
                VelY = RandomSpeed(),
                Radius = radius,
                Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256))
            };
            // ensure Width/Height match radius
            circle.Width = circle.Radius * 2;
            circle.Height = circle.Radius * 2;

            shapes.Add(circle);
        } // end AddCircle

        private void AddRectangle(Point? location = null)
        {
            int w = random.Next(30, 80);
            int h = random.Next(30, 80);

            int x = location?.X ?? random.Next(50, Math.Max(130, this.ClientSize.Width - 150));
            int y = location?.Y ?? random.Next(50, Math.Max(130, this.ClientSize.Height - 150));

            // center the rectangle on click
            if (location.HasValue)
            {
                x -= w / 2;
                y -= h / 2;
            }

            // clamp top-left so rectangle stays in client area
            x = Math.Min(Math.Max(0, x), Math.Max(0, this.ClientSize.Width - w));
            y = Math.Min(Math.Max(0, y), Math.Max(0, this.ClientSize.Height - h));

            var rectangle = new Rectangle
            {
                X = x,
                Y = y,
                VelX = RandomSpeed(),
                VelY = RandomSpeed(),
                Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256))
            };
            rectangle.Width = w;
            rectangle.Height = h;

            shapes.Add(rectangle);
        } // end AddRectangle

        private void AddTriangle(Point? location = null)
        {
            int baseLen = random.Next(30, 60);
            int h = random.Next(30, 60);

            int x = location?.X ?? random.Next(50, Math.Max(110, this.ClientSize.Width - 100));
            int y = location?.Y ?? random.Next(50, Math.Max(110, this.ClientSize.Height - 100));

            // center triangle around click
            if (location.HasValue)
            {
                x -= baseLen / 2;
                y -= h / 2;
            }

            x = Math.Min(Math.Max(0, x), Math.Max(0, this.ClientSize.Width - baseLen));
            y = Math.Min(Math.Max(0, y), Math.Max(0, this.ClientSize.Height - h));

            var triangle = new Triangle
            {
                X = x,
                Y = y,
                VelX = RandomSpeed(),
                VelY = RandomSpeed(),
                Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256))
            };
            triangle.BaseLength = baseLen;
            triangle.Height = h;

            shapes.Add(triangle);
        } // end AddTriangle
        //---------------PictureBoxShape---------------//
        private void AddPictureBoxShape(Point? location = null)
        {
            const int pw = 50;
            const int ph = 50;

            // determine position
            int x = location?.X ?? random.Next(50, Math.Max(120, this.ClientSize.Width - 100));
            int y = location?.Y ?? random.Next(50, Math.Max(120, this.ClientSize.Height - 100));

            // center picture box on click
            if (location.HasValue)
            {
                x -= pw / 2;
                y -= ph / 2;
            }

            x = Math.Min(Math.Max(0, x), Math.Max(0, this.ClientSize.Width - pw));
            y = Math.Min(Math.Max(0, y), Math.Max(0, this.ClientSize.Height - ph));

            // Load image from resources
            System.Drawing.Image image = Properties.Resources.football_img;

            var pictureShape = new PictureBoxShape(image, this, pw, ph)
            {
                X = x,
                Y = y,
                VelX = RandomSpeed(),
                VelY = RandomSpeed()
            };
            pictureShape.Width = pw;
            pictureShape.Height = ph;

            shapes.Add(pictureShape);
        } // end AddPictureBoxShape
        //---------------IrregularPolygon---------------//
        private void AddIrregularPolygon(Point? location = null)
        {
            var irregular = new IrregularPolygon();

            // ensure bounding box is known
            irregular.RegeneratePoints();
            int w = irregular.Width;
            int h = irregular.Height;

            int x = location?.X ?? random.Next(50, Math.Max(120, this.ClientSize.Width - 100));
            int y = location?.Y ?? random.Next(50, Math.Max(120, this.ClientSize.Height - 100));

            // center polygon on click
            if (location.HasValue)
            {
                x -= w / 2;
                y -= h / 2;
            }

            x = Math.Min(Math.Max(0, x), Math.Max(0, this.ClientSize.Width - w));
            y = Math.Min(Math.Max(0, y), Math.Max(0, this.ClientSize.Height - h));

            irregular.X = x;
            irregular.Y = y;
            irregular.VelX = RandomSpeed();
            irregular.VelY = RandomSpeed();
            irregular.Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

            shapes.Add(irregular);
        } // end AddIrregularPolygon
        //-------Late binding------//
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // Move all shapes
            for (int i = 0; i < shapes.Count; i++)
            {
                shapes[i].Move(this.ClientSize.Width, this.ClientSize.Height);
            }

            // Collision detection + response
            ResolveAllCollisions();

            Invalidate();
        }
        //----Shape-Shape collision----//
        // Centralized collision resolution used by tick and resize nudging | Polymorphism
        private void ResolveAllCollisions()
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                for (int j = i + 1; j < shapes.Count; j++)
                {
                    var a = shapes[i];
                    var b = shapes[j];

                    if (a.CollidesWith(b))
                    {
                        HandleCollision(a, b);
                    }
                }
            }
        } // end ResolveAllCollisions

        // Collision response
        private void HandleCollision(Shape a, Shape b)
        {
            // Swap velocities
            int tmpVX = a.VelX;
            int tmpVY = a.VelY;
            a.VelX = b.VelX;
            a.VelY = b.VelY;
            b.VelX = tmpVX;
            b.VelY = tmpVY;

            // Visual cue: change colors
            a.Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            b.Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

            // Nudge to reduce sticking - move each by its velocity
            a.X += a.VelX;
            a.Y += a.VelY;
            b.X += b.VelX;
            b.Y += b.VelY;
            //---Visual State Changes---//
            // Trigger visual-state changes per shape type
            if (a is Rectangle rectA) rectA.ToggleGradient();
            if (b is Rectangle rectB) rectB.ToggleGradient();

            if (a is Triangle triA) triA.StartPulse();
            if (b is Triangle triB) triB.StartPulse();

            if (a is PictureBoxShape picA) picA.FlipImage();
            if (b is PictureBoxShape picB) picB.FlipImage();

            if (a is IrregularPolygon polyA) polyA.RegeneratePoints();
            if (b is IrregularPolygon polyB) polyB.RegeneratePoints();
        } // end HandleCollision
        //--------Bounce off walls-------//
        // When the frame shrinks, move shapes that are now outside back inside and flip velocity on the axis nudged
        private void NudgeShapesIntoBounds()
        {
            int panelW = this.ClientSize.Width;
            int panelH = this.ClientSize.Height;

            foreach (var s in shapes)
            {
                if (s == null) continue;

                // Determine bounds
                int minX, minY, maxX, maxY;

                if (s is Circle c)
                {
                    minX = c.X - c.Radius;
                    minY = c.Y - c.Radius;
                    maxX = c.X + c.Radius;
                    maxY = c.Y + c.Radius;

                    // Nudge horizontally
                    if (maxX > panelW)
                    {
                        int overflow = maxX - panelW;
                        c.X -= overflow;
                        c.VelX = -Math.Abs(c.VelX);
                    }
                    else if (minX < 0)
                    {
                        int overflow = -minX;
                        c.X += overflow;
                        c.VelX = Math.Abs(c.VelX);
                    }

                    // Nudge vertically
                    if (maxY > panelH)
                    {
                        int overflow = maxY - panelH;
                        c.Y -= overflow;
                        c.VelY = -Math.Abs(c.VelY);
                    }
                    else if (minY < 0)
                    {
                        int overflow = -minY;
                        c.Y += overflow;
                        c.VelY = Math.Abs(c.VelY);
                    }
                }
                else
                {
                    minX = s.X;
                    minY = s.Y;
                    maxX = s.X + s.Width;
                    maxY = s.Y + s.Height;

                    // Nudge horizontally
                    if (maxX > panelW)
                    {
                        int overflow = maxX - panelW;
                        s.X -= overflow;
                        s.VelX = -Math.Abs(s.VelX);
                    }
                    else if (minX < 0)
                    {
                        int overflow = -minX;
                        s.X += overflow;
                        s.VelX = Math.Abs(s.VelX);
                    }

                    // Nudge vertically
                    if (maxY > panelH)
                    {
                        int overflow = maxY - panelH;
                        s.Y -= overflow;
                        s.VelY = -Math.Abs(s.VelY);
                    }
                    else if (minY < 0)
                    {
                        int overflow = -minY;
                        s.Y += overflow;
                        s.VelY = Math.Abs(s.VelY);
                    }
                }
            }
        } // end NudgeShapesIntoBounds
        //--------Polymorphism  ----//
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw all shapes using polymorphism
            foreach (var obj in shapes)
            {
                var shape = (Shape)obj; //explicit cast to shape
                shape.Draw(e.Graphics);
            }
        } // end OnPaint

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        
    }
}
