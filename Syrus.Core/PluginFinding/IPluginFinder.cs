
using Syrus.Plugin;
using System.Collections;
using System.Collections.Generic;

namespace Syrus.Core.PluginFinding
{
    /// <summary>
    /// Rozhraní pro identifikaci pluginu pro vyhledávání.
    /// </summary>
    internal interface IPluginFinder
    {
        /// <summary>
        /// Převede kolekci pluginů na kolekci určenou pro vyhledávání.
        /// </summary>
        void Initialize(ICollection<PluginPair> pluginPairs);

        /// <summary>
        /// Podle zadaných kritérií nalezne vhodné pluginy.
        /// </summary>
        /// <param name="match">Řetězec podle kterého se identifikuje plugin</param>
        /// <param name="query">Převede match na query, např. pro regex provede group matching.</param>
        /// <returns></returns>
        IEnumerable<PluginPair> Find(string match, out SearchingQuery query);
    }
}
