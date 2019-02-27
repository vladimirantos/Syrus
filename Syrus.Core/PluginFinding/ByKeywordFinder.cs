﻿using System;
using System.Collections.Generic;
using System.Linq;
using Syrus.Plugin;

namespace Syrus.Core.PluginFinding
{
    internal class ByKeywordFinder : IPluginFinder
    {
        private List<KeyValuePair<string, PluginPair>> _keywordPlugins = new List<KeyValuePair<string, PluginPair>>();

        public void Initialize(ICollection<PluginPair> pluginPairs)
        {
            foreach (PluginPair p in pluginPairs)
            {
                foreach (string keyword in p.Metadata.Keywords)
                    _keywordPlugins.Add(new KeyValuePair<string, PluginPair>(keyword, p));
            }
        }

        public IEnumerable<PluginPair> Find(string match, out SearchingQuery query)
        {
            throw new NotImplementedException();
        }

    }
}
