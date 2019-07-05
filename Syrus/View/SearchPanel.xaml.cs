using MVVMLight.Messaging;
using Syrus.ViewModel;
using System.Windows.Input;

namespace Syrus.View
{
    public partial class SearchPanel
    {
        public SearchPanel()
        {
            InitializeComponent();
        }

        private void SearchingInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Up && e.Key != Key.Down)
                return;
            Messenger.Default.Send<Key>(e.Key, "selectResult");
            e.Handled = true;
            SearchingInput.Focus();
        }
    }
}
