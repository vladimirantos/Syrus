using Syrus.Core.Metadata;
using Syrus.Plugin;
using System.Collections.Generic;
using System.IO;

namespace Syrus.Core
{
    public class PluginLoader : ILoader
    {
        public const string MetadataFileName = "plugin.json";
        private MetadataParser _metadataParser;

        public PluginLoader()
        {
            _metadataParser = new MetadataParser();
        }

        public IEnumerable<IPlugin> Load(string pluginsLocation)
        {
            string[] directories = Directory.GetDirectories(pluginsLocation);
            for(int i = 0; i < directories.Length; i++)
            {
                string pluginPath = Path.Combine(pluginsLocation, directories[i]);
                PluginMetadata metadata = LoadMetadata(pluginPath);
                IPlugin plugin = LoadPlugin(Path.Combine(pluginPath, metadata.Name));
            }

            return new List<IPlugin>();
        }

        private PluginMetadata LoadMetadata(string pluginPath)
        {
            string pluginFullPath = Path.Combine(pluginPath, MetadataFileName);
            try
            {
                return _metadataParser.ParseFromFile(pluginFullPath);
            }
            catch (IOException e)
            {
                throw new SyrusException($"Failed to load metadata file {pluginFullPath}", e);
            }
            catch (MetadataParserException e) 
            {
                throw new SyrusException($"Failed to parse metadata file {pluginFullPath}", e);
            }
        }

        private IPlugin LoadPlugin(string libraryPath)
        {
            return null;
        }
    }
}
