﻿using Service.Camera.Core.Events;
using System;

namespace Service.Camera.Core.Interfaces
{
    public interface IGrabber : IDisposable
    {
        int CallbackCount { get; }
        event EventHandler<GrabImageEventArgs> ImageCaptured;
        void GrabStart();
        void GrabStop();
    }
}
