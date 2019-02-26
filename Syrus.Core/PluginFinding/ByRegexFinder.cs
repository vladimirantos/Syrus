using System;
using System.Collections.Generic;
using Syrus.Plugin;

namespace Syrus.Core.PluginFinding
{
    internal class ByRegexFinder : IPluginFinder
    {
        private List<KeyValuePair<string, PluginPair>> _regexPlugins = new List<KeyValuePair<string, PluginPair>>();

        public void Initialize(ICollection<PluginPair> pluginPairs)
        {
            foreach (PluginPair p in pluginPairs)
                foreach (SearchingPattern searchingPattern in p.Metadata.RegexPatterns)
                    _regexPlugins.Add(new KeyValuePair<string, PluginPair>(searchingPattern.Text, p));
            _regexPlugins.Sort(new KeyValuePairComparer());
        }

        public IEnumerable<PluginPair> Find(string match, out SearchingQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
