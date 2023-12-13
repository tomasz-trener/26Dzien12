using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P04WeatherForecastWPF.Client.ViewModels
{
    internal class MainViewModel
    {
        private string cityName = "warszawa"; 

        public string CityName { get {  return cityName; } set {  cityName = value; } }
    }
}
