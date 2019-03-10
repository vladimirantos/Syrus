using Syrus.Plugin;
using System;
using System.Collections.Generic;

namespace Syrus.Plugins.Weather
{
    public class Main : IPlugin
    {
        public void OnInitialize(PluginContext context)
        {
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
    }
}
