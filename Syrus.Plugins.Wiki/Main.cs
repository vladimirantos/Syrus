using Syrus.Plugin;
using System;
using System.Collections.Generic;

namespace Syrus.Plugins.Wiki
{
    public class Main : IPlugin
    {
        public void OnInitialize(PluginContext context)
        {
            System.Console.WriteLine("WIKI");
        }

        public IEnumerable<Result> Search(Query query)
        {
            return new List<Result>();
        }
    }
}
