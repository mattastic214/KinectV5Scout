﻿/**
 *  Tutorial from @Vangos Pterneas https://github.com/Vangos/kinect-2-coordinate-mapping
 *  MIT License
**/

using Microsoft.Kinect;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Runtime.InteropServices;


namespace KinectConstantsBGRA
{
    public static class ColorExtensions
    {

        #region Members

        /// <summary>
        /// The bitmap source.
        /// </summary>
        static WriteableBitmap _bitmap = null;

        /// <summary>
        /// Frame width.
        /// </summary>
        static int _width;

        /// <summary>
        /// Frame height.
        /// </summary>
        static int _height;

        /// <summary>
        /// The RGB pixel values.
        /// </summary>
        static byte[] _pixels = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts a color frame to a System.Media.Imaging.BitmapSource.
        /// </summary>
        /// <param name="frame">The specified color frame.</param>
        /// <returns>The specified frame in a System.Media.Imaging.BitmapSource representation of the color frame.</returns>
        public static BitmapSource ToBitmap(this ColorFrame frame)
        {
            if (_bitmap == null)
            {
                _width = frame.FrameDescription.Width;
                _height = frame.FrameDescription.Height;
                _pixels = new byte[_width * _height * Constants.BYTES_PER_PIXEL];
                _bitmap = new WriteableBitmap(_width, _height, Constants.DPI, Constants.DPI, Constants.FORMAT, null);
            }

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(_pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(_pixels, ColorImageFormat.Bgra);
            }

            _bitmap.Lock();

            Marshal.Copy(_pixels, 0, _bitmap.BackBuffer, _pixels.Length);
            _bitmap.AddDirtyRect(new Int32Rect(0, 0, _width, _height));

            _bitmap.Unlock();

            return _bitmap;
        }

        #endregion
    }
}
