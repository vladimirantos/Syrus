using System;
using System.Collections.Generic;
using System.Text;
using Syrus.Plugin;

namespace Syrus.Core.PluginFinding
{
    internal class ByCommandFinder : IPluginFinder
    {
        public IEnumerable<PluginPair> Find(string match, out ParsedSearchingPattern searchingPattern)
        {
            throw new NotImplementedException();
        }
    }
}
