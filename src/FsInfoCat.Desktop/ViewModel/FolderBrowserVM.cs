using FsInfoCat.Desktop.WMI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FolderBrowserVM : DependencyObject
    {
        private readonly ILogger<FolderBrowserVM> _logger;

        public event DependencyPropertyChangedEventHandler CurrentPathPropertyChanged;

        public static readonly DependencyProperty CurrentPathProperty = DependencyProperty.Register(nameof(CurrentPath), typeof(string), typeof(FolderBrowserVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FolderBrowserVM).OnCurrentPathPropertyChanged(e)));

        public string CurrentPath
        {
            get => GetValue(CurrentPathProperty) as string;
            set => SetValue(CurrentPathProperty, value);
        }

        protected virtual void OnCurrentPathPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnCurrentPathPropertyChanged(args.OldValue as string, args.NewValue as string); }
            finally { CurrentPathPropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnCurrentPathPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnCurrentPathPropertyChanged Logic
        }

        #region LogicalDisks Property Members

        private static readonly DependencyPropertyKey InnerLogicalDisksPropertyKey = DependencyProperty.RegisterReadOnly(nameof(InnerLogicalDisks), typeof(ObservableCollection<FolderVM>), typeof(FolderBrowserVM), new PropertyMetadata(null));

        private static readonly DependencyPropertyKey LogicalDisksPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LogicalDisks), typeof(ReadOnlyObservableCollection<FolderVM>), typeof(FolderBrowserVM), new PropertyMetadata(null));

        protected static readonly DependencyProperty InnerLogicalDisksProperty = InnerLogicalDisksPropertyKey.DependencyProperty;

        public static readonly DependencyProperty LogicalDisksProperty = LogicalDisksPropertyKey.DependencyProperty;

        protected ObservableCollection<FolderVM> InnerLogicalDisks
        {
            get => (ObservableCollection<FolderVM>)GetValue(InnerLogicalDisksProperty);
            private set => SetValue(InnerLogicalDisksPropertyKey, value);
        }

        public ReadOnlyObservableCollection<FolderVM> LogicalDisks
        {
            get => (ReadOnlyObservableCollection<FolderVM>)GetValue(LogicalDisksProperty);
            private set => SetValue(LogicalDisksPropertyKey, value);
        }

        #endregion

        private static readonly DependencyPropertyKey ItemExpandedCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ItemExpandedCommand), typeof(Commands.RelayCommand), typeof(FolderBrowserVM), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((FolderBrowserVM)d).OnItemExpandedExecute)));

        public static readonly DependencyProperty ItemExpandedCommandProperty = ItemExpandedCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand ItemExpandedCommand => (Commands.RelayCommand)GetValue(ItemExpandedCommandProperty);

        private void OnItemExpandedExecute(object parameter)
        {
            // TODO: Implement OnItemExpandedExecute Logic
        }


        private static readonly DependencyPropertyKey ItemSelectedCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ItemSelectedCommand), typeof(Commands.RelayCommand), typeof(FolderBrowserVM), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((FolderBrowserVM)d).OnItemSelectedExecute)));

        public static readonly DependencyProperty ItemSelectedCommandProperty = ItemSelectedCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand ItemSelectedCommand => (Commands.RelayCommand)GetValue(ItemSelectedCommandProperty);

        private void OnItemSelectedExecute(object parameter)
        {
            // TODO: Implement OnItemSelectedExecute Logic
        }

        public FolderBrowserVM()
        {
            InnerLogicalDisks = new();
            LogicalDisks = new(InnerLogicalDisks);
            if (!DesignerProperties.GetIsInDesignMode(this))
                _logger = Services.ServiceProvider.GetRequiredService<ILogger<FolderBrowserVM>>();
            using CancellationTokenSource tokenSource = new();
            ExitEventHandler handler = new((sender, e) => tokenSource.Cancel(true));
            Application.Current.Exit += handler;
            _ = Win32_LogicalDisk.GetLogicalDisksAsync(tokenSource.Token).ContinueWith(task =>
            {
                if (task.IsCanceled)
                    _logger.LogWarning("GetLogicalDisksAsync canceled.");
                //return;
                else if (task.IsFaulted)
                //if (task.IsFaulted)
                {
                    if (!DesignerProperties.GetIsInDesignMode(this))
                        _logger.LogError(task.Exception, "Unexpected error while getting logical disks");
                    Dispatcher.Invoke(new Action<string, Exception>(NotifyError), "", task.Exception);
                }
                else
                    Dispatcher.Invoke(new Action<Win32_LogicalDisk[]>(OnGetLogicalDisksCompleted), new object[] { task.Result });
            }).ContinueWith(t => Dispatcher.Invoke(() => Application.Current.Exit -= handler));
        }

        private void OnGetLogicalDisksCompleted(Win32_LogicalDisk[] logicalDisks)
        {
            foreach (FolderVM ld in logicalDisks.Select(d => new FolderVM(d, 3)))
                InnerLogicalDisks.Add(ld);
        }
        private void NotifyError(string title, Exception exception)
        {
            MainWindow mainWindow = Services.ServiceProvider.GetService<MainWindow>();
            if (mainWindow is not null)
                MessageBox.Show(mainWindow, string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
