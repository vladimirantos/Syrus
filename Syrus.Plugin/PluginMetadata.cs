﻿using System.Collections.Generic;
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
        /// Name of icon
        /// </summary>
        public string Icon { get; set; }

        public string PluginLocation { get; set; }

        /// <summary>
        /// Command which have to use for searching
        /// </summary>
        public string Command { get; set; }

        public IEnumerable<string> Keywords { get; set; }

        /// <summary>
        /// If is <c>true</c>, it will be use for searching allways.
        /// </summary>
        public bool Default { get; set; }

        public int Priority { get; set; }

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
        public IEnumerable<SearchingPattern> KeywordPattern => SearchingPatterns.Where(p => p.KeywordPattern);

        /// <summary>
        /// Configuration
        /// </summary>
        public Dictionary<string, object> Constants { get; set; }
    }

    public class SearchingPattern
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public bool IsRegex { get; set; }
        public bool KeywordPattern { get; set; }
    }

    public class SearchingQuery : SearchingPattern
    {

    }
}
