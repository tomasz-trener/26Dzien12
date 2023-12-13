﻿using P04WeatherForecastWPF.Client.Commands;
using P04WeatherForecastWPF.Client.Models;
using P04WeatherForecastWPF.Client.Services;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P04WeatherForecastWPF.Client.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _cityName = "warszawa";
        private City[] _cities;


        private readonly AccuWeatherService _accuWeatherService;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
            set { 
                _cities = value;
                OnPropertyChanged();
            }
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
            //OnPropertyChanged("Cities");
        }
    }
}
