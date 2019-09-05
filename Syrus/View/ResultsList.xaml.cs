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
            Messenger.Default.Register<Key>(this, "highlightResult", HighlightResult);
            Messenger.Default.Register<object>(this, "selectResult", SelectResult);
        }


        private void HighlightResult(Key key)
        {
            var list = ResultListBox.Visibility == Visibility.Visible ? ResultListBox : ResultListBoxFullscreen;
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

        private void SelectResult(object obj) => InvokeSelectedResultCommand(
            ResultListBox.Visibility == Visibility.Visible ?
            ResultListBox.SelectedItem as Result :
            ResultListBoxFullscreen.SelectedItem as Result);

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            => InvokeSelectedResultCommand(sender as ListBoxItem);

        private void InvokeSelectedResultCommand(ListBoxItem listBoxItem)
            => InvokeSelectedResultCommand(listBoxItem.Content as Result);


        private void InvokeSelectedResultCommand(Result result)
            => (DataContext as SearchingViewModel).SelectResultCommand.Execute(result);
    }
}
