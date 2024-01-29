using MatroxCxpCameraExample.ViewModels;
using MatroxCxpCameraExample.Views;
using System.Windows;

namespace MatroxCxpCameraExample
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow = new MainWindow();
            MainWindow.DataContext = new MainViewModel();
            MainWindow.Show();
        }
    }
}
