using FsInfoCat.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace FsInfoCat.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ILogger<App> _logger;

        public static string GetAppDataPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Desktop.Properties.Settings.Default.ApplicationDataFolderName);

        public static string GetLocalDbPath() => Path.Combine(GetAppDataPath(), Desktop.Properties.Settings.Default.LocalDbFile);

        public static DirectoryInfo EnsureAppDataPath()
        {
            DirectoryInfo result = new DirectoryInfo(GetAppDataPath());
            if (!result.Exists)
                result.Create();
            return result;
        }

        public App() { _logger = Services.GetLoggingService().CreateLogger<App>(); }

        protected override void OnStartup(StartupEventArgs e)
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            using (var scope = _logger.BeginScope("OnStartup"))
            {
                LocalDb.LocalDbContext.Initialize(Desktop.Properties.Settings.Default.LocalDbFile, assembly);
                if (AppConfig.TryGetConnectionStringBuilder(nameof(UpstreamDb.UpstreamDbContext), out DbConnectionStringBuilder connectionStringBuilder, out string providerName))
                {
                    if (connectionStringBuilder is SqlConnectionStringBuilder)
                        UpstreamDb.UpstreamDbContext.Initialize(connectionStringBuilder.ConnectionString, assembly.GetName());
                    else
                    {
                        _logger.LogWarning($"Unsported provider type \"{{{nameof(ConnectionStringSettings.ProviderName)}}}\" specified in {nameof(ConnectionStringSettings)} \"{{{nameof(ConnectionStringSettings.Name)}}}\".",
                            providerName, nameof(UpstreamDb.UpstreamDbContext));
                        UpstreamDb.UpstreamDbContext.Initialize(null, assembly.GetName());
                    }
                }
                else
                    UpstreamDb.UpstreamDbContext.Initialize(null, assembly.GetName());
                base.OnStartup(e);
            }
        }

        public static ViewModel.MainViewModel GetMainVM() => ((App)Current).FindResource("MainVM") as ViewModel.MainViewModel;

        public static ViewModel.SettingsViewModel GetSettingsVM() => ((App)Current).FindResource("SettingsVM") as ViewModel.SettingsViewModel;

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
        }
    }
}
