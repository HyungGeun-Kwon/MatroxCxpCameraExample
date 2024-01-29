using Matrox.MatroxImagingLibrary;
using Service.Camera.Core.Events;
using Service.Camera.Core.Interfaces;
using System;

namespace Service.Camera.MilX.Models
{
    public class MilXGrabController : IGrabber
    {
        private MIL_ID _milSystem = MIL.M_NULL;
        private MIL_ID _milDigitizer = MIL.M_NULL;

        private MIL_ID[] _milGrabImage = new MIL_ID[1];
        private MIL_DIG_HOOK_FUNCTION_PTR _captureCallbackDelegate;
        private bool _isGrabbing = false;
        private int _imgWidth;
        private int _imgHeight;

        public int CallbackCount { get; private set; } = 0;
        public event EventHandler<GrabImageEventArgs> ImageCaptured;

        public MilXGrabController(MIL_ID milSystem, MIL_ID milDigitizer)
        {
            _milSystem = milSystem;
            _milDigitizer = milDigitizer;

            SetCallback();
        }

        public void GrabStart()
        {
            if (_isGrabbing) return;

            CallbackCount = 0;
            MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_DISABLE);
            UpdateSize();
            AllocBuffer();
            MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_ENABLE);
            MIL.MdigProcess(_milDigitizer, _milGrabImage, 1, MIL.M_START, MIL.M_DEFAULT, _captureCallbackDelegate, IntPtr.Zero);
            _isGrabbing = true;
        }

        public void GrabStop()
        {
            if (!_isGrabbing) return;

            MIL.MdigProcess(_milDigitizer, _milGrabImage, 1, MIL.M_STOP, MIL.M_DEFAULT, _captureCallbackDelegate, IntPtr.Zero);
            FreeBuffers();

            _isGrabbing = false;
        }

        private void UpdateSize()
        {
            _imgWidth = (int)MIL.MdigInquire(_milDigitizer, MIL.M_SIZE_X, MIL.M_NULL);
            _imgHeight = (int)MIL.MdigInquire(_milDigitizer, MIL.M_SIZE_Y, MIL.M_NULL);
        }

        private void SetCallback()
        {
            _captureCallbackDelegate = new MIL_DIG_HOOK_FUNCTION_PTR(CaptureCallback);
        }

        private void AllocBuffer()
        {
            MIL.MbufAlloc2d(_milSystem, _imgWidth, _imgHeight, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_GRAB + MIL.M_PROC, ref _milGrabImage[0]);
            MIL.MbufClear(_milGrabImage[0], 0xFF);
        }
        private void FreeBuffers()
        {
            MIL.MbufFree(_milGrabImage[0]);
        }

        private MIL_INT CaptureCallback(MIL_INT hookType, MIL_ID hookID, IntPtr UserDataPtr)
        {
            MIL.MdigGetHookInfo(hookID, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref _milGrabImage[0]);
            byte[] byteArr = new byte[_imgWidth * _imgHeight];
            MIL.MbufGet2d(_milGrabImage[0], 0, 0, _imgWidth, _imgHeight, byteArr);
            ImageCaptured?.Invoke(this, new GrabImageEventArgs(byteArr));

            return 0;
        }

        public void Dispose()
        {
            GrabStop();
        }
    }
}
