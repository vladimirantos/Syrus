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
            if(e.Key == Key.Enter)
            {
                Messenger.Default.Send<object>(null, "selectResult");
                e.Handled = true;
                SearchingInput.Focus();
            }
            else if(e.Key == Key.Up || e.Key == Key.Down)
            {
                Messenger.Default.Send<Key>(e.Key, "highlightResult");
                e.Handled = true;
                SearchingInput.Focus();
            }
        }
    }
}
