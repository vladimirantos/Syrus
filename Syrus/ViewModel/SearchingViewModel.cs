namespace Syrus.ViewModel
{
    class SearchingViewModel : NotifyPropertyChanges
    {
        private string _query;
        public string SearchingQuery 
        {
            get => _query;
            set => SetProperty(ref _query, value, Search);
        }

        public void Search(string newValue)
        {

        }
    }
}
