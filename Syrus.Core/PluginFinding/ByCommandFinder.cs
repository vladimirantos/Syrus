﻿using System;
using System.Collections.Generic;
using Syrus.Plugin;

namespace Syrus.Core.PluginFinding
{
    internal class ByCommandFinder : IPluginFinder
    {
        public void Initialize(ICollection<PluginPair> pluginPairs)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PluginPair> Find(string match, out SearchingQuery query)
        {
            throw new NotImplementedException();
        }

    }
}
