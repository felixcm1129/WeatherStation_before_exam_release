using System.Threading;
using System.Windows;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var lang = WeatherApp.Properties.Settings.Default.Language;
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ApplicationView app = new ApplicationView();


            app.Show();

        }
    }
}
