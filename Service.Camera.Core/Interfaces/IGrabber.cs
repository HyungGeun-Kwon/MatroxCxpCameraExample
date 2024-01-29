using Service.Camera.Core.Events;
using System;

namespace Service.Camera.Core.Interfaces
{
    public interface IGrabber
    {
        event EventHandler<GrabImageEventArgs> ImageCaptured;
        void GrabStart();
        void GrabStop();
    }
}
