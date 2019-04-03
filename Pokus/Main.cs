using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Pokus
{
    public class Main : IPlugin
    {
        public void OnInitialize(PluginContext context)
        {
        }

        public IEnumerable<Result> Search(Query query)
        {
            return new List<Result>()
            {
                new Result()
            {
                Text = "BANIK PICO",
                Group = "Pičoviny",
            }
        };
            
        }
    }
}
