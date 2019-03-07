using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Syrus.ViewModel
{
    public delegate void PluginSelected();
    class SearchingViewModel : NotifyPropertyChanges
    {
        private string _query;
        private string _placeholder;
        private Core.Syrus _syrus;
        public event PluginSelected OnSelectPlugin;

        public string SearchingQuery 
        {
            get => _query;
            set => SetProperty(ref _query, value, Search);
        }

        public string Placeholder 
        {
            get => _placeholder;
            set => SetProperty(ref _placeholder, value);
        }

        private ObservableCollection<Result> _results;
        public ObservableCollection<Result> Results 
        {
            get => _results;
            set => SetProperty(ref _results, value);
        }

        public ICommand CompleteTextByTabCommand => new Command(CompleteText, _ => Results.Count > 0 && CanDisplayHelp(Results.First()));

        public SearchingViewModel()
        {
            string pluginsLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syrus", "plugins");
            string cacheLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syrus", "cache");
            _syrus = new Core.Syrus(pluginsLocation, cacheLocation, new Core.Configuration() {
                Language = "cs"
            });
            _syrus.LoadPlugins().Initialize();
            Results = new ObservableCollection<Result>();
        }

        public async void Search(string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                Results = new ObservableCollection<Result>();
                Placeholder = string.Empty;
                return;
            }
            IEnumerable<Result> results = await _syrus.SearchAsync(newValue);
            Results = new ObservableCollection<Result>(results);
            Placeholder = Results.Count > 0 && CanDisplayHelp(Results[0]) ? CreateHelp(newValue, Results[0]) : string.Empty;
        }

        private void CompleteText(object obj)
        {
            SearchingQuery = Results.First().Text.ToLower() + " ";
            Placeholder = null;
            OnSelectPlugin.Invoke();
        }

        /// <summary>
        /// Kontroluje, jestli je možné zobrazit nápovědu (placeholder) a zároveň doplnit command pomocí TAB.
        /// Řeší případy command, command_, command_argument
        /// </summary>
        private bool CanDisplayHelp(Result result) => result.FromQuery.HasCommand 
            && !result.FromQuery.HasArguments 
            && !result.FromQuery.Original.EndsWith(' ');

        /// <summary>
        /// Ze zadaného výsledku hledání vybere zbývající část textu a zobrazí ji v Placeholderu.
        /// </summary>
        /// <param name="text">Text napsaný do vyhledávacího pole</param>
        /// <param name="result">Aktuálně nalezený výsledek hledání</param>
        private string CreateHelp(string text, Result result)
            => Regex.Replace(result.FromPlugin.FromKeyword, text, string.Empty, RegexOptions.IgnoreCase).ToLower();
    }

}
