using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Service.Camera.Core.Utils
{
    public class ImageUtil
    {
        public static void SetGrayscalePalette(Bitmap bitmap)
        {
            ColorPalette GrayscalePalette = bitmap.Palette;

            if (GrayscalePalette.Entries.Length != 256)
            {
                var monoBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
                GrayscalePalette = monoBmp.Palette;

                for (int i = 0; i < GrayscalePalette.Entries.Length; i++)
                {
                    GrayscalePalette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = GrayscalePalette;
                return;
            }

            for (int i = 0; i < GrayscalePalette.Entries.Length; i++)
            {
                GrayscalePalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            bitmap.Palette = GrayscalePalette;
        }
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {

            var bitmapImage = new BitmapImage();            // 새 비트맵 이미지 객체 생성
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);               // 메모리 스트림에 bitmap을 Bmp로 저장한다.
                stream.Position = 0;                                // 스트림 포지션 0으로 설정해 처음부터 잡음
                bitmapImage.BeginInit();                            // 비트맵 이미지 초기화
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // 비트맵 이미지가 다 생성되야 stream 닫게 캐시 설정
                bitmapImage.StreamSource = stream;                  // bitmapImage에 할당
                bitmapImage.EndInit();                              // 비트맵 이미지 초기화 종료    
            }
            bitmapImage.Freeze();       // 이미지를 변경할 수 없도록 함 (중요!)
            return bitmapImage;
        }
    }
}
