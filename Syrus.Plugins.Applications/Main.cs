using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Syrus.Plugin;

namespace Syrus.Plugins.Applications
{
    public class Main : IPlugin
    {
        private ApplicationSearcher _applicationSearcher;
        private ResourceDictionary ViewTemplate { get; set; }
        private string previous = string.Empty;
        public void OnInitialize(PluginContext context)
        {
            ViewTemplate = context.CreateView("pack://application:,,,/Syrus.Plugins.Applications;component/View.xaml");
            _applicationSearcher = new ApplicationSearcher(context);
            _applicationSearcher.Initialize(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            _applicationSearcher.Initialize(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
        }

        public Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            int length = query.Original.Length;
            if (previous.Length > length || length < 3)
            {
                previous = query.Original;
                return Task.FromResult<IEnumerable<Result>>(new List<Result>());
            }
            previous = query.Original;
            string q = query.Original.ToLower();
            List<AppInfo> selectedApps = _applicationSearcher.AppInfos.Retrieve(q);
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
