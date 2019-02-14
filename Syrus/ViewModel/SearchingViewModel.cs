namespace Syrus.ViewModel
{
    class SearchingViewModel : BaseViewModel
    {
        private string _query;
        public string SearchingQuery 
        {
            get => _query;
            set 
            {
                _query = value;
                NotifyPropertyChanged();
            }
        }
    }
}
