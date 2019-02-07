using Syrus.Plugin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
        private Dictionary<string, IPlugin> _commandPlugins = new Dictionary<string, IPlugin>();
        private Dictionary<string[], IEnumerable<IPlugin>> _termsPlugins = new Dictionary<string[], IEnumerable<IPlugin>>();
        public IEnumerable<PluginPair> Plugins { get; set; }

        public SearchEngine() => _searchEngines = new List<ISearch>();

        public void Add(ISearch searchEngine) => _searchEngines.Add(searchEngine);

        public void Indexing()
        {
            foreach(PluginPair p in Plugins)
            {
                _commandPlugins.Add(p.Metadata.Command, p.Plugin);
            }
        }

        public async IEnumerable<Result> Search(string match)
        {
            Task<IEnumerable<Result>>[] tasks = new Task<IEnumerable<Result>>[_searchEngines.Count];
            for(int i = 0; i < _searchEngines.Count; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => _searchEngines[i].Search(match));
            }
            return (await Task.WhenAll(tasks)).ToList();
        }
    }
}
