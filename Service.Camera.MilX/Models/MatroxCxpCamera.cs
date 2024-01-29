using Matrox.MatroxImagingLibrary;
using Service.Camera.Core.Interfaces;
using Service.Camera.MilX.Interfaces;
using System;

namespace Service.Camera.MilX.Models
{
    public class MatroxCxpCamera : IMilXCamera
    {
        private MIL_ID _milApplication = MIL.M_NULL;
        private MIL_ID _milSystem = MIL.M_NULL;
        private MIL_ID _milDigitizer = MIL.M_NULL;

        public IMilXFeatureController MilXFeatureController { get; }
        public IGrabber GrabController { get; }

        public MatroxCxpCamera()
        {
            AllocApplication();
            AllocSystem();
            AllocDigitizer();
            MilXFeatureController = new MilXFeatureController(_milDigitizer);
            GrabController = new MilXGrabController(_milSystem, _milDigitizer);
        }
        private void AllocApplication()
        {
            MIL.MappAlloc(MIL.M_NULL, MIL.M_DEFAULT, ref _milApplication);                           // MIL 애플리케이션을 할당
            MIL.MappControl(_milApplication, MIL.M_ERROR, MIL.M_THROW_EXCEPTION);                    // 예외가 발생하면 MILException을 발생시키도록 함
        }
        private void AllocSystem()
        {
            MIL.MsysAlloc(MIL.M_DEFAULT, "M_DEFAULT", MIL.M_DEFAULT, MIL.M_DEFAULT, ref _milSystem); // MIL 시스템을 할당합니다.
            if (_milSystem == MIL.M_NULL) throw new Exception("SystemID Allocation Fatal");
        }
        private void AllocDigitizer()
        {
            MIL_INT digitizerCount = MIL.MsysInquire(_milSystem, MIL.M_DIGITIZER_NUM, MIL.M_NULL);   // 디지타이저가 몇개 있는 지 찾습니다.
            if (digitizerCount == 0) throw new Exception("Digitizer Not Searched Fatal");

            MIL.MdigAlloc(_milSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_DEFAULT, ref _milDigitizer);  // 디지타이저를 할당합니다.
            if (_milDigitizer == MIL.M_NULL) throw new Exception("DigitizerID Allocate Fatal");
        }
        public void Dispose()
        {
            MIL.MappFreeDefault(_milApplication, _milSystem, MIL.M_NULL, _milDigitizer, MIL.M_NULL);
        }
    }
}
