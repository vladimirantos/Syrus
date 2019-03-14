using Syrus.Plugin;
using System;
using System.Collections.Generic;


namespace Syrus.Plugins.Weather
{
    public class Main : IPlugin
    {
        private const string ApiKey = "2747ba3ddb5d7e5e4d358c143ce2f61f";
        public void OnInitialize(PluginContext context)
        {
            GetCurrentPosition();
        }

        public IEnumerable<Result> Search(Query query)
        {
            if (!query.HasArguments)
                return new List<Result>();
            return new List<Result>()
            {
                new Result()
                {
                    Text = $"Počasí v {query.Arguments}",
                    QuickResult = $"- {query.Arguments} 19 °C, zataženo"
                }
            };
        }

        private void GetCurrentPosition()
        {
            //var x = geo.Position;
        }
    }
}
