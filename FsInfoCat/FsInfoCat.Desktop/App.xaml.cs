using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services.Initialize(ConfigureServices);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            Local.LocalDbContext.ConfigureServices(services, GetType().Assembly, Desktop.Properties.Settings.Default.LocalDbfileName);
            //services.AddSingleton<MainWindow>();
        }

        //private void Application_Startup(object sender, StartupEventArgs e)
        //{
        //    IHostBuilder host = Host.CreateDefaultBuilder(e.Args);
        //    host.ConfigureHostConfiguration(configHost =>
        //    {
        //        configHost.SetBasePath(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //        configHost.AddJsonFile("hostsettings.json", optional: true);
        //    });
        //    host.ConfigureServices(services =>
        //    {
        //        Local.LocalDbContext.ConfigureServices(services, GetType().Assembly, Desktop.Properties.Settings.Default.LocalDbfileName);
        //    });
        //    host.Build().RunAsync();
        //}
    }
}
