﻿namespace Service.Camera.MilX.Interfaces
{
    public interface IMilXFeatureController
    {
        void SetStrFeature(string feature, string strValue);
        double SetDoubleFeature(string feature, double doubleValue);
        string GetStrFeature(string feature);
        double GetDoubleFeature(string feature);
    }
}
