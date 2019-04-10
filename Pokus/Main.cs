using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Pokus
{
    public class Main : IPlugin
    {
        private object ViewModel { get; set; }
        private ResourceDictionary ViewTemplate { get; set; } = new ResourceDictionary();
        private Random _rand = new Random();
        public Main()
        {
            //ViewTemplate.Source = new Uri("C:/Users/vladi/AppData/Roaming/Syrus/plugins/Syrus.Plugins.Pokus/Pokus;component/View.xaml",
            //UriKind.RelativeOrAbsolute);
            ViewTemplate.Source = new Uri("pack://application:,,,/Pokus;component/View.xaml", UriKind.Absolute);
        }

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
                    Content = new View()
                    {
                        Template = ViewTemplate,
                        ViewModel = new PokusVm()
                        {
                            Title = "AHOJ " + _rand.Next(1, 100)
                        }
                    }
                }
            };
            
        }
    }

    public class PokusVm : BaseViewModel
    {
        public string Title { get; set; } =  "BANIK PICO ***";
    }
}
