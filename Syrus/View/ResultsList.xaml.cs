using MVVMLight.Messaging;
using Syrus.Plugin;
using Syrus.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Syrus.View
{
    public partial class ResultsList
    {
        public ResultsList()
        {
            InitializeComponent();
            Messenger.Default.Register<Key>(this, "selectResult", SelectResult);
        }

        private void SelectResult(Key key)
        {
            var list = ResultListBox;
            switch (key)
            {
                case Key.Down:
                    if (!list.Items.MoveCurrentToNext()) list.Items.MoveCurrentToLast();
                    break;

                case Key.Up:
                    if (!list.Items.MoveCurrentToPrevious()) list.Items.MoveCurrentToFirst();
                    break;
            }

            if (list.SelectedItem != null)
            {
                (Keyboard.FocusedElement as UIElement).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }



        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            => (DataContext as SearchingViewModel).SelectResultCommand.Execute((sender as ListBoxItem).Content as Result);

    }
}
