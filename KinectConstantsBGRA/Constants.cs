using System.Windows.Media;

namespace KinectConstantsBGRA
{
    class Constants
    {

        #region Constants

        /// <summary>
        /// Kinect DPI.
        /// </summary>
        public static readonly double DPI = 96.0;

        /// <summary>
        /// Default format.
        /// </summary>
        public static readonly PixelFormat FORMAT = PixelFormats.Bgr32;

        /// <summary>
        /// Bytes per pixel.
        /// </summary>
        public static readonly int BYTES_PER_PIXEL = (FORMAT.BitsPerPixel + 7) / 8;

        public static readonly int MAP_DEPTH_TO_BYTE = 8000 / 256;

        #endregion

    }
}
