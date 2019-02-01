using Syrus.Plugin;
using System.Collections.Generic;

namespace Syrus.Core
{
    public class SyrusFactory
    {
        private ILoader _loader;
        private ISearch _search;
        
        public string PluginsLocation { get; private set; }

        public SyrusFactory(ILoader loader, ISearch search) => (_loader, _search) = (loader, search);

        public SyrusFactory(string pluginsLocation) : this(new PluginLoader(), new SearchEngine())
            => (PluginsLocation) = (pluginsLocation);
        
        public void Initialize()
        {
            _loader.Load(PluginsLocation);
        }

        public IEnumerable<Result> Search(string term) => new List<Result>();

        private IEnumerable<IPlugin> LoadPlugins() => new List<IPlugin>();
    }
}
