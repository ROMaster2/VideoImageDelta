namespace VideoImageDeltaApp.Models
{
    public partial struct Geometry
    {
        #region Constructors

        // Additional constructors for conversion.

        /// <summary>
        /// Constructor for converting Tesseract.Rect to this format.
        /// </summary>
        public Geometry(Tesseract.Rect rect, Anchor anchor = Anchor.Undefined)
             : this(rect.X1, rect.Y1, rect.Width, rect.Height, anchor) { }

        /// <summary>
        /// Constructor for converting ImageMagick.MagickGeometry to this format.
        /// </summary>
        public Geometry(ImageMagick.MagickGeometry mGeo,
                    Anchor anchor = Anchor.Undefined)
            : this(mGeo.X, mGeo.Y, mGeo.Width, mGeo.Height, anchor) { }

        /// <summary>
        /// Constructor for converting ImageMagick.MagickGeometry and Gravity to this format.
        /// </summary>
        public Geometry(ImageMagick.MagickGeometry mGeo,
                    ImageMagick.Gravity gravity = ImageMagick.Gravity.Undefined)
            : this(mGeo.X, mGeo.Y, mGeo.Width, mGeo.Height, gravity.ToAnchor()) { }

        #endregion Constructors

        #region Public Methods

        public string ToFFmpegString()
        {
            return Width.ToString() + ':' + Height.ToString() + ':' + X.ToString() + ':' + Y.ToString();
        }

        // Todo: Handle fractional pixels.

        public ImageMagick.MagickGeometry ToMagick()
        {
            return new ImageMagick.MagickGeometry((int)X, (int)Y, (int)Width, (int)Height);
        }

        public Tesseract.Rect ToTesseract()
        {
            return new Tesseract.Rect((int)X, (int)Y, (int)Width, (int)Height);
        }

        #endregion Public Methods

    }
}
