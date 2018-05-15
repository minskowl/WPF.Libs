using System;
using Savchin.Logging;

namespace Savchin.Wpf.Input
{

    /// <summary>
    /// ICommandAspect
    /// </summary>
    public interface ICommandAspect
    {
        /// <summary>
        /// Befores this instance.
        /// </summary>
        void Before(object parameter);

        /// <summary>
        /// Afters this instance.
        /// </summary>
        void After(object parameter);

        /// <summary>
        /// Errors the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        void Error(object parameter);

        /// <summary>
        /// Withes the specified aspect.
        /// </summary>
        /// <param name="aspect">The aspect.</param>
        /// <returns></returns>
        ICommandAspect With(ICommandAspect aspect);

    }

    /// <summary>
    /// Command AspectBase class
    /// </summary>
    public abstract class CommandAspectBase : ICommandAspect
    {

        private ICommandAspect _next;

        protected ILogger Logger { get; }

        protected CommandAspectBase(ILogger logger)
        {
            Logger = logger;
        }

        public virtual void Before(object parameter)
        {
            _next.DoBefore(Logger, parameter);
        }

        public virtual void After(object parameter)
        {
            _next.DoAfter(Logger, parameter);
        }

        /// <summary>
        /// Errors this instance.
        /// </summary>
        public virtual void Error(object parameter)
        {
            _next.DoError(Logger, parameter);
        }

        /// <summary>
        /// Withes the specified aspect.
        /// </summary>
        /// <param name="aspect">The aspect.</param>
        public ICommandAspect With(ICommandAspect aspect)
        {
            if (_next == null)
                _next = aspect;
            else
                _next.With(aspect);
            return this;
        }

    }

    public static class CommandAspectHelper
    {
        public static void DoAfter(this ICommandAspect aspect, ILogger logger, object parameter)
        {
            try
            {
                aspect?.After(parameter);
            }
            catch (Exception ex)
            {
                logger?.Warning(ex);
            }
        }
        public static void DoBefore(this ICommandAspect aspect, ILogger logger, object parameter)
        {
            try
            {
                aspect?.Before(parameter);
            }
            catch (Exception ex)
            {
                logger?.Warning(ex);
            }
        }
        public static void DoError(this ICommandAspect aspect, ILogger logger, object parameter)
        {
            try
            {
                aspect?.Error(parameter);
            }
            catch (Exception ex)
            {
                logger?.Warning(ex);
            }
        }
    }
}
