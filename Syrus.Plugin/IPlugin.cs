namespace Syrus.Plugin
{
    public interface IPlugin
    {
        PluginMetadata Metadata { get; }

        void OnInitialize();
        void Search(string searchTerm);
    }
}
