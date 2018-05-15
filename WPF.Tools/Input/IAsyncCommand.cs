using System.ComponentModel;
using System.Threading.Tasks;
using Savchin.Wpf.Interfaces;

namespace Savchin.Wpf.Input
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAsyncCommandBase : ICommandBase, INotifyPropertyChanged
    {
        
        /// <summary>
        /// Gets a value indicating whether [in process].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [in process]; otherwise, <c>false</c>.
        /// </value>
        bool InProcess { get; }
    }

    public interface IAsyncCommand : IAsyncCommandBase
    {

        /// <summary>
        /// Determines whether this instance can execute.
        /// </summary>
        /// <returns></returns>
        bool CanExecute();

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync();
   
        /// <summary>
        /// Executes this instance.
        /// </summary>
        void Execute();
    }


    public interface IAsyncCommand<in T> : IAsyncCommandBase
    {
        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        Task ExecuteAsync(T parameter);
        /// <summary>
        /// Determines whether this instance can execute the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        bool CanExecute(T parameter);
    }
}