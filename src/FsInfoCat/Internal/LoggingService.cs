using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Internal
{
    internal class LoggingService : ILoggingService
    {
        private static readonly LoggerFactory _loggerFactory;

        static LoggingService()
        {
            _loggerFactory = new LoggerFactory();
            _loggerFactory.AddProvider(new DebugLoggerProvider());
        }

        public ILogger<T> CreateLogger<T>() => _loggerFactory.CreateLogger<T>();
    }
}
