using Service.Camera.Core.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Service.Camera.Core.Models
{
    public class BitmapDataWrapper : IDisposable
    {
        public Bitmap Bmp { get; }
        private IntPtr _ptrData;
        private readonly byte[] _rawData;
        private readonly int _stride;

        public BitmapDataWrapper(Bitmap bmp, IntPtr ptrData, byte[] rawData, int stride)
        {
            Bmp = bmp;
            _ptrData = ptrData;
            _rawData = rawData;
            _stride = stride;
        }

        public BitmapDataWrapper(byte[] rawData, int width, int height, int stride, PixelFormat pixelFormat)
        {
            _ptrData = Marshal.AllocHGlobal(rawData.Length);
            Marshal.Copy(rawData, 0, _ptrData, rawData.Length);
            Bmp = new Bitmap(width, height, stride, pixelFormat, _ptrData);
        }

        public void SetBitmapGrayscalePalette()
        {
            ImageUtil.SetGrayscalePalette(Bmp);
        }

        public BitmapDataWrapper DeepCopy()
        {
            byte[] rawDataCopy = new byte[_rawData.Length];
            Array.Copy(_rawData, rawDataCopy, _rawData.Length);

            return new BitmapDataWrapper(rawDataCopy, Bmp.Width, Bmp.Height, _stride, Bmp.PixelFormat);
        }

        public void Dispose()
        {
            if (_ptrData != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_ptrData);
                _ptrData = IntPtr.Zero;
            }

            Bmp?.Dispose();
        }
    }
}
