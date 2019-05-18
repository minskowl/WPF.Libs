using System;
using Savchin.ComponentModel;
using Savchin.Logging;
using Savchin.Wpf.Interfaces;

namespace Savchin.Wpf.Input
{
    public class DoAfterCommandAspect : CommandAspectBase
    {
        private readonly Action _action;

        public DoAfterCommandAspect(ILogger logger, Action action)
            : base(logger)
        {
            _action = action;
        }

        public override void After(object parameter)
        {
            base.After(parameter);

            _action?.Invoke();
        }
    }

    public static class DoAfterCommandAspectExtension
    {
        private static ILogger _logger;

        private static ILogger Logger => _logger ?? (_logger = ServiceLocator.GetInstance<ILogger>());

        public static TCommand DoAfter<TCommand>(this TCommand command, Action action)
            where TCommand : ICommandBase
        {
            command.With(new DoAfterCommandAspect(Logger, action));
            return command;
        }
    }
}