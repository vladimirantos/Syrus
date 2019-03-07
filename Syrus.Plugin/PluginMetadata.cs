using System;

namespace Syrus.Plugin
{
    public class PluginMetadata : PluginMetadataBase
    {
        /// <summary>
        /// Absolute path to plugin directory.
        /// </summary>
        public string PluginLocation { get; set; }

        /// <summary>
        /// Klíčové slovo podle kterého byl plugin identifikován.
        /// </summary>
        public string FromKeyword { get; set; }

        public SearchingConfiguration? CurrentSearchingConfiguration { get; set; }
    }
}
