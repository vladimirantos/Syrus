using Syrus.Plugin;
using System;
using System.Collections.Generic;

namespace Syrus.Core
{
    public class SyrusFactory
    {
        private ILoader _loader;
        private ISearch _search;
        
        public string PluginsLocation { get; private set; }

        public IEnumerable<PluginPair> Plugins { get; private set; }

        public SyrusFactory(ILoader loader, ISearch search) => (_loader, _search) = (loader, search);

        public SyrusFactory(string pluginsLocation) : this(new PluginLoader(pluginsLocation), new SearchEngine())
            => PluginsLocation = pluginsLocation;
        
        public SyrusFactory Initialize()
        {
            return this;
        }

        public SyrusFactory LoadPlugins()
        {
            Plugins = _loader.Load();
            return this;
        }

        public IEnumerable<Result> Search(string term) => new List<Result>();
    }
}
