using Syrus.Plugin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Syrus.Core
{
    public interface ISearch
    {
        IEnumerable<Result> Search(string match);
    }

    public class SearchingEngine : ISearch
    {
        private List<ISearch> _searchEngines;

        public SearchingEngine() => _searchEngines = new List<ISearch>();

        public void Add(ISearch searchEngine) => _searchEngines.Add(searchEngine);

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
