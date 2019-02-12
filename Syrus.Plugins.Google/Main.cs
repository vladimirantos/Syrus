﻿using Syrus.Plugin;
using System;
using System.Collections.Generic;

namespace Syrus.Plugins.Google
{
    public class Main : IPlugin
    {
        public void OnInitialize(PluginContext context)
        {
            System.Console.WriteLine("GOOGLE");
        }

        public void Search(string searchTerm)
        {
            Console.WriteLine("Google > search " + searchTerm);
        }
    }
}
