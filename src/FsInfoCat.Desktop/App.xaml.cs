using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string AppResourceName_MainWindowViewModel = "MainWindowViewModel";
        public const string AppResourceName_AppSettingsViewModel = "AppSettingsViewModel";
        public const string AppResourceName_WorkerProgressViewModel = "WorkerProgressViewModel";

        public HttpClient HttpClient { get; } = new HttpClient();

        public static TWindow GetWindowByDataContext<TWindow, TDataContext>(TDataContext obj)
            where TWindow : Window
            where TDataContext : class
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (App.Current.CheckAccess())
                return App.Current.Windows.Cast<Window>().OfType<TWindow>().FirstOrDefault(w => w.DataContext != null && ReferenceEquals(w.DataContext, obj));

            return App.Current.Dispatcher.Invoke(() => GetWindowByDataContext<TWindow, TDataContext>(obj));
        }

        public static ViewModels.MainWindowViewModel MainWindowViewModel
        {
            get
            {
                if (App.Current.CheckAccess())
                    return App.Current.FindResource(AppResourceName_MainWindowViewModel) as ViewModels.MainWindowViewModel;
                return App.Current.Dispatcher.Invoke(() => App.Current.FindResource(AppResourceName_MainWindowViewModel) as ViewModels.MainWindowViewModel);
            }
        }

        public static ViewModels.AppSettingsViewModel AppSettingsViewModel
        {
            get
            {
                if (App.Current.CheckAccess())
                    return App.Current.FindResource(AppResourceName_AppSettingsViewModel) as ViewModels.AppSettingsViewModel;
                return App.Current.Dispatcher.Invoke(() => App.Current.FindResource(AppResourceName_AppSettingsViewModel) as ViewModels.AppSettingsViewModel);
            }
        }

        public static ViewModels.WorkerProgressViewModel WorkerProgressViewModel
        {
            get
            {
                if (App.Current.CheckAccess())
                    return App.Current.FindResource(AppResourceName_WorkerProgressViewModel) as ViewModels.WorkerProgressViewModel;
                return App.Current.Dispatcher.Invoke(() => App.Current.FindResource(AppResourceName_WorkerProgressViewModel) as ViewModels.WorkerProgressViewModel);
            }
        }
    }
}
