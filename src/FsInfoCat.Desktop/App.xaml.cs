using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
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
        public static readonly LoggerFactory LoggerFactory;
        static App()
        {
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddProvider(new DebugLoggerProvider());
        }

        public static ViewModel.MainViewModel GetMainVM() => ((App)Current).FindResource("MainVM") as ViewModel.MainViewModel;

        public static ViewModel.SettingsViewModel GetSettingsVM() => ((App)Current).FindResource("SettingsVM") as ViewModel.SettingsViewModel;
    }
}
