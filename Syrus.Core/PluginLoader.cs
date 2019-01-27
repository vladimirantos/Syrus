using Syrus.Plugin;
using System.Collections.Generic;

namespace Syrus.Core
{
    public class PluginLoader
    {
        public IEnumerable<IPlugin> Load(string pluginsLocation) => new List<IPlugin>();
    }
}
