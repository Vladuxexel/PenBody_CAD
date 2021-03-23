using System;
using System.Windows.Input;

namespace PenBody_CadUI
{
    /// <summary>
    /// Реализация класса Relay Command.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Делегат выполнения команды.
        /// </summary>
        private Action<object> execute;

        /// <summary>
        /// Делегат возможности выполнения команды.
        /// </summary>
        private Func<object, bool> canExecute;

        /// <summary>
        /// Событие, выполняемое командой.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Конструктор класса Relay Command.
        /// </summary>
        /// <param name="execute">Действие</param>
        /// <param name="canExecute">Возможность выполнения</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Метод определения возможности выполнения команды.
        /// </summary>
        /// <param name="parameter">Параметр команды</param>
        /// <returns>Может ли выполниться команда</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        /// <summary>
        /// Метод выполнения команды.
        /// </summary>
        /// <param name="parameter">Параметр команды</param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
