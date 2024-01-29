using Service.Camera.Core.Models;
using System;

namespace Service.Camera.Core.Events
{
    public class GrabImageEventArgs : EventArgs
    {
        public BitmapDataWrapper BmpWrapper { get; }

        public GrabImageEventArgs(BitmapDataWrapper bmpWrapper)
        {
            BmpWrapper = bmpWrapper;
        }
    }
}
