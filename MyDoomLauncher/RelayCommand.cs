using System;
using System.Windows.Input;

namespace MyDoomLauncher
{
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action targetAction)
        {
            _targetAction = new Action(targetAction);
        }

        public RelayCommand(Action targetAction, Func<bool> canExecuteAction)
        {
            _targetAction = targetAction;
            _canExecuteAction = canExecuteAction;
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter)
        {
            if (_targetAction == null)
                return false;

            if (_canExecuteAction != null)
                return _canExecuteAction();
            else
                return true;
        }

        public void Execute(object parameter)
        {
            _targetAction?.Invoke();
        }

        public event EventHandler CanExecuteChanged;

        private readonly Func<bool> _canExecuteAction;
        private readonly Action _targetAction;
    }
}