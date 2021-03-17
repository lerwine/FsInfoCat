using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Copied from ALASTAIRTREE website web page on 3/17/2017:
    /// <list type="bullet">
    /// <item><term>Page Title</term> Logging with output in Unit Tests in .Net Core 2.0</item>
    /// <item><term>Author</term> unknown</item>
    /// <item><term>Published</term> February 21, 2018</item>
    /// <item><term>URL</term> https://alastaircrabtree.com/using-logging-in-unit-tests-in-net-core/</item>
    /// </list>
    /// </remarks>
    public static class TestLogger
    {
        public static ILogger<T> Create<T>()
        {
            var logger = new NUnitLogger<T>();
            return logger;
        }

        class NUnitLogger<T> : ILogger<T>, IDisposable
        {
            private readonly Action<string> output = Console.WriteLine;

            public void Dispose()
            {
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                Func<TState, Exception, string> formatter) => output(formatter(state, exception));

            public bool IsEnabled(LogLevel logLevel) => true;

            public IDisposable BeginScope<TState>(TState state) => this;
        }
    }
}
