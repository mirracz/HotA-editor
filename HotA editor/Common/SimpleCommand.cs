using System.Windows.Input;
using System;

namespace HotA_editor.Common;

public class SimpleCommand(Func<object, bool> canExecute, Action<object> execute) : ICommand
{
    public SimpleCommand(Action<object> execute) : this(null, execute) { }

    private readonly Func<object, bool> _canExecute = canExecute ?? new Func<object, bool>(param => true);
    private readonly Action<object> _execute = execute;

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute.Invoke(parameter);
    }

    public void Execute(object parameter)
    {
        _execute.Invoke(parameter);
    }
}
