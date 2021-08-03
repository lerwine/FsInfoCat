using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public static class DesignTimeLoggerFactory
    {
        private static readonly object _syncRoot = new();
        private static ILoggerFactory _factory;
        private static LinkedList<WeakReference<ILogger>> _loggers = new();

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
                _loggers.AddLast(new WeakReference<ILogger>(result));
                return result;
            }
        }
    }
}
