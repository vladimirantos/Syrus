using System.Collections.Generic;

namespace Syrus.Plugin
{
    public class PluginMetadata
    {
        /// <summary>
        /// Plugin name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of DLL library
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Current version of this plugin
        /// </summary>
        public string Version { get; set; }
        
        public string Author { get; set; }

        /// <summary>
        /// Absolute path to plugin icon
        /// </summary>
        public string IconPath { get; set; }

        /// <summary>
        /// Command which have to use for searching
        /// </summary>
        public string Command { get; set; }

        public Dictionary<string, object> Constants { get; set; }
    }
}
