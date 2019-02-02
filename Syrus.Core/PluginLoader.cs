using Syrus.Core.Metadata;
using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
            List<IPlugin> plugins = new List<IPlugin>();
            string[] directories = Directory.GetDirectories(pluginsLocation);
            for(int i = 0; i < directories.Length; i++)
            {
                string pluginPath = Path.Combine(pluginsLocation, directories[i]);
                PluginMetadata metadata = LoadMetadata(pluginPath);
                try
                {
                    IPlugin plugin = CreatePluginInstance($"{Path.Combine(pluginPath, metadata.FullName)}.dll");
                    plugin.Metadata = metadata;
                    plugins.Add(plugin);
                }
                catch(IOException e)
                {
                    throw new SyrusException($"Failed to load assembly {metadata.FullName} from {pluginPath}", e);
                }
            }
            return plugins;
        }

        /// <summary>
        /// Vrací metadata z json souboru.
        /// </summary>
        /// <param name="pluginPath">Absolutní cesta k souboru s metadaty</param>
        /// <exception cref="SyrusException">Pokud neexistuje soubor s metadaty nebo není validní</exception>
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

        /// <summary>
        /// Vytvoří instanci IPlugin ze zadaného assembly.
        /// </summary>
        /// <param name="assemblyPath">Absolutní cesta k assembly</param>
        /// <exception cref="FileNotFoundException">Pokud cesta k assembly neexistuje</exception>
        /// <exception cref="SyrusException">Pokud neexistuje třída implementující IPlugin rozhraní</exception>
        private IPlugin CreatePluginInstance(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
                throw new FileNotFoundException($"Library {assemblyPath} not found");
            Assembly decoupledAssembly = Assembly.LoadFile(assemblyPath);
            Type pluginClass = decoupledAssembly.GetTypes().First(type => type.GetInterfaces().Contains(typeof(IPlugin)));
            if (pluginClass == null)
                throw new SyrusException($"Missing implementation of {nameof(IPlugin)} in {assemblyPath}");
            var plugin = ObjectActivator.CreateInstance<IPlugin>(pluginClass.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault());
            return plugin;
        }
    }
}
