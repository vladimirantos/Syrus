using System;
using System.Collections.Generic;
using System.Text;

namespace Syrus.Plugins.Applications
{
    public class Application
    {
        public string Name { get; set; }
        public string IconPath { get; set; }
        public string Description { get; set; }
        public string UninstallPath { get; set; }
        public string ExecutableName { get; set; }
        public string ParentDirectory { get; set; }
    }
}
