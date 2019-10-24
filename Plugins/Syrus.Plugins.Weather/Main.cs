using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syrus.Plugin;
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
            if (!query.HasArguments || count != _requestNumber)
                return Empty;
            try
            {
                WeatherApi weather = await _weatherFactory.GetWeatherAsync(query.Arguments);
                return new List<Result>()
                {
                    new Result()
                    {
                        Text = $"Počasí v {query.Arguments}",
                        QuickResult = $"- {query.Arguments} {weather.Main}",
                        Content = new View()
                        {
                            Template = ViewTemplate,
                            ViewModel = new WeatherVm()
                            {
                                City = query.Arguments,
                                Temperature = $"{weather.Main.Temperature} °C"
                            }
                        }
                    }
                };
            }
            catch(Exception e)
            {
                Log.Exception("Exception in Weather", e);
            }
            return Empty;
        }
    }
}
