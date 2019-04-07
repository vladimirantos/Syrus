using System.Collections.Generic;
using System.Windows;

namespace Syrus.Plugin
{
    public interface IPlugin
    {
        void OnInitialize(PluginContext context);

        IEnumerable<Result> Search(Query query);
    }

    public interface IViewablePlugin : IPlugin
    {
        object ViewModel { get; set; }
        ResourceDictionary View { get; set; }
    }
}
