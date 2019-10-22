using System.Collections.Generic;
using System.Threading.Tasks;
using Syrus.Plugin;

namespace Syrus.Plugins.Weather
{
    public sealed class Main : BasePlugin
    {
        private PluginContext _pluginContext; 

        public override void OnInitialize(PluginContext context)
        {
            _pluginContext = context;
            InitTemplate(nameof(View));
            //ViewTemplate = CreateTemplate(nameof(View));
        }

        public override Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            throw new System.NotImplementedException();
        }
    }
}
