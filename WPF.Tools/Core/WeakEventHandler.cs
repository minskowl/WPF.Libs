﻿using System;
using System.Reflection;

namespace Savchin.Wpf.Core
{
    /// <summary>
    /// WeakEventHandler
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class WeakEventHandler
    {
        readonly MethodInfo _method;
        readonly WeakReference _reference;


        internal object Target => _reference.Target;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventHandler"/> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public WeakEventHandler(EventHandler handler)
        {
            _reference = new WeakReference(handler.Target);
            _method = handler.Method;
        }

        public void Invoke(object sender)
        {
            var target = _reference.Target;
            if (target != null)
                _method.Invoke(target, new[] { sender, EventArgs.Empty });
        }
    }
}