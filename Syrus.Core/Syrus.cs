using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syrus.Core
{
    public class Syrus
    {
        private ILoader _loader;
        private ISearch _search;
        
        public string PluginsLocation { get; private set; }

        public Syrus(string pluginsLocation)
            : this(new PluginLoader(pluginsLocation), new SearchEngine(), pluginsLocation) { }
           
        public Syrus(ILoader loader, ISearch search, string pluginsLocation) 
            => (_loader, _search, PluginsLocation) = (loader, search, pluginsLocation);
        
        public Syrus Initialize()
        {
            List<Action> actions = new List<Action>();
            foreach(PluginPair p in _search.Plugins)
            {
                actions.Add(() => p.Plugin.OnInitialize(new PluginContext(p.Metadata)));
            }
            Parallel.Invoke(actions.ToArray());
            return this;
        }

        public Syrus LoadPlugins()
        {
            _search.Plugins = _loader.Load().ToList();
            return this;
        }

        public IEnumerable<Result> Search(string term) => new List<Result>();
    }
}
