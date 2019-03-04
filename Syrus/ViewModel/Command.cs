using System;
using System.Windows.Input;

namespace Syrus.ViewModel
{
    public class Command : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _execute;

        public event EventHandler CanExecuteChanged;

        public Command(Action<object> execute, Predicate<object> canExecute)
            => (_execute, _canExecute) = (execute, canExecute);

        public bool CanExecute(object parameter) => _canExecute != null && _canExecute.Invoke(parameter);

        public void Execute(object parameter) => _execute.Invoke(parameter);
    }
}
