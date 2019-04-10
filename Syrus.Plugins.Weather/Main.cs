using Syrus.Plugin;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Syrus.Plugins.Weather
{
    public class Main : IPlugin
    {
        private const string ApiKey = "2747ba3ddb5d7e5e4d358c143ce2f61f";

        private PluginContext _pluginContext;
        private WeatherFactory _weatherFactory;
        private ResourceDictionary ViewTemplate { get; set; } = new ResourceDictionary();
        public void OnInitialize(PluginContext context)
        {
            _pluginContext = context;
            _weatherFactory = new WeatherFactory(ApiKey);
            ViewTemplate.Source = new Uri("pack://application:,,,/Pokus;component/View.xaml", UriKind.Absolute);
        }

        public IEnumerable<Result> Search(Query query)
        {
            if (!query.HasArguments || query.Arguments.Length < 3)
                return new List<Result>();
            try
            {
                WeatherApi weather = _weatherFactory.GetWeather(query.Arguments);
                return new List<Result>()
                {
                    new Result()
                    {
                        Text = $"Počasí v {query.Arguments}",
                        QuickResult = $"- {query.Arguments} {weather.Main}",
                        Content = new View()
                        {
                            Template = 
                        }
                    }
                };
            }
            catch (WebException)
            {

            }
            return new List<Result>();
        }
    }


    public class WeatherVm : BaseViewModel
    {
        public string City { get; set; }
        public string Temperature { get; set; }
    }

}


// {{
//  "coord": {
//    "lon": 14.42,
//    "lat": 50.09
//  },
//  "weather": [
//    {
//      "id": 803,
//      "main": "Clouds",
//      "description": "broken clouds",
//      "icon": "04n"
//    }
//  ],
//  "base": "stations",
//  "main": {
//    "temp": 276.98,
//    "pressure": 1018,
//    "humidity": 83,
//    "temp_min": 275.93,
//    "temp_max": 278.71
//  },
//  "visibility": 10000,
//  "wind": {
//    "speed": 4.6,
//    "deg": 300
//  },
//  "clouds": {
//    "all": 75
//  },
//  "dt": 1553543643,
//  "sys": {
//    "type": 1,
//    "id": 6848,
//    "message": 0.0051,
//    "country": "CZ",
//    "sunrise": 1553489706,
//    "sunset": 1553534496
//  },
//  "id": 3067696,
//  "name": "Prague",
//  "cod": 200
//}}
//}
