﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Syrus.Plugin;
using Syrus.Shared;
using Syrus.Shared.Logging;

namespace Syrus.Plugins.Weather
{
    public sealed class Main : BasePlugin
    {
        private PluginContext _pluginContext;
        private WeatherFactory _weatherFactory;
        private int _requestNumber;

        public override void OnInitialize(PluginContext context)
        {
            _pluginContext = context;
            _weatherFactory = new WeatherFactory(_pluginContext.Metadata.ReadonlyConstants["apiKey"].ToString());
            InitTemplate(nameof(View));
        }

        public async override Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            int count = ++_requestNumber;
            if ((!query.HasArguments && query.Arguments.Length < 2) || count != _requestNumber)
                return Empty;
            try
            {
                WeatherApi weather = await _weatherFactory.GetWeatherAsync(query.Arguments);
                if (weather == null)
                    return Empty;
                foreach(Weather w in weather.Weathers)
                {
                    w.Icon = await WeatherIconDownloader.SaveIcon(w.Icon, Path.Combine(_pluginContext.CacheLocation, _pluginContext.Metadata.FullName));
                }
                return new List<Result>()
                {
                    new Result()
                    {
                        Text = $"Počasí v {query.Arguments.FirstToUpper()}",
                        QuickResult = $"{query.Arguments.FirstToUpper()} {weather.Main}",
                        Content = new View()
                        {
                            Template = ViewTemplate,
                            ViewModel = new WeatherVm()
                            {
                                Weather = weather
                            }
                        }
                    }
                };
            }
            catch(HttpRequestException e) 
            {
            }catch(Exception e)
            {
                Log.Exception("Exception in Weather", e);
            }

            return Empty;
        }
    }
}
