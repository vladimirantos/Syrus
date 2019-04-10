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
        private ResourceDictionary ViewTemplate { get; set; }
        private Random _rand = new Random();
        public Main()
        {
            //ViewTemplate.Source = new Uri("C:/Users/vladi/AppData/Roaming/Syrus/plugins/Syrus.Plugins.Pokus/Pokus;component/View.xaml",
            //UriKind.RelativeOrAbsolute);
        }

        public void OnInitialize(PluginContext context)
        {
            ViewTemplate = context.CreateView("pack://application:,,,/Pokus;component/View.xaml");
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
