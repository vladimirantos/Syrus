using Syrus.Plugin;
using System.Collections.Generic;
using System.Net.Http;

namespace Syrus.Plugins.Weather
{
    public class Main : IPlugin
    {
        private const string ApiKey = "2747ba3ddb5d7e5e4d358c143ce2f61f";
        private const string ApiUrl = "http://api.openweathermap.org/data/2.5/";

        private PluginContext _pluginContext;
        private readonly HttpClient _httpClient = new HttpClient();

        public void OnInitialize(PluginContext context)
        {
            _pluginContext = context;

            //api.openweathermap.org/data/2.5/weather?q={city name}&APPID=ApiKey
        }

        public IEnumerable<Result> Search(Query query)
        {
            if (!query.HasArguments || query.Arguments.Length < 3)
                return new List<Result>();
            Load(query.Arguments);
            return new List<Result>()
            {
                new Result()
                {
                    Text = $"Počasí v {query.Arguments}",
                    QuickResult = $"- {query.Arguments} 19 °C, zataženo"
                }
            };
        }

        private async void Load(string x)
        {
            try
            {
                string url = GetCityWeatherUrl(x);
                var a = _pluginContext.JObjectFromHttp(url);
                Coord c = new Coord(a["coord"]);
            }
            catch (HttpRequestException e)
            {

            }
        }

        private void GetCurrentPosition()
        {
            //var x = geo.Position;
        }

        private string GetCityWeatherUrl(string city) => $"{ApiUrl}/weather?q={city}&APPID={ApiKey}";
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
