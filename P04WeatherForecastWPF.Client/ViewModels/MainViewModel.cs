using P04WeatherForecastWPF.Client.Commands;
using P04WeatherForecastWPF.Client.Models;
using P04WeatherForecastWPF.Client.Services;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P04WeatherForecastWPF.Client.ViewModels
{
    internal class MainViewModel
    {
        private string _cityName = "warszawa";
        private City[] _cities;


        private readonly AccuWeatherService _accuWeatherService;

        public string CityName 
        { 
            get 
            {  
                return _cityName; 
            }
        set 
            {  
                _cityName = value;
            } 
        }

        public City[] Cities
        {
            get { return _cities; }
            set { _cities = value; }
        }


        public ICommand LoadCitiesCommand { get; }

        public MainViewModel()
        {
            LoadCitiesCommand = new RelayCommand(x => LoadCities());
            _accuWeatherService = new AccuWeatherService();
        }

        public async void LoadCities()
        {
            Cities = await _accuWeatherService.GetLocations(_cityName); 
        }
    }
}
