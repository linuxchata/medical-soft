using System;
using System.Windows.Input;

namespace Common.Commands
{
    /// <summary>
    /// Command class which implement ICommand interface.
    /// </summary>
    public sealed class CommonCommand : ICommand
    {
        private readonly Action<object> action;

        private readonly Predicate<object> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonCommand"/> class.
        /// </summary>
        /// <param name="action">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public CommonCommand(Action<object> action, Predicate<object> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
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

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Returns is action can be executed.</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}
