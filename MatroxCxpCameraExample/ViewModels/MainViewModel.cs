using MatroxCxpCameraExample.Modules.Mvvm;
using Service.Camera.Core.Events;
using Service.Camera.Core.Models;
using Service.Camera.Core.Utils;
using Service.Camera.MilX.Models;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MatroxCxpCameraExample.ViewModels
{
    public class MainViewModel : NotifyBase
    {
        private BitmapImage _mainImage;
        private double _expTime;
        private double _gain;

        private int _width;
        private int _height;
        private double _maxExpTime;
        private double _minExpTime;
        private double _maxGain;
        private double _minGain;

        private MatroxCxpCamera _cam;
        public BitmapImage MainImage { get => _mainImage; set => SetProperty(ref _mainImage, value); }
        public double ExpTime { get => _expTime; set => SetProperty(ref _expTime, value); }
        public double GainRaw { get => _gain; set => SetProperty(ref _gain, value); }

        public ICommand BtnGrabStartClickCommand => new BindingCommand(OnBtnGrabStartClick);
        public ICommand BtnGrabStopClickCommand => new BindingCommand(OnBtnGrabStopClick);
        public ICommand ExpTimeLostFocusCommand => new BindingCommand(OnExpTimeLostFocus);
        public ICommand ExpTimeKeyDownCommand => new BindingCommand<KeyEventArgs>(OnExpTimeKeyDown);
        public ICommand GainLostFocusCommand => new BindingCommand(OnGainLostFocus);
        public ICommand GainKeyDownCommand => new BindingCommand<KeyEventArgs>(OnGainKeyDown);
        public ICommand ClosingCommand => new BindingCommand(OnClosing);

        public MainViewModel()
        {
            _cam = new MatroxCxpCamera();
            _cam.GrabController.ImageCaptured += OnImageProcessed;
            SetCameraDefaultValues();
        }

        private void SetCameraDefaultValues()
        {
            _cam.MilXFeatureController.SetStrFeature("PixelFormat", "Mono8");
            _width = (int)_cam.MilXFeatureController.GetDoubleFeature("Width");
            _height = (int)_cam.MilXFeatureController.GetDoubleFeature("Height");
            ExpTime = _cam.MilXFeatureController.GetDoubleFeature("ExposureTime");
            GainRaw = _cam.MilXFeatureController.GetDoubleFeature("Gain");
            _minExpTime = _cam.MilXFeatureController.GetDoubleFeature("AutoExposureTimeLowerLimit");
            _maxExpTime = _cam.MilXFeatureController.GetDoubleFeature("AutoExposureTimeUpperLimit");
            _minGain = 0;
            _maxGain = 3072;
        }

        private void OnBtnGrabStartClick()
        {
            _cam.GrabController.GrabStart();
        }
        private void OnBtnGrabStopClick()
        {
            _cam.GrabController.GrabStop();
        }
        private void OnExpTimeLostFocus() => ChangeExpTime();
        private void OnGainLostFocus() => ChangeGain();

        private void OnExpTimeKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { ChangeExpTime(); }
        }
        private void OnGainKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { ChangeGain(); }
        }

        private void ChangeExpTime()
        {
            double setValue = new double[2] { ExpTime, _minExpTime }.Max();
            setValue = new double[2] { setValue, _maxExpTime }.Min();
            ExpTime = _cam.MilXFeatureController.SetDoubleFeature("ExposureTime", setValue);
        }
        private void ChangeGain()
        {
            double setValue = new double[2] { GainRaw, _minGain }.Max();
            setValue = new double[2] { setValue, _maxGain }.Min();
            GainRaw = _cam.MilXFeatureController.SetDoubleFeature("Gain", setValue);
        }

        private void OnImageProcessed(object sender, GrabImageEventArgs e)
        {
            BitmapDataWrapper bitmapDataWrapper = new BitmapDataWrapper(e.RawData, _width, _height, _width, PixelFormat.Format8bppIndexed);
            bitmapDataWrapper.SetBitmapGrayscalePalette();
            MainImage = ImageUtil.BitmapToBitmapImage(bitmapDataWrapper.Bmp);
            bitmapDataWrapper.Dispose();
        }

        private void OnClosing()
        {
            _cam.Dispose();
        }
    }
}
