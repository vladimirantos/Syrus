using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;

namespace Syrus.ViewModel
{
    class SearchingViewModel : NotifyPropertyChanges
    {
        private string _query;
        private Core.Syrus _syrus;

        public string SearchingQuery 
        {
            get => _query;
            set => SetProperty(ref _query, value, Search);
        }

        private ObservableCollection<Result> _results;
        public ObservableCollection<Result> Results 
        {
            get => _results;
            set => SetProperty(ref _results, value);
        }

        public SearchingViewModel()
        {
            _syrus = new Core.Syrus(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syrus"));
            _syrus.LoadPlugins().Initialize();
            Results = new ObservableCollection<Result>();
        }

        public void Search(string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                Results = new ObservableCollection<Result>();
                return;
            }
            IEnumerable<Result> results = _syrus.Search(newValue);
            Results = new ObservableCollection<Result>(results);
            //Results = new CollectionViewSource()
            //{
            //    Source = new ObservableCollection<Result>(results),
            //    GroupDescriptions = { new PropertyGroupDescription(nameof(Result.Group)) }
            //};
        }
    }
}
