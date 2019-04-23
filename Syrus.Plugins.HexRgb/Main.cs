using System.Collections.Generic;
using System.Threading.Tasks;
using Syrus.Plugin;

namespace Syrus.Plugins.HexRgb
{
    public class Main : IPlugin
    {
        public void OnInitialize(PluginContext context)
        {
        }

        public Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            return Task.FromResult<IEnumerable<Result>>(new List<Result>()
            {
                new Result()
                {
                    Text = query.Original
                }
            });
        }
    }
}
