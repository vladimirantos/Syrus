using Syrus.Core.Metadata;
using Syrus.Plugin;
using Syrus.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Syrus.Core
{
    public class PluginLoader : ILoader
    {
        public const string MetadataFileName = "plugin.json";
        private MetadataParser _metadataParser;
        private string _pluginLocation;

        public PluginLoader(string pluginLocation)
        {
            _metadataParser = new MetadataParser();
            _pluginLocation = pluginLocation;
        }

        public IEnumerable<PluginPair> Load()
        {
            List<PluginPair> plugins = new List<PluginPair>();
            string[] directories = Directory.GetDirectories(_pluginLocation);
            for (int i = 0; i < directories.Length; i++)
            {
                string pluginPath = Path.Combine(_pluginLocation, directories[i]);
                PluginMetadata metadata = LoadMetadata(pluginPath);
                metadata.Icon = metadata.Icon != null ? Path.Combine(pluginPath, metadata.Icon) : null;
                metadata.NightIcon = metadata.NightIcon != null ? Path.Combine(pluginPath, metadata.NightIcon) : null;
                IPlugin plugin;
                try
                {
                    plugin = CreatePluginInstance($"{Path.Combine(pluginPath, metadata.FullName)}.dll");
                }
                catch(IOException e)
                {
                    throw new SyrusException($"Failed to load assembly {metadata.FullName} from {pluginPath}", e);
                }
                yield return new PluginPair(plugin, metadata);
            }
        }

        /// <summary>
        /// Get metadata from json file
        /// </summary>
        /// <param name="pluginPath">Absolute path to json file</param>
        /// <exception cref="SyrusException">When file not exists or valid</exception>
        private PluginMetadata LoadMetadata(string pluginPath)
        {
            string pluginFullPath = Path.Combine(pluginPath, MetadataFileName);
            try
            {
                PluginMetadata metadata = _metadataParser.ParseFromFile(pluginFullPath);
                metadata.PluginLocation = pluginPath;
                return metadata;
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
        /// Create instance of IPlugin from assembly
        /// </summary>
        /// <param name="assemblyPath">Absolute path to assembly</param>
        /// <exception cref="FileNotFoundException">When file not exists</exception>
        /// <exception cref="SyrusException">When don't exists class in assembly that doesn't implement IPlugin</exception>
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
