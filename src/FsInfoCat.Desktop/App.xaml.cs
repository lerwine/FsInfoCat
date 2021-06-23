using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace FsInfoCat.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ILogger<App> _logger;

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await Services.Initialize(e.Args);
            //await Services.Initialize_Obsolete(ConfigureServices, e.Args);
            if (Dispatcher.CheckAccess())
                ShowMainWindow();
            else
                Dispatcher.Invoke(ShowMainWindow);
        }

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
            Local.LocalDbContext.AddDbContextPool(services.AddSingleton<MainWindow>().AddSingleton<ViewModel.MainVM>(),
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
