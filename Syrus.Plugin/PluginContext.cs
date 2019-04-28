using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Syrus.Plugin
{
    public class PluginContext
    {
        public PluginMetadata Metadata { get; private set; }
        public string PluginsLocation { get; set; }
        public string CacheLocation { get; set; }
        public Cache Cache => new Cache(CacheLocation, Metadata.FullName);

        public PluginContext(PluginMetadata metadata) => Metadata = metadata;

        /// <summary>
        /// Deserialize JSON to type
        /// </summary>
        public T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);
        
        /// <summary>
        /// Parse json to JObject
        /// </summary>
        public JObject JObjectParse(string json) => JObject.Parse(json);

        /// <summary>
        /// Download json from url and parse to JObject.
        /// </summary>
        /// <param name="url">Url to json source</param>
        public JObject JObjectFromHttp(string url) => JObject.Parse(new WebClient().DownloadString(url));

        /// <summary>
        /// Donwload json from url async and parse to JObject
        /// </summary>
        public async Task<JObject> JObjectFromHttpAsync(string url)
        {
            using (WebClient client = new WebClient())
            {
                var result = await client.DownloadStringTaskAsync(url);
                return JObject.Parse(result);
            }
        }

        /// <summary>
        /// Get ResourceDictionary by path to view
        /// </summary>
        public ResourceDictionary CreateView(string path)
            => new ResourceDictionary()
            {
                Source = new Uri(path)
            };
    }
}
