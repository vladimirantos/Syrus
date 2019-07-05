using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Syrus.Plugins.Translate
{
    class Main : IPlugin
    {
        public void OnInitialize(PluginContext context)
        {
            System.Console.WriteLine("TRANSLATE");
        }

        public Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            return Task.FromResult<IEnumerable<Result>>(new List<Result>());
        }
    }
}
