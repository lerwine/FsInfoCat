using FsInfoCat.Desktop.WMI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

        public static readonly DependencyProperty InstructionsTextProperty = DependencyProperty.Register(nameof(InstructionsText), typeof(string), typeof(FolderBrowserVM), new PropertyMetadata(""));

        public string InstructionsText
        {
            get => GetValue(InstructionsTextProperty) as string;
            set => SetValue(InstructionsTextProperty, value);
        }

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
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            string fullPath;
            if (string.IsNullOrWhiteSpace(newValue))
                fullPath = newValue;
            else
            {
                try { fullPath = System.IO.Path.GetFullPath(newValue); }
                catch (Exception exception)
                {
                    PathError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
                    return;
                }
                while (string.IsNullOrWhiteSpace(System.IO.Path.GetFileName(fullPath)))
                {
                    string p = System.IO.Path.GetDirectoryName(fullPath);
                    if (string.IsNullOrWhiteSpace(p))
                        break;
                    fullPath = p;
                }
            }
            if (string.IsNullOrEmpty(fullPath))
            {
                SelectedFolder = null;
                PathError = "";
            }
            else
            {
                FolderVM isSelected = SelectedFolder;
                if (isSelected is not null && StringComparer.InvariantCultureIgnoreCase.Equals(isSelected.Path, fullPath))
                    return;
                FolderVM.FindByPathAsync(LogicalDisks, fullPath).ContinueWith(task =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (task.IsCanceled)
                        {
                            PathError = FsInfoCat.Properties.Resources.Description_IO_Operation_Canceled;
                            return;
                        }
                        LinkedList<FolderVM> list;
                        try { list = task.Result; }
                        catch (Exception exception)
                        {
                            PathError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
                            return;
                        }
                        if (list is null) // Figure out what to do if null was returned but path does exist (i.e. directory created or drive added after browser window was opened)
                            PathError = FsInfoCat.Properties.Resources.ErrorMessage_DirectoryNotFound;
                        else
                        {
                            foreach (FolderVM f in list)
                                f.IsExpanded = true;
                            FolderVM wasSelected = SelectedFolder;
                            isSelected = list.Last.Value;
                            isSelected.IsSelected = true;
                            if (wasSelected is null)
                                SelectedFolder = isSelected;
                            else if (!ReferenceEquals(wasSelected, isSelected))
                            {
                                SelectedFolder = isSelected;
                                wasSelected.IsSelected = false;
                            }
                            PathError = "";
                        }
                    });
                });
            }
        }

        private static readonly DependencyPropertyKey PathErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PathError), typeof(string), typeof(FolderBrowserVM), new PropertyMetadata(""));

        public static readonly DependencyProperty PathErrorProperty = PathErrorPropertyKey.DependencyProperty;

        public string PathError
        {
            get => GetValue(PathErrorProperty) as string;
            private set => SetValue(PathErrorPropertyKey, value);
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

        public static readonly DependencyProperty SelectedFolderProperty = DependencyProperty.Register(nameof(SelectedFolder), typeof(FolderVM), typeof(FolderBrowserVM),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as FolderBrowserVM).OnSelectedFolderPropertyChanged((FolderVM)e.OldValue, (FolderVM)e.NewValue)));

        internal static DirectoryInfo Prompt(string title, string instructions)
        {
            View.FolderBrowserWindow window = new();
            FolderBrowserVM viewModel = (FolderBrowserVM)window.DataContext;
            if (viewModel is null)
            {
                viewModel = new FolderBrowserVM();
                window.DataContext = viewModel;
            }
            viewModel.InstructionsText = instructions ?? "";
            if (!string.IsNullOrWhiteSpace(title))
                window.Title = title;
            MainWindow mainWindow = Services.ServiceProvider.GetService<MainWindow>();
            if (mainWindow is not null)
                window.Owner = mainWindow;
            if (window.ShowDialog() ?? false)
            {
                string path = viewModel.SelectedFolder?.Path;
                if (!string.IsNullOrEmpty(path))
                    return new DirectoryInfo(path);
            }
            return null;
        }

        public FolderVM SelectedFolder
        {
            get => (FolderVM)GetValue(SelectedFolderProperty);
            set => SetValue(SelectedFolderProperty, value);
        }

        protected virtual void OnSelectedFolderPropertyChanged(FolderVM oldValue, FolderVM newValue)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            if (newValue is null)
            {
                OkClickCommand.IsEnabled = false;
                CurrentPath = "";
            }
            else
            {
                OkClickCommand.IsEnabled = true;
                CurrentPath = newValue.Path;
            }
        }

        private static readonly DependencyPropertyKey ValueSelectedCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ValueSelectedCommand), typeof(Commands.RelayCommand), typeof(FolderBrowserVM), new PropertyMetadata(null));

        public static readonly DependencyProperty ValueSelectedCommandProperty = ValueSelectedCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand ValueSelectedCommand => (Commands.RelayCommand)GetValue(ValueSelectedCommandProperty);

        protected virtual void OnValueSelectedExecute(object parameter)
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            if (parameter is FolderVM folder)
                SelectedFolder = folder;
            else if (parameter is null)
                SelectedFolder = null;
        }

        private static readonly DependencyPropertyKey OkClickCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(OkClickCommand), typeof(Commands.RelayCommand), typeof(FolderBrowserVM), new PropertyMetadata(null));

        public static readonly DependencyProperty OkClickCommandProperty = OkClickCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand OkClickCommand => (Commands.RelayCommand)GetValue(OkClickCommandProperty);

        private void InvokeOkClickCommand(object parameter)
        {
            if (parameter is Window window)
                window.DialogResult = true;
        }

        private static readonly DependencyPropertyKey CancelClickCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelClickCommand), typeof(Commands.RelayCommand), typeof(FolderBrowserVM), new PropertyMetadata(null));

        public static readonly DependencyProperty CancelClickCommandProperty = CancelClickCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand CancelClickCommand => (Commands.RelayCommand)GetValue(CancelClickCommandProperty);

        private void InvokeCancelClickCommand(object parameter)
        {
            if (parameter is Window window)
                window.DialogResult = false;
        }

        public FolderBrowserVM()
        {
            InnerLogicalDisks = new();
            LogicalDisks = new(InnerLogicalDisks);
            SetValue(ValueSelectedCommandPropertyKey, new Commands.RelayCommand(OnValueSelectedExecute));
            SetValue(OkClickCommandPropertyKey, new Commands.RelayCommand(InvokeOkClickCommand, false, SelectedFolder is null));
            SetValue(CancelClickCommandPropertyKey, new Commands.RelayCommand(InvokeCancelClickCommand));
            _logger = App.GetLogger(this);
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
#endif
            CancellationTokenSource tokenSource = new();
            ExitEventHandler handler = new((sender, e) => tokenSource.Cancel(true));
            Application.Current.Exit += handler;
            IDisposable scope = _logger.BeginScope("Getting logical disks");
            _ = Win32_LogicalDisk.GetLogicalDisksAsync(tokenSource.Token).ContinueWith(task =>
            {
                if (task.IsCanceled)
                    _logger.LogWarning("GetLogicalDisksAsync canceled.");
                else if (task.IsFaulted)
                {
                    _logger.LogError(ErrorCode.GetLogicalDisksFailure.ToEventId(), task.Exception, FsInfoCat.Properties.Resources.ErrorMessage_GetLogicalDisksFailure);
                    Dispatcher.Invoke(new Action<string, Exception>(NotifyError), "", task.Exception);
                }
                else
                {
                    FolderVM[] vm = (FolderVM[])Dispatcher.Invoke(new Func<Win32_LogicalDisk[], FolderVM[]>(OnGetLogicalDisksCompleted), new object[] { task.Result });
                    if (vm.Length > 0)
                    {
                        _logger.LogDebug("Pre-loading contents of {Count} root directories", vm.Length);
                        foreach (FolderVM f in vm)
                            f.PreloadAsync().Wait();
                    }
                    else
                        _logger.LogWarning("No root directories were created from {Count} logical disks.", task.Result.Length);
                }
            }).ContinueWith(t =>
            {
                try
                {
                    try { Dispatcher.Invoke(() => Application.Current.Exit -= handler); }
                    finally { scope.Dispose(); }
                }
                finally { tokenSource.Dispose(); }
            });
        }

        private FolderVM[] OnGetLogicalDisksCompleted(Win32_LogicalDisk[] logicalDisks)
        {
            FolderVM[] vm = logicalDisks.Select(d => new FolderVM(d)).ToArray();
            foreach (FolderVM ld in vm)
                InnerLogicalDisks.Add(ld);
            return vm;
        }

        private void NotifyError(string title, Exception exception)
        {
            MainWindow mainWindow = Services.ServiceProvider.GetService<MainWindow>();
            if (mainWindow is not null)
                MessageBox.Show(mainWindow, string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
