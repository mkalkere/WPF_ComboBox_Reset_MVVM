using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SelectedItemToPreviousItemWPFMVVM
{
    public class CustomCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        private Action<object> execute;
        private Predicate<object> canExecute;

        public CustomCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute == null ? true : canExecute(parameter);
        //{
        //    return canExecute == null ? true : canExecute(parameter);
        //}

        public void Execute(object parameter) => execute(parameter);
        //{
        //    execute(parameter);
        //}
    }
}
