﻿using Syrus.Plugin;

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
            throw new System.NotImplementedException();
        }
    }
}
