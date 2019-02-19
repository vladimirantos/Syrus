using Newtonsoft.Json;

namespace Syrus.Plugin
{
    public class PluginContext
    {
        public PluginMetadata Metadata { get; private set; }
        public string PluginsLocation { get; set; }
        public string CacheLocation { get; set; }

        public PluginContext(PluginMetadata metadata) => Metadata = metadata;

        /// <summary>
        /// Deserialize JSON to type
        /// </summary>
        public T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}
