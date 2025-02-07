﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMGame.Commands
{
    class RelayCommand<T> : ICommand
    {
        private Action<T> commandTask;
        private Predicate<T> canExecuteTask;

        public RelayCommand(Action<T> workToDo, Predicate<T> canExecute)
        {
            commandTask = workToDo;
            canExecuteTask = canExecute;
        }

        public RelayCommand(Action<T> workToDo)
            : this(workToDo, DefaultCanExecute)
        {
            commandTask = workToDo;
        }

        private static bool DefaultCanExecute(T parameter)
        {
            return true;
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteTask != null && canExecuteTask((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                //+=asociaza un handler la un eveniment
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                //-=sterge un handler de la un eveniment
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            commandTask((T)parameter);
        }
    }
}
