
using Syrus.Plugin;
using System.Collections.Generic;

namespace Syrus.Core.PluginFinding
{
    internal interface IPluginFinder
    {
        void Initialize();

        IEnumerable<PluginPair> Find(string match, out ParsedSearchingPattern searchingPattern);
    }
}
