using System;

namespace Syrus.Plugin
{
    public class PluginMetadata : PluginMetadataBase
    {
        /// <summary>
        /// Absolute path to plugin directory.
        /// </summary>
        public string PluginLocation { get; set; }

        public SearchingConfiguration? CurrentSearchingConfiguration { get; set; }
    }
}
