using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Syrus.Plugin;

namespace Syrus.Plugins.Applications
{
    public class Main : BasePlugin
    {
        private ApplicationSearcher _applicationSearcher;
        private ResourceDictionary ViewTemplate { get; set; }
        private string previous = string.Empty;
        public override void OnInitialize(PluginContext context)
        {
            InitTemplate(nameof(View));
            _applicationSearcher = new ApplicationSearcher(context);
            _applicationSearcher.Initialize(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            _applicationSearcher.Initialize(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
        }
        
        public override Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            int length = query.Original.Length;
            if (previous.Length > length || length < 2)
            {
                previous = query.Original;
                return Task.FromResult<IEnumerable<Result>>(new List<Result>());
            }
            previous = query.Original;
            string q = query.Original.ToLower();
            IEnumerable<AppInfo> selectedApps = _applicationSearcher.AppInfos.Retrieve(q).Take(10);
            List<Result> results = new List<Result>();
            foreach (AppInfo app in selectedApps)
            {
                results.Add(new Result()
                {
                    Text = app.Name,
                    Icon = app.Icon,
                    Content = new View()
                    {
                        Template = ViewTemplate,
                        ViewModel = app
                    }
                });
            }
            return Task.FromResult<IEnumerable<Result>>(results);
        }
    }
}
