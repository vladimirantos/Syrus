using Syrus.Core.Caching;
using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Syrus.Core
{
    public class Syrus
    {
        private ILoader _loader;
        private ISearch _search;
        private QueryHistoryCache _searchingHistory = new QueryHistoryCache();

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

        public async Task<IEnumerable<Result>> SearchAsync(string term)
        {
            Query query = Query.FromString(term);
            IEnumerable<PluginPair> selectedPlugins = _search.SelectPlugins(query);
            IEnumerable<Result> results = await _search.Search(query, (ICollection<PluginPair>)selectedPlugins);
            List<Result> resultsList = results.ToList();
            if (resultsList.Count == 0)
            {
                resultsList.AddRange(_search.ConvertPluginsToResult(selectedPlugins));
                resultsList.AddRange(_search.SearchByDefaultPlugins(query));
            }
            else
            {
                _searchingHistory.Add(term);
            }
            return AddIcons(resultsList, query);
        }

        private IEnumerable<Result> AddIcons(IEnumerable<Result> results, Query query)
        {
            foreach (var result in results)
            {
                result.FromQuery = query;
                if (string.IsNullOrEmpty(result.Icon))
                    result.Icon = result.FromPlugin.Icon;
                if (string.IsNullOrEmpty(result.NightIcon))
                    result.NightIcon = result.FromPlugin.NightIcon;
                yield return result;
            }
        }
    }
}
