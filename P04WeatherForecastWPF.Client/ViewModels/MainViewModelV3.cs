﻿using CommunityToolkit.Mvvm.ComponentModel;
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

    // ObservableObject - z biblioteki CommunityToolkit
    // 1) dodaje obsluge kolekcji obserowowalnych (ObservableCollection)
    // 2) od razu implementuje intrefejs inotifyPropertyChanged (nie musze korzystać z base viewmodel)
    // 3) mamy mozliowosc uproszczenia deklaracji pól i właściwości poprzez uzycie ObservableProperty ale uwaga - ono wymaga tego aby klasa była partial 
    // 4) ma zaimplementowana obsluge zdarzen [RelayCommand] 
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
                weatherVM = new WeatherViewModel(weather, cityName);
                OnPropertyChanged(nameof(CurrentTemperature));
                OnPropertyChanged(nameof(WeatherVM)); // to do wyjasnienia 
            }
        }
    }
}
