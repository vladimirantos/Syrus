using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;
using Syrus.Plugin;

namespace Syrus.Plugins.Applications
{
    public class Main : IPlugin
    {
        private ApplicationSearcher _applicationSearcher;

        public void OnInitialize(PluginContext context)
        {
            _applicationSearcher = new ApplicationSearcher(context);
            _applicationSearcher.Initialize(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            _applicationSearcher.Initialize(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
        }

        public Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            string q = query.Original.ToLower();
            IEnumerable<AppInfo> selectedApps = _applicationSearcher.AppInfos
                .Where(app => app.Name.ToLower().StartsWith(q))
                .ToList();
            List<Result> results = new List<Result>();
            foreach(AppInfo app in selectedApps)
            {
                results.Add(new Result()
                {
                    Text = app.Name,
                    Icon = app.Icon,
                });
            }
            return Task.FromResult<IEnumerable<Result>>(results);
        }
    }
}
