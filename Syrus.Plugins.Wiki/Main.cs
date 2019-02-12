using Syrus.Plugin;
using System;

namespace Syrus.Plugins.Wiki
{
    public class Main : IPlugin
    {
        public void OnInitialize(PluginContext context)
        {
            System.Console.WriteLine("WIKI");
        }

        public void Search(string searchTerm)
        {
            Console.WriteLine("Wiki > search " + searchTerm);
        }
    }
}
