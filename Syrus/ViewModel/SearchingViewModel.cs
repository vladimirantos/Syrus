using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

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

        private IEnumerable<Result> _results;
        public IEnumerable<Result> Results 
        {
            get => _results;
            set => SetProperty(ref _results, value);
        }

        public SearchingViewModel()
        {
            _syrus = new Core.Syrus(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syrus"));
            _syrus.LoadPlugins().Initialize();
        }

        public void Search(string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                Results = new List<Result>();
                return;
            }
            Results = _syrus.Search(newValue);
        }
    }
}
