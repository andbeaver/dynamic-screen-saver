using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adv_Assignment2
{
    internal class PictureBoxShape : Shape
    {
        private PictureBox pictureBox;
        private Control parentControl;
        private bool disposed;
        private bool flipped;

        public PictureBoxShape(Image image, Control parentControl, int width = 50, int height = 50)
        {
            if (parentControl == null) throw new ArgumentNullException(nameof(parentControl));
            this.parentControl = parentControl;

            // Initialize size on base class so CollidesWith / DetectEdge work
            this.Width = Math.Max(1, width);
            this.Height = Math.Max(1, height);

            pictureBox = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(this.Width, this.Height),
                BackColor = Color.Transparent,
                Location = new Point(this.X, this.Y),
                // allow clicks to fall through to the form so shapes are added at the mouse location
                Enabled = false
            };

            parentControl.Controls.Add(pictureBox);
            flipped = false;
        }

        public void FlipImage()
        {
            if (pictureBox?.Image == null) return;
            // toggle horizontal flip
            pictureBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            pictureBox.Refresh();
            flipped = !flipped;
        }

        public override void Draw(Graphics g)
        {
            
            if (pictureBox != null)
            {
                pictureBox.Size = new Size(Width, Height);
                pictureBox.Location = new Point(X, Y);
            }
        }

        public override void Move(int panelWidth, int panelHeight)
        {
            X += VelX;
            Y += VelY;

            // Bounce using base Bounds (Width/Height) then update control position
            if (X < 0 || X + Width > panelWidth) VelX = -VelX;
            if (Y < 0 || Y + Height > panelHeight) VelY = -VelY;

            if (pictureBox != null)
            {
                pictureBox.Location = new Point(X, Y);
            }
        }

        public override bool CollidesWith(Shape other)
        {
            if (other == null) return false;

            // Rectangle-circle special case (closest-point test)
            if (other is Circle circle)
            {
                int closestX = Math.Max(X, Math.Min(circle.X, X + Width));
                int closestY = Math.Max(Y, Math.Min(circle.Y, Y + Height));

                int distanceX = circle.X - closestX;
                int distanceY = circle.Y - closestY;

                return (distanceX * distanceX + distanceY * distanceY) <= (circle.Radius * circle.Radius);
            }

            // For all other shapes use the base AABB collision logic
            return base.CollidesWith(other);
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            if (pictureBox != null)
            {
                if (parentControl != null && parentControl.Controls.Contains(pictureBox))
                    parentControl.Controls.Remove(pictureBox);

                pictureBox.Image?.Dispose();
                pictureBox.Dispose();
                pictureBox = null;
            }
        }
    }
}
