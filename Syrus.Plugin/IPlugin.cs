using System.Collections.Generic;

namespace Syrus.Plugin
{
    public interface IPlugin
    {
        void OnInitialize(PluginContext context);

        IEnumerable<Result> Search(Query query);
    }
}
