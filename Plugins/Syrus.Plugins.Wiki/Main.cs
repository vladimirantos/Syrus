using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syrus.Plugins.Wiki
{
    public class Main : IPlugin
    {
        public void OnInitialize(PluginContext context)
        {
            System.Console.WriteLine("WIKI");
        }

        public Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            return Task.FromResult<IEnumerable<Result>>(new List<Result>());
        }
    }
}
