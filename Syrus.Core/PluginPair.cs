using Syrus.Plugin;

namespace Syrus.Core
{
    public class PluginPair
    {
        public IPlugin Plugin { get; internal set; }
        public PluginMetadata Metadata { get; internal set; }

        public PluginPair(IPlugin plugin, PluginMetadata metadata)
            => (Plugin, Metadata) = (plugin, metadata);
    }
}
