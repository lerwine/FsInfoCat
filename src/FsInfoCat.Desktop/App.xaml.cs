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
            return Services.ServiceProvider.GetRequiredService<ILogger<T>>();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await Services.Initialize(e.Args);
            if (Dispatcher.CheckAccess())
                ShowMainWindow();
            else
                Dispatcher.Invoke(ShowMainWindow);
        }

        internal ViewModel.MainVM GetMainViewModel() => (ViewModel.MainVM)Services.ServiceProvider.GetRequiredService<IApplicationNavigation>();

        private void ShowMainWindow()
        {
            _logger = Services.ServiceProvider.GetRequiredService<ILogger<App>>();
            MainWindow mainWindow = Services.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        [ServiceBuilderHandler]
#pragma warning disable IDE0051 // Remove unused private members
        private static void ConfigureServices(IServiceCollection services)
#pragma warning restore IDE0051 // Remove unused private members
        {
            FsInfoCat.Local.LocalDbContext.AddDbContextPool(services
                    .AddSingleton<MainWindow>()
                    .AddSingleton<IApplicationNavigation, ViewModel.MainVM>(),
                typeof(App).Assembly, Desktop.Properties.Settings.Default.LocalDbfileName);
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (Services.Host)
                await Services.Host.StopAsync(TimeSpan.FromSeconds(5));
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
