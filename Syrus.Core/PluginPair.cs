using Syrus.Plugin;
using System.IO;

namespace Syrus.Core
{
    public class PluginPair
    {
        public IPlugin Plugin { get; internal set; }
        public PluginMetadata Metadata { get; internal set; }

        public PluginPair(IPlugin plugin, PluginMetadata metadata)
            => (Plugin, Metadata) = (plugin, metadata);

        public static explicit operator Result(PluginPair p)
        {
            return new Result()
            {
                Text = p.Metadata.Name,
                Group = "Možnosti vyhledávání",
                Icon = p.Metadata.Icon != null ? Path.Combine(p.Metadata.PluginLocation, p.Metadata.Icon) : "",
                FromPlugin = p.Metadata,
                OnClick = (IAppApi api, Result currentResult) => api.ChangeQuery(currentResult.FromPlugin.FromKeyword + " ")
            };
        }
    }
}
