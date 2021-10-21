using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Windows;

namespace FsInfoCat.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ILogger<App> _logger;

        public static ILogger<T> GetLogger<T>(T dependencyObject) where T : DependencyObject
        {
            if (DesignerProperties.GetIsInDesignMode(dependencyObject))
                return DesignTimeLoggerFactory.GetLogger<T>();
            return Hosting.ServiceProvider.GetRequiredService<ILogger<T>>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(App).FullName}.{nameof(Application_Startup)}");
            Hosting.Initialize(e.Args, typeof(Hosting).Assembly, typeof(Local.LocalDbContext).Assembly, GetType().Assembly).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    System.Diagnostics.Debug.WriteLine("Service initialization canceled");
                    Shutdown(1);
                }
                else if (task.IsFaulted)
                {
                    System.Diagnostics.Debug.WriteLine($"Service initialization failed: {task.Exception}");
                    Exception exception = (task.Exception.InnerExceptions.Count == 1) ? task.Exception.InnerException : task.Exception;
                    MessageBox.Show($"Service initialization failed: {exception.Message.NullIfWhiteSpace() ?? exception.ToString()}", exception.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Stop);
                    Shutdown(2);
                }
                else
                    Dispatcher.Invoke(ShowMainWindow, System.Windows.Threading.DispatcherPriority.Send);
            });
        }

        internal static ViewModel.MainVM GetMainViewModel() => (ViewModel.MainVM)Hosting.ServiceProvider.GetRequiredService<IApplicationNavigation>();

        private void ShowMainWindow()
        {
            _logger = Hosting.ServiceProvider.GetRequiredService<ILogger<App>>();
            MainWindow mainWindow = Hosting.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        public App()
        {
            System.Diagnostics.Debug.WriteLine($"Constructing {typeof(App).FullName}");
        }

        [ServiceBuilderHandler]
#pragma warning disable IDE0051 // Remove unused private members
        private static void ConfigureServices(IServiceCollection services, HostBuilderContext builderContext)
#pragma warning restore IDE0051 // Remove unused private members
        {
            System.Diagnostics.Debug.WriteLine($"Invoked {typeof(App).FullName}.{nameof(ConfigureServices)}");
            Local.LocalDbContext.AddDbContextPool(services
                    .AddSingleton<MainWindow>()
                    .AddSingleton<IApplicationNavigation, ViewModel.MainVM>(),
                typeof(App).Assembly,
                builderContext.Configuration.GetSection(FsInfoCatOptions.FsInfoCat)?.GetSection(nameof(FsInfoCatOptions.LocalDbFile))?.Value.NullIfWhiteSpace() ?? Desktop.Properties.Settings.Default.LocalDbfileName);
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (Hosting.Host)
                await Hosting.Host.StopAsync(TimeSpan.FromSeconds(5));
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (_logger is null)
                System.Diagnostics.Debug.WriteLine($"An unhandled has occurred: {e.Exception.Message}\n{e.Exception}");
            else
                _logger.LogError(e.Exception, "An unhandled has occurred: {Message}", string.IsNullOrWhiteSpace(e.Exception.Message) ? e.Exception.ToString() : e.Exception.Message);
        }
    }
}
