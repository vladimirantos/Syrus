using System;
using System.Collections.Generic;
using Syrus.Plugin;

namespace Syrus.Core.PluginFinding
{
    internal class ByCommandFinder : IPluginFinder
    {
        /// <summary>
        /// Klíčem je command
        /// </summary>
        private Dictionary<string, PluginPair> _commandPlugins = new Dictionary<string, PluginPair>();

        /// <summary>
        /// Pro optimalizaci, pokud je vyhledávaný řetězec delší, vyhledávání se ukončí.
        /// </summary>
        private int _maxCommandLength = 0;

        public void Initialize(ICollection<PluginPair> pluginPairs)
        {
            foreach (PluginPair p in pluginPairs)
            {
                if (p.Metadata.Command == null) //nepovinný parametr
                    continue;
                if (_maxCommandLength < p.Metadata.Command.Length) //nastavení nejdelšího commandu
                    _maxCommandLength = p.Metadata.Command.Length;

                _commandPlugins.Add(p.Metadata.Command.ToLower(), p);
            }
        }

        public IEnumerable<PluginPair> Find(string match, out SearchingQuery query)
        {
            throw new NotImplementedException();
        }

    }
}
