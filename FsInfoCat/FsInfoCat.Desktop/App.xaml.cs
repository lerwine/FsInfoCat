using Microsoft.Extensions.DependencyInjection;
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

    }
}
