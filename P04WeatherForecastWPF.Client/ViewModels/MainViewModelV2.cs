﻿using CommunityToolkit.Mvvm.ComponentModel;
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
    // 1) dodaje obsluge kolekcji obserowowalnych 
    // 2) od razu implementuje intrefejs inotifyPropertyChanged
    // 3) 
    public class MainViewModelV2 : ObservableObject, IMainViewModel
    {
        private string _cityName = "warszawa";
        // private City[] _cities;
        private City _selectedCity;
        private Weather _weather;

        private readonly IAccuWeatherService _accuWeatherService;

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

        public ObservableCollection<City> Cities {get; set;}

        //public City[] Cities
        //{
        //    get { return _cities; }
        //    set {
        //        _cities = value;
        //        OnPropertyChanged();
        //    }
        //}

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

        public Weather Weather
        {
            get => _weather;
            set
            {
                _weather = value;
                OnPropertyChanged();
                //OnPropertyChanged("CurrentTemperature");
                OnPropertyChanged(nameof(CurrentTemperature));
            }
        }
 
        public string CurrentTemperature => Weather?.Temperature.Metric.Value.ToString();

        public ICommand LoadCitiesCommand { get; }

        public MainViewModelV2(IAccuWeatherService accuWeatherService)
        {
            LoadCitiesCommand = new RelayCommand(x => loadCities());
            _accuWeatherService = accuWeatherService;
            Cities = new ObservableCollection<City>();
        }

        private async void loadCities()
        {
            var cities = await _accuWeatherService.GetLocations(_cityName);
            //Cities = new ObservableCollection<City>(cities);
            Cities.Clear();
            foreach (var city in cities)
                Cities.Add(city);
            //OnPropertyChanged("Cities");
        }

        private async void loadWeather()
        {
            if (SelectedCity != null)
            {
                Weather = await _accuWeatherService.GetCurentConditions(SelectedCity.Key);
            }
        }
    }
}