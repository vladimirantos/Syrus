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

        public IEnumerable<Result> Search(string searchTerm)
        {
            return new List<Result>();
        }
    }
}
