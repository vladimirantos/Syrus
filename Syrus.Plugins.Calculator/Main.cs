using System.Collections.Generic;
using Syrus.Plugin;

namespace Syrus.Plugins.Calculator
{
    public class Main : IPlugin
    {
        public void OnInitialize(PluginContext context)
        {
        }

        public IEnumerable<Result> Search(Query query)
        {
            return new List<Result>();
        }
    }
}
