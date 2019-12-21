using Syrus.Core.Caching;
using Syrus.Plugin;
using Syrus.Shared.Scheduling;
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
        private QueryHistoryCache _searchingHistory;

        public string PluginsLocation { get; private set; }
        public string CacheLocation { get; private set; }
        public Configuration Configuration { get; private set; }
        public Scheduler TaskScheduler { get; private set; }

        public Syrus(string pluginsLocation, string cacheLocation, Configuration configuration)
            : this(new PluginLoader(pluginsLocation), new SearchEngine(configuration), pluginsLocation, cacheLocation, configuration) { }

        public Syrus(ILoader loader, ISearch search, string pluginsLocation, string cacheLocation, Configuration configuration)
        {
            (_loader, _search, PluginsLocation, CacheLocation, Configuration) = (loader, search, pluginsLocation, cacheLocation, configuration);
            _searchingHistory = new QueryHistoryCache(CacheLocation);
            TaskScheduler = new Scheduler();
        }

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
                actions.Add(() => p.Plugin.OnInitialize(new PluginContext(p.Metadata, TaskScheduler)
                {
                    CacheLocation = CacheLocation,
                    PluginsLocation = PluginsLocation
                }));
            }
            Parallel.Invoke(actions.ToArray());
            return this;
        }

        /// <summary>
        /// Inicializace Scheduleru. Vybere pluginy, která implementují ISchedulable a vytvoří z nich úlohy.
        /// Čas spuštění se získá z Metadata.UpdateInterval
        /// </summary>
        public Syrus InitializeScheduler()
        {
            IEnumerable<PluginPair> schedulables = _search.Plugins.Where(plugin => plugin.Plugin is ISchedulable); //todo otestovat
            foreach (PluginPair schedulable in schedulables)
                TaskScheduler.AddSchedule((schedulable.Plugin as ISchedulable).UpdateAsync, schedulable.Metadata.UpdateInterval);
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
                resultsList.AddRange(_search.ConvertPluginsToResult(selectedPlugins.Where(plugin => plugin.Metadata.EnablePluginResult)));
                resultsList.AddRange(_search.SearchByDefaultPlugins(query));
            }
            else
            {
                _searchingHistory.Add(term);
            }

            return AddIcons(resultsList, query).Take(Configuration.MaxResults);
        }

        public void SaveCache() => _searchingHistory.Save();

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
