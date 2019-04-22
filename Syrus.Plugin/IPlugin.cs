using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Syrus.Plugin
{
    public interface IPlugin
    {
        void OnInitialize(PluginContext context);

        Task<IEnumerable<Result>> SearchAsync(Query query);
    }
}
