using Syrus.Plugin;
using System.Collections.Generic;
using System.IO;

namespace Syrus.Core
{
    public interface ILoader
    {
        /// <summary>
        /// Kontroluje, jestli složka s pluginem obsahuje soubor plugin.json
        /// </summary>
        bool IsValidPluginDirectory(string path);

        IEnumerable<IPlugin> Load(string pluginsLocation);
    }

    public class PluginLoader : ILoader
    {
        public const string MetadataFileName = "plugin.json";
        private MetadataParser _metadataParser;

        public PluginLoader()
        {
            _metadataParser = new MetadataParser();
        }

        public bool IsValidPluginDirectory(string path)
            => File.Exists(path);

        public IEnumerable<IPlugin> Load(string pluginsLocation)
        {
            string[] directories = Directory.GetDirectories(pluginsLocation);
            for(int i = 0; i < directories.Length; i++)
            {
                string pluginFullPath = Path.Combine(pluginsLocation, directories[i], MetadataFileName);
                if (IsValidPluginDirectory(pluginFullPath))
                    _metadataParser.ParseFromFile(pluginFullPath);
            }

            return new List<IPlugin>();
        }
    }
}
