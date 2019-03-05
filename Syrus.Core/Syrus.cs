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
        public string CacheLocation { get; private set; }
        public Configuration Configuration { get; private set; }

        public Syrus(string pluginsLocation, string cacheLocation, Configuration configuration)
            : this(new PluginLoader(pluginsLocation), new SearchEngine(configuration), pluginsLocation, cacheLocation, configuration) { }
           
        public Syrus(ILoader loader, ISearch search, string pluginsLocation, string cacheLocation, Configuration configuration) 
            => (_loader, _search, PluginsLocation, CacheLocation, Configuration) = (loader, search, pluginsLocation, cacheLocation, configuration);

        public Syrus LoadPlugins()
        {
            _search.Plugins = _loader.Load().ToList();
            _search.Initialize();
            return this;
        }

        public Syrus Initialize()
        {
            List<Action> actions = new List<Action>();
            foreach(PluginPair p in _search.Plugins)
            {
                actions.Add(() => p.Plugin.OnInitialize(new PluginContext(p.Metadata)
                {
                    CacheLocation = CacheLocation,
                    PluginsLocation = PluginsLocation
                }));
            }
            Parallel.Invoke(actions.ToArray());
            return this;
        }

        public async Task<IEnumerable<Result>> SearchAsync(string term) => await _search.Search(Query.FromString(term));
    }
}
