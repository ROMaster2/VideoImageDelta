using ImageMagick;
using System;

namespace VideoImageDeltaApp.Models
{
    public struct GeometryOld
    {
        public GeometryOld(double width, double height, Anchor anchor = Anchor.Undefined)
        {
            X = 0;
            Y = 0;
            Width = width;
            Height = height;
            Anchor = anchor;
        }
        public GeometryOld(double x, double y, double width, double height, Anchor anchor = Anchor.Undefined)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Anchor = anchor;
        }

        public double X;
        public double Y;
        public double Width;
        public double Height;
        public Anchor Anchor;

        public bool HasPoint()
        {
            return (X != 0) || (Y != 0);
        }

        public bool HasSize()
        {
            return (Width != 0) && (Height != 0);
        }

        public bool HasAnchor()
        {
            return Anchor != Anchor.Undefined;
        }

        public bool IsNull()
        {
            return !HasPoint() && !HasSize() && !HasAnchor();
        }

        public double Ratio
        {
            get
            {
                if (HasSize())
                {
                    return Math.Abs(Width / Height);
                }
                else
                {
                    throw new ArgumentException("Width and Height must have a value to get the ratio.");
                }
            }
        }

        public GeometryOld WithoutAnchor(GeometryOld baseGeometry)
        {
            double _x;
            double _y;

            if (Anchor == Anchor.Undefined ||
                Anchor.HasFlag(Anchor.Left))
            {
                _x = X;
            }
            else if (Anchor.HasFlag(Anchor.Right))
            {
                _x = baseGeometry.Width - Width + X;
            }
            else
            {
                _x = (baseGeometry.Width - Width) / 2d + X;
            }

            if (Anchor == Anchor.Undefined ||
                Anchor.HasFlag(Anchor.Top))
            {
                _y = Y;
            }
            else if (Anchor.HasFlag(Anchor.Bottom))
            {
                _y = baseGeometry.Height - Height + Y;
            }
            else
            {
                _y = (baseGeometry.Height - Height) / 2d + Y;
            }

            return new GeometryOld(_x, _y, Width, Height, Anchor.Undefined);
        }

        // Only Northwest for now
        public void UpdateRelativeToPoint(double x, double y)
        {
            X = X - x;
            Y = Y - y;
        }
        public void UpdateRelativeToPoint(GeometryOld geo)
        {
            X = X - geo.X;
            Y = Y - geo.Y;
        }
        public GeometryOld RelativeToPoint(double x, double y)
        {
            return new GeometryOld(Width, Height)
            {
                X = X - x,
                Y = Y - y
            };
        }
        public GeometryOld RelativeToPoint(GeometryOld geo)
        {
            return new GeometryOld(Width, Height)
            {
                X = X - geo.X,
                Y = Y - geo.Y
            };
        }

        // Todo: Handle fractional pixels.

        public MagickGeometry ToMagick()
        {
            return new MagickGeometry((int)X, (int)Y, (int)Width, (int)Height);
        }

        public Tesseract.Rect ToTesseract()
        {
            return new Tesseract.Rect((int)X, (int)Y, (int)Width, (int)Height);
        }

        public string ToFFmpegString()
        {
            return Width.ToString() + ':' + Height.ToString() + ':' + X.ToString() + ':' + Y.ToString();
        }

    }

}
