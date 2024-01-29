using System;

namespace Service.Camera.Core.Interfaces
{
    public interface ICamera : IDisposable
    {
        IGrabber GrabController { get; }
    }
}
