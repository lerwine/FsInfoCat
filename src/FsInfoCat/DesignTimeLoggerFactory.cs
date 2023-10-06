using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    // TODO: Document DesignTimeLoggerFactory class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class DesignTimeLoggerFactory
    {
        private static readonly object _syncRoot = new();
        private static ILoggerFactory _factory;
        private static readonly LinkedList<WeakReference<ILogger>> _loggers = new();

        public static ILogger<T> GetLogger<T>()
        {
            lock (_syncRoot)
            {
                if (_factory is null)
                    _factory = LoggerFactory.Create(builder => builder.AddConsole().AddDebug());
                else
                {
                    LinkedListNode<WeakReference<ILogger>> node = _loggers.First;
                    while (node is not null)
                    {
                        if (node.Value.TryGetTarget(out ILogger logger))
                        {
                            if (logger is ILogger<T> t)
                                return t;
                        }
                        else
                        {
                            LinkedListNode<WeakReference<ILogger>> previous = node;
                            node = node.Next;
                            _loggers.Remove(previous);
                        }
                    }
                }
                ILogger<T> result = _factory.CreateLogger<T>();
                _ = _loggers.AddLast(new WeakReference<ILogger>(result));
                return result;
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
