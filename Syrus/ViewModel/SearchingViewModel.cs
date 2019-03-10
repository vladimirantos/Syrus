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
    class SearchingViewModel : NotifyPropertyChanges, IAppApi
    {
        private string _query;
        private string _placeholder;
        private string _quickResult;
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

        public string QuickResult 
        {
            get => _quickResult;
            set => SetProperty(ref _quickResult, value);
        }

        private ObservableCollection<Result> _results;
        public ObservableCollection<Result> Results 
        {
            get => _results;
            set => SetProperty(ref _results, value);
        }

        public ICommand CompleteTextByTabCommand => new Command((object obj) 
            => ChangeQuery(Results.First().FromPlugin.FromKeyword + " "), _ => Results.Count > 0 && CanDisplayHelp(Results.First()));

        public ICommand SelectResultCommand => new Command((object obj) => ((Result)obj).OnClick(this, obj as Result));

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
                QuickResult = string.Empty;
                return;
            }
            IEnumerable<Result> results = await _syrus.SearchAsync(newValue);
            Results = new ObservableCollection<Result>(results);
            if(Results[0].FromPlugin.EnableHelp)
                Placeholder = Results.Count > 0 && CanDisplayHelp(Results[0]) ? CreateHelp(newValue, Results[0]) : string.Empty;
            QuickResult = results.First().QuickResult;
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

        public void ChangeQuery(string query, bool append = false)
        {
            SearchingQuery = append ? SearchingQuery + query : query;
            Placeholder = null;
            OnSelectPlugin.Invoke();
        }

        public void ChangeQuickResult(string text)
        {
            throw new NotImplementedException();
        }

        public void SetHelpPlaceholder(string text)
        {
            throw new NotImplementedException();
        }
    }

}
