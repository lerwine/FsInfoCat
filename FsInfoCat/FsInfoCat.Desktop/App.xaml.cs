using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace FsInfoCat.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await Services.Initialize(ConfigureServices, e.Args);
            var mainWindow = Services.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ViewModel.MainVM>();
            Local.LocalDbContext.ConfigureServices(services, GetType().Assembly, Desktop.Properties.Settings.Default.LocalDbfileName);
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (Services.Host)
                await Services.Host.StopAsync(TimeSpan.FromSeconds(5));
        }
    }
}
