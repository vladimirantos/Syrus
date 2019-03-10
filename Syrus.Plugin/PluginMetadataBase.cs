using System.Collections.Generic;
using System.Linq;

namespace Syrus.Plugin
{
    public class PluginMetadataBase
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
        /// If is <c>true</c>, it will be use for searching allways.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Results from default plugins are sorted by priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Local path to icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// If its <c>false</c>, help placeholder will be hidden.
        /// </summary>
        public bool EnableHelp { get; set; } = true;

        public IEnumerable<SearchingConfiguration> SearchingConfigurations { get; set; }

        /// <summary>
        /// Constants, which are configurable
        /// </summary>
        public Dictionary<string, object> Constants { get; set; }

        /// <summary>
        /// Readonly constants, It's not configurable.
        /// </summary>
        public Dictionary<string, object> ReadonlyConstants { get; set; }
    }

    public class SearchingConfiguration
    {
        public string Language { get; set; }
        public IEnumerable<string> Keywords { get; set; }
        public IEnumerable<string> RegularExpressions { get; set; }
    }
}
