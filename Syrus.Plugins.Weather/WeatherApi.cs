using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Syrus.Plugins.Weather
{

    internal class WeatherFactory
    {
        private const string _baseUrl = "http://api.openweathermap.org/data/2.5/weather";
        private string _apiKey;

        public WeatherFactory(string apiKey) => _apiKey = apiKey;

        public WeatherApi GetWeather(string query) => new WeatherApi(JObject.Parse(
                new WebClient().DownloadString($"{_baseUrl}?appid={_apiKey}&q={query}&units=metric")));
    }

    internal class WeatherApi
    {
        public Coord Coord { get; private set; }
        public MainWeather Main { get; private set; }
        public Wind Wind { get; private set; }
        public Rain Rain { get; private set; }
        public Snow Snow { get; private set; }
        public Sys Sys { get; private set; }
        public Clouds Clouds { get; private set; }
        public ICollection<Weather> Weathers { get; private set; } = new List<Weather>();
        public double Visibility { get; private set; }
        public string Name { get; private set; }

        public WeatherApi(JObject json)
        {
            Coord = new Coord(json["coord"]);
            Main = new MainWeather(json["main"]);
            Wind = new Wind(json["wind"]);
            Clouds = new Clouds(json["clouds"]);
            Sys = new Sys(json["sys"]);
            Name = json["name"].ToString();
            Visibility = json.SelectToken("visibility") != null ? double.Parse(json["visibility"].ToString()) : 0;
            foreach (JToken weather in json.SelectToken("weather"))
                Weathers.Add(new Weather(weather));
            if (json.SelectToken("rain") != null)
                Rain = new Rain(json["rain"]);
            if (json.SelectToken("snow") != null)
                Snow = new Snow(json["snow"]);
        }
    }

    internal class Coord
    {
        public double Latitude { get; private set; }
        public  double Longitude { get; private set; }

        public Coord(JToken token)
        {
            Latitude = double.Parse(token["lat"].ToString());
            Longitude = double.Parse(token["lon"].ToString());
        }

        public override string ToString() => $"[{Latitude}, {Longitude}]";
    }

    internal class MainWeather
    {
        public double Temperature { get; }
        public double MinTemperature { get; }
        public double MaxTemperature { get; }
        public double Pressure { get; }
        public double Humidity { get; }

        public MainWeather(JToken token)
        {
            Temperature = double.Parse(token["temp"].ToString());
            MinTemperature = double.Parse(token["temp_min"].ToString());
            MaxTemperature = double.Parse(token["temp_max"].ToString());
            Pressure = double.Parse(token["pressure"].ToString());
            Humidity = double.Parse(token["humidity"].ToString());
        }

        public override string ToString() => $"{Temperature}°C";
    }

    internal class Wind
    {
        internal enum DirectionEnum
        {
            North,
            North_North_East,
            North_East,
            East_North_East,
            East,
            East_South_East,
            South_East,
            South_South_East,
            South,
            South_South_West,
            South_West,
            West_South_West,
            West,
            West_North_West,
            North_West,
            North_North_West,
            Unknown
        }
        public double Speed { get; }
        public double Degree { get; }
        public DirectionEnum Direction { get; }

        public Wind(JToken token)
        {
            Speed = double.Parse(token["speed"].ToString());
            if(token.SelectToken("deg") != null)
                Degree = double.Parse(token.SelectToken("deg").ToString());
            Direction = assignDirection(Degree);
        }

        public string directionEnumToString(DirectionEnum dir)
        {
            switch (dir)
            {
                case DirectionEnum.East:
                    return "East";
                case DirectionEnum.East_North_East:
                    return "East North-East";
                case DirectionEnum.East_South_East:
                    return "East South-East";
                case DirectionEnum.North:
                    return "North";
                case DirectionEnum.North_East:
                    return "North East";
                case DirectionEnum.North_North_East:
                    return "North North-East";
                case DirectionEnum.North_North_West:
                    return "North North-West";
                case DirectionEnum.North_West:
                    return "North West";
                case DirectionEnum.South:
                    return "South";
                case DirectionEnum.South_East:
                    return "South East";
                case DirectionEnum.South_South_East:
                    return "South South-East";
                case DirectionEnum.South_South_West:
                    return "South South-West";
                case DirectionEnum.South_West:
                    return "South West";
                case DirectionEnum.West:
                    return "West";
                case DirectionEnum.West_North_West:
                    return "West North-West";
                case DirectionEnum.West_South_West:
                    return "West South-West";
                case DirectionEnum.Unknown:
                    return "Unknown";
                default:
                    return "Unknown";
            }
        }

        private DirectionEnum assignDirection(double degree)
        {
            if (fB(degree, 348.75, 360))
                return DirectionEnum.North;
            if (fB(degree, 0, 11.25))
                return DirectionEnum.North;
            if (fB(degree, 11.25, 33.75))
                return DirectionEnum.North_North_East;
            if (fB(degree, 33.75, 56.25))
                return DirectionEnum.North_East;
            if (fB(degree, 56.25, 78.75))
                return DirectionEnum.East_North_East;
            if (fB(degree, 78.75, 101.25))
                return DirectionEnum.East;
            if (fB(degree, 101.25, 123.75))
                return DirectionEnum.East_South_East;
            if (fB(degree, 123.75, 146.25))
                return DirectionEnum.South_East;
            if (fB(degree, 168.75, 191.25))
                return DirectionEnum.South;
            if (fB(degree, 191.25, 213.75))
                return DirectionEnum.South_South_West;
            if (fB(degree, 213.75, 236.25))
                return DirectionEnum.South_West;
            if (fB(degree, 236.25, 258.75))
                return DirectionEnum.West_South_West;
            if (fB(degree, 258.75, 281.25))
                return DirectionEnum.West;
            if (fB(degree, 281.25, 303.75))
                return DirectionEnum.West_North_West;
            if (fB(degree, 303.75, 326.25))
                return DirectionEnum.North_West;
            if (fB(degree, 326.25, 348.75))
                return DirectionEnum.North_North_West;
            return DirectionEnum.Unknown;
        }

        //fB = fallsBetween
        private bool fB(double val, double min, double max)
        {
            if ((min <= val) && (val <= max))
                return true;
            return false;
        }
    }

    internal class Rain
    {
        public readonly double H3;

        public Rain(JToken rainData)
        {
            if (rainData.SelectToken("3h") != null)
                H3 = double.Parse(rainData.SelectToken("3h").ToString());
        }
    }

    internal class Snow
    {
        public readonly double H3;

        public Snow(JToken snowData)
        {
            if (snowData.SelectToken("3h") != null)
                H3 = double.Parse(snowData.SelectToken("3h").ToString());
        }
    }

    internal class Sys
    {
        public readonly int Type;
        public readonly int ID;
        public readonly double Message;
        public readonly string Country;
        public readonly DateTime Sunrise;
        public readonly DateTime Sunset;

        public Sys(JToken sysData)
        {
            if (sysData.SelectToken("type") != null)
                Type = int.Parse(sysData.SelectToken("type").ToString());
            if (sysData.SelectToken("id") != null)
                ID = int.Parse(sysData.SelectToken("id").ToString());
            Message = double.Parse(sysData.SelectToken("message").ToString());
            Country = sysData.SelectToken("country").ToString();
            Sunrise = convertUnixToDateTime(double.Parse(sysData.SelectToken("sunrise").ToString()));
            Sunset = convertUnixToDateTime(double.Parse(sysData.SelectToken("sunset").ToString()));
        }

        private DateTime convertUnixToDateTime(double unixTime)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(unixTime).ToLocalTime();
        }
    }

    internal class Clouds
    {
        public readonly double All;

        public Clouds(JToken cloudsData)
        {
            All = double.Parse(cloudsData.SelectToken("all").ToString());
        }
    }

    internal class Weather
    {
        public readonly int ID;
        public readonly string Main;
        public readonly string Description;
        public readonly string Icon;

        public Weather(JToken weatherData)
        {
            ID = int.Parse(weatherData.SelectToken("id").ToString());
            Main = weatherData.SelectToken("main").ToString();
            Description = weatherData.SelectToken("description").ToString();
            Icon = weatherData.SelectToken("icon").ToString();
        }
    }
}
