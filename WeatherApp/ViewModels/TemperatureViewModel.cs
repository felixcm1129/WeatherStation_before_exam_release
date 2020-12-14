using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Threading;
using WeatherApp.Commands;
using WeatherApp.Models;

namespace WeatherApp.ViewModels
{
    public class TemperatureViewModel : BaseViewModel
    {
        private TemperatureModel currentTemp;
        private string city;

        public ITemperatureService TemperatureService { get; private set; }

        public DelegateCommand<string> GetTempCommand { get; set; }

        public TemperatureModel CurrentTemp 
        { 
            get => currentTemp;
            set
            {
                currentTemp = value;
                OnPropertyChanged();
                OnPropertyChanged("RawText");
            }
        }

        private ObservableCollection<TemperatureModel> temperatures;

        public ObservableCollection<TemperatureModel> Temperatures
        {
            get { return temperatures; }
            set { 
                temperatures = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                city = value;

                if (TemperatureService != null)
                {
                    TemperatureService.SetLocation(City);
                }

                OnPropertyChanged();
            }
        }

        private string _rawText;

        public string RawText {
            get {
                return _rawText;
            }
            set
            {
                _rawText = value;
                OnPropertyChanged();
            }
            
        }

        public TemperatureViewModel()
        {
            Name = this.GetType().Name;
            Temperatures = new ObservableCollection<TemperatureModel>();

            GetTempCommand = new DelegateCommand<string>(GetTemp, CanGetTemp);
        }


        public bool CanGetTemp(string obj)
        {
            if (TemperatureService == null) return false;
            else return TemperatureService != null;
        }

        public void GetTemp(string obj)
        {
            if (TemperatureService == null) throw new NullReferenceException();

            _ = GetTempAsync();
        }

        private async Task GetTempAsync()
        {
            CurrentTemp = await TemperatureService.GetTempAsync();

            if (CurrentTemp != null)
            {
                RawText = CurrentTemp.ToString() + Environment.NewLine + RawText;
                Debug.WriteLine(CurrentTemp);
            }
        }

        public double CelsiusInFahrenheit(double c)
        {
            return c * 9.0 / 5.0 + 32;
        }

        public double FahrenheitInCelsius(double f)
        {
            return (f - 32) * 5.0 / 9.0;
        }

        public void SetTemperatureService(ITemperatureService srv)
        {
            TemperatureService = srv;
        }
    }
}
