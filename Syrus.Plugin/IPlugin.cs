namespace Syrus.Plugin
{
    public interface IPlugin
    {
        PluginMetadata Metadata { get; internal set; }

        void OnInitialize();
        void Search(string searchTerm);
    }
}
