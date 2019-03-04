using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Syrus.ViewModel
{
    class SearchingViewModel : NotifyPropertyChanges
    {
        private string _query;
        private string _placeholder;
        private Core.Syrus _syrus;

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

        public ICommand CompleteTextByTabCommand => new Command(CompleteText, _ => !string.IsNullOrEmpty(SearchingQuery));

        public SearchingViewModel()
        {
            string pluginsLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syrus", "plugins");
            string cacheLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syrus", "cache");
            _syrus = new Core.Syrus(pluginsLocation, cacheLocation, new Core.Configuration() {
                Language = "en"
            });
            _syrus.LoadPlugins().Initialize();
            Results = new ObservableCollection<Result>();
        }

        public async void Search(string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                Results = new ObservableCollection<Result>();
                return;
            }
            IEnumerable<Result> results = await _syrus.SearchAsync(newValue);
            Results = new ObservableCollection<Result>(results);
            if (Results.Count == 1)
                CreateHelp(newValue, Results[0]);
            //Results = new CollectionViewSource()
            //{
            //    Source = new ObservableCollection<Result>(results),
            //    GroupDescriptions = { new PropertyGroupDescription(nameof(Result.Group)) }
            //};
        }

        private void CompleteText(object obj)
        {
            
        }


        private void CreateHelp(string text, Result result)
            => Placeholder = Regex.Replace(result.Text, text, string.Empty, RegexOptions.IgnoreCase);
    }
}
