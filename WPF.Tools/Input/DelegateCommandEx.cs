using System;
using Savchin.Wpf.Interfaces;

namespace Savchin.Wpf.Input
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class DelegateCommandEx : DelegateCommandBase, IDelegateCommand
    {
        private readonly Action _executeMethod;
        private readonly Func<bool> _canExecuteMethod;

        public DelegateCommandEx(Action executeMethod, Func<bool> canExecuteMethod = null)
        {
            if (executeMethod == null)
                throw new ArgumentNullException(nameof(executeMethod));

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        public void Execute()
        {
            Execute(null);
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public override void Execute(object parameter)
        {
            Execute(_executeMethod, parameter, _executeMethod);
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod();
        }


    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class DelegateCommandEx<T> : DelegateCommandBase, IDelegateCommand<T>
    {
        private readonly Action<T> _executeMethod;
        private readonly Func<T, bool> _canExecuteMethod;



        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommandEx{T}"/> class.
        /// </summary>
        /// <param name="executeMethod">The execute method.</param>
        /// <param name="canExecuteMethod">The can execute method.</param>
        /// <exception cref="System.ArgumentNullException">executeMethod</exception>
        public DelegateCommandEx(Action<T> executeMethod, Func<T, bool> canExecuteMethod = null)
        {
            if (executeMethod == null)
                throw new ArgumentNullException(nameof(executeMethod));

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }


        public override void Execute(object parameter)
        {
            Execute(Convert(parameter));
        }

        public override bool CanExecute(object parameter)
        {
            return CanExecute(Convert(parameter));
        }


        private static T Convert(object parameter)
        {
            return parameter == null ? default(T) : (T)parameter;
        }

        public void Execute(T parameter)
        {
            Execute(_executeMethod, parameter, () => _executeMethod(parameter));
        }

        public bool CanExecute(T parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod(parameter);
        }
    }
}
