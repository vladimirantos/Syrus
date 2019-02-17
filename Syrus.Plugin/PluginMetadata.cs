using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// If is <c>true</c>, it will be use for searching allways.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Type of sentenses which can be used for serching.
        /// </summary>
        public IEnumerable<SearchingPattern> SearchingPatterns { get; set; }

        /// <summary>
        /// Get only regex patterns
        /// </summary>
        public IEnumerable<SearchingPattern> RegexPatterns => SearchingPatterns.Where(p => p.IsRegex);

        /// <summary>
        /// Get only text patterns
        /// </summary>
        public IEnumerable<SearchingPattern> TextPatterns => SearchingPatterns.Where(p => !p.IsRegex);

        /// <summary>
        /// Configuration
        /// </summary>
        public Dictionary<string, object> Constants { get; set; }
    }

    public class SearchingPattern
    {
        public string Text { get; set; }
        public bool IsRegex { get; set; }
        public int SimilarityPercentage { get; set; }
    }
}
