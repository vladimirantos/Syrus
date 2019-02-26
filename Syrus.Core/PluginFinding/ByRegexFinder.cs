using System;
using System.Collections.Generic;
using Syrus.Plugin;

namespace Syrus.Core.PluginFinding
{
    internal class ByRegexFinder : IPluginFinder
    {
        public IEnumerable<PluginPair> Find(string match, out SearchingQuery query)
        {
            throw new NotImplementedException();
        }

        public void Initialize(ICollection<PluginPair> pluginPairs)
        {
            throw new NotImplementedException();
        }
    }
}
