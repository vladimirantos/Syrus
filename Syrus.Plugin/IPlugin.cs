using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syrus.Plugin
{
    public interface IPlugin
    {
        void OnInitialize(PluginContext context);

        //void OnClose();

        Task<IEnumerable<Result>> SearchAsync(Query query);
    }

    public abstract class BasePlugin : IPlugin
    {
        public virtual void OnInitialize(PluginContext context)
        {
        }

        public virtual void OnClose()
        {
            
        }

        public abstract Task<IEnumerable<Result>> SearchAsync(Query query);
    }
}
