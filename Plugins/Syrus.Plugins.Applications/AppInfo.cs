using Syrus.Plugin;
using System.Collections.Generic;

namespace Syrus.Plugins.Applications
{
    internal class AppInfo : BaseViewModel
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string UninstallPath { get; set; }
        public string AppVersion { get; set; }
        public string Web { get; set; }
        public string InstallLocation { get; set; }
        public string Publisher { get; set; }

        public override string ToString() => Name;
    }

    internal class AppInfoComparer : IComparer<AppInfo>
    {
        public int Compare(AppInfo x, AppInfo y) => string.Compare(x.Name.ToLower(), y.Name.ToLower());
    }
}
