namespace Syrus.Plugin
{
    public class PluginContext
    {
        public PluginMetadata Metadata { get; private set; }

        public PluginContext(PluginMetadata metadata) => Metadata = metadata;
    }
}
