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

        public ObservableCollection<Result> Results { get; set; }

        public SearchingViewModel()
        {
            _syrus = new Core.Syrus(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syrus"));
            _syrus.LoadPlugins().Initialize();
        }

        public void Search(string newValue)
        {
            List<Result> results = _syrus.Search(newValue).ToList();
            if(results.Count > 0)
                results.ForEach(Results.Add);
        }
    }
}
