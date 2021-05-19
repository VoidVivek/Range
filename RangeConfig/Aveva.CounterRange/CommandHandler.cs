using System;
using System.Windows.Input;

namespace Aveva.CounterRange
{
    /// <summary>
    ///     Class BaseCommandHandler.
    /// </summary>
    public abstract class BaseCommandHandler
    {
        /// <summary>
        ///     The can execute predicate
        /// </summary>
        protected Predicate<object> canExecutePredicate;

        /// <summary>
        ///     CanExecuteChanged event handler
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        ///     Checks if command can be executed
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns><c>true</c> if this instance can execute the specified parameter; otherwise, <c>false</c>.</returns>
        public bool CanExecute(object parameter)
        {
            if (canExecutePredicate != null)
                return canExecutePredicate(parameter);
            return true;
        }
    }

    /// <summary>
    ///     Class CommandHandler.
    /// </summary>
    /// <seealso cref="Aveva.CounterRange.BaseCommandHandler" />
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class CommandHandler : BaseCommandHandler, ICommand
    {
        /// <summary>
        ///     The execute action
        /// </summary>
        private readonly Action executeAction;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="action">Command Action</param>
        /// <param name="canExecute">Predicate to determine if command can execute</param>
        public CommandHandler(Action action, Predicate<object> canExecute)
        {
            executeAction = action;
            canExecutePredicate = canExecute;
        }

        /// <summary>
        ///     Execution method
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can
        ///     be set to <see langword="null" />.
        /// </param>
        public void Execute(object parameter)
        {
            executeAction();
        }
    }

    /// <summary>
    ///     Class CommandHandler.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Aveva.CounterRange.BaseCommandHandler" />
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class CommandHandler<T> : BaseCommandHandler, ICommand
    {
        /// <summary>
        ///     The execute action
        /// </summary>
        private readonly Action<T> executeAction;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="action">Command Action</param>
        /// <param name="canExecute">Predicate to determine if command can execute</param>
        public CommandHandler(Action<T> action, Predicate<object> canExecute)
        {
            executeAction = action;
            canExecutePredicate = canExecute;
        }

        /// <summary>
        ///     Execution method
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can
        ///     be set to <see langword="null" />.
        /// </param>
        public void Execute(object parameter)
        {
            executeAction((T) parameter);
        }
    }
}