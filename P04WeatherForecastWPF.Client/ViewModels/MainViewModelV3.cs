using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using P04WeatherForecastWPF.Client.Commands;
using P04WeatherForecastWPF.Client.Models;
using P04WeatherForecastWPF.Client.Services;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P04WeatherForecastWPF.Client.ViewModels
{

    // dodalismy WeatherViewModel ktory bindujemy z widokiem 
    public partial class MainViewModelV3 : ObservableObject, IMainViewModel
    {
          
        private City _selectedCity;
      
        private readonly IAccuWeatherService _accuWeatherService;

        public ObservableCollection<City> Cities {get; set;}

        //[ObservableProperty]
        //private Weather weather;
        [ObservableProperty]
        private WeatherViewModel weatherVM;

        [ObservableProperty]
        private string cityName = "warszawa";

        // tutaj korzystamy ze standardowego podjescia bez uzycia bibioteki mvvmtoolkit 
        // poniewaz chcemy miec wieksza kontrole nad tym jak aktualizujemy intrefejs 
        public City SelectedCity
        { 
            get => _selectedCity; 
            set
            {
                _selectedCity = value;
                OnPropertyChanged();
                loadWeather();
            } 
        }
        public string CurrentTemperature => weatherVM?.CurrentTemperature.ToString();

       
        public MainViewModelV3(IAccuWeatherService accuWeatherService)
        {
         
            _accuWeatherService = accuWeatherService; 
            Cities = new ObservableCollection<City>();
        }

        // atrybut relayCommand powoduje ze od razu moge zbindowac te metode z kontrolka w xaml 
        [RelayCommand]
        public async void LoadCities()
        {
            var cities = await _accuWeatherService.GetLocations(cityName);
           
            Cities.Clear();
            foreach (var city in cities)
                Cities.Add(city);
 
        }

        private async void loadWeather()
        {
            if (SelectedCity != null)
            {
                var weather = await _accuWeatherService.GetCurentConditions(SelectedCity.Key);
                WeatherVM = new WeatherViewModel(weather, cityName); // wystarczyło zmienić z małej literki na duża!! :)
                OnPropertyChanged(nameof(CurrentTemperature));
                //OnPropertyChanged(nameof(WeatherVM)); // to do wyjasnienia 
            }
        }
    }
}
