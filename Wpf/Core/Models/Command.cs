using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wpf.Core.Models
{
    public class Command : ICommand
    {

        Action<object?> _Execute;
        Func<object?, bool>? _CanExecute = null;
      
        public Command(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            this._Execute = execute;
            this._CanExecute = canExecute;
        }
        public Command(Action execute,Func<bool> canExecute)
        {
            this._Execute = (discart) => execute();
            this._CanExecute = (q) => canExecute();
        }
        public Command(Action execute)
        {
            this._Execute = (discart) => execute();
            this._CanExecute = (q) => true;
        }
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            return _CanExecute == null || _CanExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            this._Execute(parameter);
        }
        
    }
}
