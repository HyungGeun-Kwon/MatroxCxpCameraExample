using Matrox.MatroxImagingLibrary;
using Service.Camera.MilX.Interfaces;
using System.Text;

    namespace Service.Camera.MilX.Models
{
    public class MilXFeatureController : IMilXFeatureController
    {
        private MIL_ID _milDigitizer;
        public MilXFeatureController(MIL_ID milDigitizer)
        {
            _milDigitizer = milDigitizer;
        }

        public void SetStrFeature(string feature, string strValue)
        {
            MIL.MdigControlFeature(_milDigitizer, MIL.M_FEATURE_VALUE, feature, MIL.M_TYPE_STRING, strValue);
        }
        public double SetDoubleFeature(string feature, double doubleValue)
        {
            MIL.MdigControlFeature(_milDigitizer, MIL.M_FEATURE_VALUE, feature, MIL.M_TYPE_DOUBLE, ref doubleValue);
            return doubleValue;
        }
        public string GetStrFeature(string feature)
        {
            StringBuilder reVal = new StringBuilder();
            MIL.MdigInquireFeature(_milDigitizer, MIL.M_FEATURE_VALUE, feature, MIL.M_TYPE_STRING, reVal);
            return reVal.ToString();
        }
        public double GetDoubleFeature(string feature)
        {
            double reVal = 0;
            MIL.MdigInquireFeature(_milDigitizer, MIL.M_FEATURE_VALUE, feature, MIL.M_TYPE_DOUBLE, ref reVal);
            return reVal;
        }
    }
}
