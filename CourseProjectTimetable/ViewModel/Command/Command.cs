using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CourseProjectTimetable.ViewModel.Commands
{
    public class Command : ICommand
    { 
        private readonly Predicate<object> _canExecute;
        private readonly Action _execute;

        private readonly Predicate<object> canExecute;
        private readonly Action<object> execute;

        public event EventHandler CanExecuteChanged;

        public Command(Action execute)
                   : this(execute, null)
        {
        }
        
        public Command(Action execute,
                       Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public Command(Action<object> execute)
                    : this(execute, null)
        {
        }

        public Command(Action<object> execute, Predicate<Object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }
        

        public void Execute(object parametr)
        {
            execute(parametr);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}