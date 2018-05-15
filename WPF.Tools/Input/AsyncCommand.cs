using System;
using System.Threading.Tasks;
using Savchin.Wpf.Core;

namespace Savchin.Wpf.Input
{
    public class AsyncCommand : AsyncCommandBase, IAsyncCommand
    {

        private readonly Func<bool> _canExecuteMethod;
        private readonly Func<Task> _executeMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AsyncCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod = null)
        {
            if (executeMethod == null)
                throw new ArgumentNullException(nameof(executeMethod));
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }





        public override void Execute(object parameter)
        {
            Execute();
        }

        public override bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            await Execute(_executeMethod, null, _executeMethod);
        }

        /// <summary>
        /// Determines whether this instance can execute.
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            return (_canExecuteMethod?.Invoke() ?? true) && !InProcess;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public async void Execute()
        {
            await ExecuteAsync();
        }

    }



    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ObjectBase" />
    /// <seealso cref="IAsyncCommand" />
    public class AsyncCommand<T> : AsyncCommandBase, IAsyncCommand<T>
    {
        private readonly Func<T, bool> _canExecuteMethod;
        private readonly Func<T, Task> _executeMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand{T}" /> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        public AsyncCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod = null)
        {
            _canExecuteMethod = canExecuteMethod;
            _executeMethod = executeMethod;
        }

        public override void Execute(object parameter)
        {
            ExecuteImpl(parameter);
        }


        public override bool CanExecute(object parameter)
        {
            return CanExecute(Convert(parameter));
        }


        public async Task ExecuteAsync(T parameter)
        {
            await Execute(_executeMethod, parameter, () => _executeMethod(parameter));
        }

        public bool CanExecute(T parameter)
        {
            return !InProcess && (_canExecuteMethod?.Invoke(parameter) ?? true);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected async void ExecuteImpl(object parameter)
        {
            await ExecuteAsync(Convert(parameter));
        }

        private static T Convert(object parameter)
        {
            return parameter == null ? default(T) : (T)parameter;
        }
    }
}