using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace PwsgForms
{
    [Serializable()]
    public abstract class Furniture
    {
        public Point loc;
        public int angle;
        public float scale = 1;
        [field: NonSerialized()] public bool IsBeingCreated = true;
        [field: NonSerialized()] public bool Selected;
        protected string type;

        public Furniture(Point loc, string type)
        {
            this.loc = loc;
            this.type = type;
        }

        public abstract void Draw(Graphics gr);
        public abstract int? DistanceFrom(Point p);
        public virtual void onClickWhenCreating(MouseEventArgs e){ }
        public virtual void onMoveWhenCreating(MouseEventArgs e){ }
        public override string ToString()
        {
            return $"{Language.Get(type)} {{X={loc.X}, Y={loc.Y}}}";
        }

        public static Furniture Make(Point loc, string type)
        {
            switch (type)
            {
                case "Sofa":
                    return new SimpleFurniture(loc, type, Properties.Resources.sofa);
                case "Table":
                    return new SimpleFurniture(loc, type, Properties.Resources.table);
                case "Kitchen Table":
                    return new SimpleFurniture(loc, type, Properties.Resources.coffee_table);
                case "Bed":
                    return new SimpleFurniture(loc, type, Properties.Resources.double_bed);
                case "Wall":
                    return new WallFurniture(loc);
            }
            throw new ArgumentException("Not existing type of furniture: " + type);
        }

        public Point WorldToObject(Point p)
        {
            Matrix m = new Matrix();
            m.Scale(1.0f / scale, 1.0f / scale);
            m.Rotate(-angle);
            m.Translate(-loc.X, -loc.Y);
            Point[] arr = new Point[] { p };
            m.TransformPoints(arr);
            m.Dispose();
            return arr[0];
        }
    }

    [Serializable()]
    public class SimpleFurniture : Furniture
    {
        private Image image;
        public SimpleFurniture(Point loc, string type, Image image) : base(loc, type)
        {
            this.image = image;
            IsBeingCreated = false;
        }

        public override int? DistanceFrom(Point p)
        {
            if(Math.Abs(p.X) <= image.Width / 2 && Math.Abs(p.Y) <= image.Height / 2)
            {
                return (int)Math.Sqrt(p.X * p.X + p.Y * p.Y);
            }
            return null;
        }

        public override void Draw(Graphics gr)
        {
            int x = -image.Width / 2;
            int y = -image.Height / 2;

            ColorMatrix clrMatrix = new ColorMatrix();
            clrMatrix.Matrix33 = 0.5f;
            ImageAttributes imgAttributes = new ImageAttributes();

            if (Selected)
                imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            gr.DrawImage(image, new Rectangle(x, y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imgAttributes);
            imgAttributes.Dispose();
        }
    }

    [Serializable()]
    public class WallFurniture : Furniture
    {
        private Point newPoint;
        private Point lastPoint = new Point(0, 0);
        [NonSerialized()] private GraphicsPath graphPath = new GraphicsPath();
        private List<Point> points = new List<Point>();
        static Pen blackPen = new Pen(Color.Black, 12);
        static Pen grayPen = new Pen(Color.FromArgb(128, 0, 0, 0), 12);

        public WallFurniture(Point loc) : base(loc, "Wall")
        {
            points.Add(new Point(0, 0));
        }

        static WallFurniture()
        {
            blackPen.MiterLimit = grayPen.MiterLimit = 1.0f;
        }

        [OnDeserialized()] void OnDeserializing(StreamingContext c)
        {
            graphPath = new GraphicsPath();
            graphPath.AddLines(points.ToArray());
        }
        public override int? DistanceFrom(Point p)
        {
            return graphPath.IsOutlineVisible(p, blackPen) ? 0 : (int?)null;
        }

        public override void Draw(Graphics gr)
        {
            GraphicsPath tmpGP = (GraphicsPath)graphPath.Clone();
            if (IsBeingCreated)
            {
                tmpGP.AddLine(lastPoint, newPoint);
            }
            gr.DrawPath(Selected ? grayPen : blackPen, tmpGP);
            tmpGP.Dispose();
        }

        public override void onClickWhenCreating(MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                points.Add(newPoint);
                graphPath.AddLine(lastPoint, newPoint);
                lastPoint = newPoint;
            }
            else if(e.Button == MouseButtons.Right)
            {
                IsBeingCreated = false;
            }
        }

        public override void onMoveWhenCreating(MouseEventArgs e)
        {
            newPoint = WorldToObject(e.Location);
        }
    }
}
