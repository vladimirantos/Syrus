﻿using Syrus.Plugin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;

namespace Syrus.Core
{
    public interface ISearch
    {
        IEnumerable<PluginPair> Plugins { get; set; }
        IEnumerable<Result> Search(string match);
        void Indexing();
    }

    public class SearchEngine : ISearch
    {
        private List<ISearch> _searchEngines;

        /// <summary>
        /// Key is command and value is IPlugin
        /// </summary>
        private Dictionary<string, IPlugin> _commandPlugins = new Dictionary<string, IPlugin>();
        private List<KeyValuePair<string, IPlugin>> _termsPlugins = new List<KeyValuePair<string, IPlugin>>();

        public IEnumerable<PluginPair> Plugins { get; set; }

        public SearchEngine() => _searchEngines = new List<ISearch>();

        public void Add(ISearch searchEngine) => _searchEngines.Add(searchEngine);

        public void Indexing()
        {
            foreach(PluginPair p in Plugins)
            {
                if(p.Metadata.Command != null)
                    _commandPlugins.Add(p.Metadata.Command, p.Plugin);
                foreach (string term in p.Metadata.SearchingPatterns)
                    _termsPlugins.Add(new KeyValuePair<string, IPlugin>(term, p.Plugin));
            }
            _termsPlugins.Sort(new KeyValuePairComparer());
        }

        public IEnumerable<Result> Search(string match)
        {
            Task<IEnumerable<Result>>[] tasks = new Task<IEnumerable<Result>>[_searchEngines.Count];
            for(int i = 0; i < _searchEngines.Count; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => _searchEngines[i].Search(match));
            }
            //return (await Task.WhenAll(tasks)).ToList();
            return new List<Result>();
        }
    }

    class KeyValuePairComparer : IComparer<KeyValuePair<string, IPlugin>>
    {
        public int Compare(KeyValuePair<string, IPlugin> x, KeyValuePair<string, IPlugin> y) => string.Compare(x.Key, y.Key);
    }
}
