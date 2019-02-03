namespace Syrus.Plugin
{
    public interface IPlugin
    {
        void OnInitialize(PluginContext context);

        void Search(string searchTerm);
    }
}
