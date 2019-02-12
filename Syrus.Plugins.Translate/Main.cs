using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syrus.Plugins.Translate
{
    class Main : IPlugin
    {
        public void OnInitialize(PluginContext context)
        {
            System.Console.WriteLine("TRANSLATE");
        }

        public void Search(string searchTerm)
        {
            Console.WriteLine("Translate > search " + searchTerm);
        }
    }
}
