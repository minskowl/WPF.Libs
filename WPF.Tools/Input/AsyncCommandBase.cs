using System;
using System.Threading.Tasks;
using Savchin.ComponentModel;
using Savchin.Logging;
using ObjectBase = Savchin.Wpf.Core.ObjectBase;

namespace Savchin.Wpf.Input
{


    public abstract class AsyncCommandBase : ObjectBase, IAsyncCommandBase
    {
        private ICommandAspect _aspect;

        internal static ILogger Logger { private get; set; }

        private bool _isActive;


        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive == value)
                    return;

                _isActive = value;
                IsActiveChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool _inProcess;


        /// <summary>
        /// Gets a value indicating whether in process
        /// </summary>
        /// <value>
        /// <c>true</c> if command in process; otherwise, <c>false</c>.
        /// </value>
        public bool InProcess
        {
            get { return _inProcess; }
            private set { Set(ref _inProcess, value, RaiseCanExecuteChanged); }
        }


        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);

        /// <summary>
        /// Occurs when [can execute changed].
        /// </summary>
        public virtual event EventHandler CanExecuteChanged;

        /// <summary>
        /// Occurs when [is active changed].
        /// </summary>
        public virtual event EventHandler IsActiveChanged;

        /// <summary>
        /// Initializes the <see cref="AsyncCommandBase" /> class.
        /// </summary>
        static AsyncCommandBase()
        {
            Logger = ServiceLocator.GetInstance<ILogger>();
        }

        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Withes the specified aspect.
        /// </summary>
        /// <param name="aspect">The aspect.</param>
        public void With(ICommandAspect aspect)
        {
            if (_aspect == null)
                _aspect = aspect;
            else
                _aspect.With(aspect);
        }

        protected async Task Execute(Delegate method, object parameter, Func<Task> command)
        {
            InProcess = true;

            _aspect.DoBefore(Logger, parameter);
            try
            {
                var parameterText = parameter == null ? string.Empty : ": parameter " + parameter;
                Logger?.Info("Invoke command {0}:{1}{2}", method.Target, method.Method.Name, parameterText);

                await command();

                _aspect.DoAfter(Logger, parameter);
            }
            catch (Exception ex)
            {
                Logger?.Warning(ex);

                _aspect.DoError(Logger, parameter);
            }
            finally
            {
                InProcess = false;
            }
        }


    }
}