using Syrus.Plugin;
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
        private Dictionary<string, PluginPair> _commandPlugins = new Dictionary<string, PluginPair>();
        private List<KeyValuePair<string, PluginPair>> _termsPlugins = new List<KeyValuePair<string, PluginPair>>();
        private List<KeyValuePair<string, PluginPair>> _regexPlugins = new List<KeyValuePair<string, PluginPair>>();
        private int _maxCommandLength = 0;

        public IEnumerable<PluginPair> Plugins { get; set; }

        public SearchEngine() => _searchEngines = new List<ISearch>();

        public void Add(ISearch searchEngine) => _searchEngines.Add(searchEngine);

        public void Indexing()
        {
            foreach(PluginPair p in Plugins)
            {
                if(p.Metadata.Command != null)
                {
                    if (_maxCommandLength < p.Metadata.Command.Length)
                        _maxCommandLength = p.Metadata.Command.Length;
                    _commandPlugins.Add(p.Metadata.Command.ToLower(), p);
                }
                foreach (SearchingPattern term in p.Metadata.SearchingPatterns)
                {
                    if (!term.IsRegex)
                        _termsPlugins.Add(new KeyValuePair<string, PluginPair>(term.Text.ToLower(), p));
                    else _regexPlugins.Add(new KeyValuePair<string, PluginPair>(term.Text.ToLower(), p));
                }
            }
            _termsPlugins.Sort(new KeyValuePairComparer());
            _regexPlugins.Sort(new KeyValuePairComparer());
        }

        public IEnumerable<Result> Search(string match)
        {
            match = match.ToLower();
            IEnumerable<PluginPair> plugins = SelectPlugins(match);
            List<Result> results = new List<Result>();
            foreach (var plugin in plugins)
                results.AddRange(plugin.Plugin.Search(match));

            //Task<IEnumerable<Result>>[] tasks = new Task<IEnumerable<Result>>[_searchEngines.Count];
            //for(int i = 0; i < _searchEngines.Count; i++)
            //{
            //    tasks[i] = Task.Factory.StartNew(() => _searchEngines[i].Search(match));
            //}
            ////return (await Task.WhenAll(tasks)).ToList();

            if (results.Count == 0)
                results.AddRange(ResultsFromPlugins(plugins));
            return results;
        }

        private IEnumerable<PluginPair> SelectPlugins(string match)
        {
            List<PluginPair> plugins = new List<PluginPair>();
            if(_maxCommandLength >= match.Length)
                plugins.AddRange(_commandPlugins.Where(kv => kv.Key.StartsWith(match)).Select(kv => kv.Value));
            return plugins;
        }

        private IEnumerable<Result> ResultsFromPlugins(IEnumerable<PluginPair> plugins)
        {
            List<Result> results = new List<Result>();
            foreach(PluginPair p in plugins)
            {
                results.Add(new Result()
                {
                    Title = p.Metadata.Name
                });
            }
            return results;
        }
    }

    class KeyValuePairComparer : IComparer<KeyValuePair<string, PluginPair>>
    {
        public int Compare(KeyValuePair<string, PluginPair> x, KeyValuePair<string, PluginPair> y) => string.Compare(x.Key, y.Key);
    }
}
