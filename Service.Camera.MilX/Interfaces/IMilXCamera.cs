using Service.Camera.Core.Interfaces;

namespace Service.Camera.MilX.Interfaces
{
    public interface IMilXCamera : ICamera
    {
        IMilXFeatureController MilXFeatureController { get; }
    }
}
