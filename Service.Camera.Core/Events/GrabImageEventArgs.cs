using Service.Camera.Core.Models;
using System;

namespace Service.Camera.Core.Events
{
    public class GrabImageEventArgs : EventArgs
    {
        public byte[] RawData { get; }

        public GrabImageEventArgs(byte[] rawData    )
        {
            RawData = rawData;
        }
    }
}
