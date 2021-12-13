using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lote.Common
{
    public class Commands<T> : ICommand
    {
        private readonly Action<T> _Execute;
        private readonly Func<bool> _CanExecute;

        public Commands(Action<T> Execute, Func<bool> CanExecute)
        {
            _Execute = Execute;
            _CanExecute = CanExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_CanExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_CanExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }
        public bool CanExecute(object parameter)
        {
            return _CanExecute == null ? true : _CanExecute();
        }

        public void Execute(object parameter)
        {
            _Execute((T)parameter);
        }
    }
}
