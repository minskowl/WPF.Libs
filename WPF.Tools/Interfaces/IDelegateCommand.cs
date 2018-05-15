namespace Savchin.Wpf.Interfaces
{
    public interface IDelegateCommand : ICommandBase
    {
        /// <summary>
        /// Executes this instance.
        /// </summary>
        void Execute();
        /// <summary>
        /// Determines whether this instance can execute.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </returns>
        bool CanExecute();
    }

    public interface IDelegateCommand<in T> : ICommandBase
    {
        /// <summary>
        /// Executes the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        void Execute(T parameter);
        /// <summary>
        /// Determines whether this instance can execute the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        ///   <c>true</c> if this instance can execute the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        bool CanExecute(T parameter);
    }
}
