using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Syrus.Plugin
{
    public interface IPlugin
    {
        void OnInitialize(PluginContext context);

        Task<IEnumerable<Result>> SearchAsync(Query query);
    }

    public interface ISchedulable
    {
        /// <summary>
        /// Je volána v pravidelných intervalech. Může být využita pro aktualizaci indexů.
        /// </summary>
        Task UpdateAsync();
    }

    public abstract class BasePlugin : IPlugin
    {
        protected ResourceDictionary ViewTemplate { get; set; }

        protected IEnumerable<Result> Empty => new List<Result>();

        public virtual void OnInitialize(PluginContext context)
        {
        }

        public virtual void OnClose()
        {
            
        }

        public virtual Task UpdateAsync() => Task.CompletedTask;

        public abstract Task<IEnumerable<Result>> SearchAsync(Query query);

        /// <summary>
        /// Nastaví resource dictionary do property ViewTemplate.
        /// </summary>
        /// <param name="view"></param>
        protected virtual void InitTemplate(string view)
        {
            string path = $"pack://application:,,,/{Assembly.GetCallingAssembly().GetName().Name};component/{view}.xaml";
            ViewTemplate = new ResourceDictionary()
            {
                Source = new Uri(path)
            };
        }
    }
}
