using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.UnitTests.Fakes
{
    public sealed class AppFake : Application
    {
        private object _syncRoot = new();
        private readonly ILogger<AppFake> _logger;

        public Exception UnhandledException { get; private set; }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            lock (_syncRoot)
            {
                if (UnhandledException is null)
                    UnhandledException = e.Exception;
                else if (UnhandledException is AggregateException aggregateException)
                    UnhandledException = new AggregateException(aggregateException.InnerExceptions.Concat(new Exception[] { e.Exception }));
                else
                    UnhandledException = new AggregateException(UnhandledException, e.Exception);
            }
        }

        private AppFake(TaskCompletionSource<Exception> taskCompletionSource)
        {
            _logger = Hosting.ServiceProvider.GetRequiredService<ILogger<AppFake>>();
            _logger.LogDebug("Invoked {Type} constructor", typeof(AppFake));
            Startup += (s, e) => taskCompletionSource.SetResult(null);
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            //Uri resourceLocater = new("/FsInfoCat.Desktop;V1.0.0.0;component/app.xaml", UriKind.Relative);
            //LoadComponent(this, resourceLocater);
        }

        internal static void AssemblyInit(TestContext context)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(AppFake).FullName}.{nameof(AssemblyInit)}");
            var waitForApplicationRun = new TaskCompletionSource<Exception>();
            Task.Run(async () =>
            {
                System.Diagnostics.Debug.WriteLine($"{typeof(AppFake).FullName}.{nameof(AssemblyInit)} starting service initialization");
                try
                {
                    await Hosting.Initialize(Array.Empty<string>(), typeof(Hosting).Assembly, typeof(Local.LocalDbContext).Assembly, typeof(Desktop.ViewModel.MainVM).Assembly, typeof(AppFake).Assembly);
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine($"Service initialization failed: {exception}");
                    waitForApplicationRun.SetResult(exception);
                }
                AppFake app = new(waitForApplicationRun);
                app._logger.LogDebug("Running {Type}", typeof(AppFake));
                //app.Startup += (s, e) => waitForApplicationRun.SetResult(true);
                app.Run();
                app._logger.LogDebug("{Type} exited", typeof(AppFake));
            });
            System.Diagnostics.Debug.WriteLine($"{typeof(AppFake).FullName}.{nameof(AssemblyInit)} waiting for initialization");
            waitForApplicationRun.Task.Wait();
            if (waitForApplicationRun.Task.Result is not null)
                throw new AssertInconclusiveException("Initialization failed.", waitForApplicationRun.Task.Result);
            System.Diagnostics.Debug.WriteLine($"{typeof(AppFake).FullName}.{nameof(AssemblyInit)} completed");
        }

        internal static void AssemblyCleanup() => Current?.Dispatcher?.Invoke(Current.Shutdown);
    }
}
