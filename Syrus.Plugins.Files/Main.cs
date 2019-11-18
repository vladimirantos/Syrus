using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Syrus.Plugins.Files
{
    public class Main : BasePlugin
    {
        public override void OnInitialize(PluginContext context)
        {
        }

        public async override Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            return Empty;
        }
    }
}
