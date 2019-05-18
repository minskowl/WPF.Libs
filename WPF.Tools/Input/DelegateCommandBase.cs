using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using Savchin.ComponentModel;
using Savchin.Logging;
using Savchin.Wpf.Core;
using Savchin.Wpf.Interfaces;

namespace Savchin.Wpf.Input
{
    public abstract class DelegateCommandBase : ICommandBase
    {
        public string Action { get; set; }

        private static ILogger _logger;
        private static ILogger Logger => _logger ?? (_logger = ServiceLocator.GetInstance<ILogger>());

        private bool _isActive;
        private List<WeakEventHandler> _canExecuteChangedHandlers;
        private EventHandler _isActiveChanged;
        private ICommandAspect _aspect;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive == value)
                    return;
                _isActive = value;
                OnIsActiveChanged();
            }
        }

        public virtual event EventHandler IsActiveChanged
        {
            add
            {
                EventHandler eventHandler = _isActiveChanged;
                EventHandler comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange(ref _isActiveChanged, comparand + value, comparand);
                }
                while (eventHandler != comparand);
            }
            remove
            {
                EventHandler eventHandler = _isActiveChanged;
                EventHandler comparand;
                do
                {
                    comparand = eventHandler;
                    eventHandler = Interlocked.CompareExchange(ref _isActiveChanged, comparand - value, comparand);
                }
                while (eventHandler != comparand);
            }
        }



        public event EventHandler CanExecuteChanged
        {
            add
            {
                WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
                CommandManager.RequerySuggested -= value;
            }
        }



        protected virtual void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
            WeakEventHandlerManager.CallWeakReferenceHandlers(this, _canExecuteChangedHandlers);
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        public void With(ICommandAspect aspect)
        {
            if (_aspect == null)
                _aspect = aspect;
            else
                _aspect.With(aspect);
        }

        protected virtual void OnIsActiveChanged()
        {
            var eventHandler = _isActiveChanged;
            eventHandler?.Invoke(this, EventArgs.Empty);
        }

        protected void Execute(Delegate method, object parameter, Action action)
        {
            try
            {
                Log(method, parameter);
                _aspect.DoBefore(Logger, parameter);
                action();
                _aspect.DoAfter(Logger, parameter);
            }
            catch (Exception exception)
            {
                _aspect.DoError(Logger, parameter);
                Logger.Warning(exception);
            }

        }

        public abstract void Execute(object parameter);
        public abstract bool CanExecute(object parameter);


        private void Log(Delegate executeMethod, object parameter)
        {
            Logger?.Info("Invoke command {0}:{1}{2}",
                executeMethod.Target,
                Action ?? executeMethod.Method.Name,
                parameter == null ? string.Empty : $": parameter '{parameter}'"
                );
        }

    }
}