using System.Windows.Input;
using Savchin.Wpf.Input;

namespace Savchin.Wpf.Interfaces
{
    public interface ICommandBase : ICommand//, IActiveAware
    {
        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        void RaiseCanExecuteChanged();

        /// <summary>
        /// Withes the specified aspect.
        /// </summary>
        /// <param name="aspect">The aspect.</param>
        void With(ICommandAspect aspect);
    }
}