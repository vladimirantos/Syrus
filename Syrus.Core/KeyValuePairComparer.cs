using System.Collections.Generic;

namespace Syrus.Core
{
    class KeyValuePairComparer : IComparer<KeyValuePair<string, PluginPair>>
    {
        public int Compare(KeyValuePair<string, PluginPair> x, KeyValuePair<string, PluginPair> y) => string.Compare(x.Key, y.Key);
    }
}
