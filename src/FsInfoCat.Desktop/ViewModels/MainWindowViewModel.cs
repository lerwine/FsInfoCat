using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Schema;
using FsInfoCat.Desktop.Commands;

namespace FsInfoCat.Desktop.ViewModels
{
    public class MainWindowViewModel : DependencyObject
    {
        private CancellationTokenSource _tokenSource;

        public RelayCommand RunPendingJobCommand { get; }

        public RelayCommand RunAllPendingJobsCommand { get; }

        public RelayCommand StopJobCommand { get; }

        public RelayCommand SettingsCommand { get; }

        public RelayCommand ExitCommand { get; }

        public RelayCommand HelpCommand { get; }

        public RelayCommand AboutCommand { get; }

        #region "Is task running" (IsTaskRunning) Property Members

        /// <summary>
        /// Defines the name for the <see cref="IsTaskRunning" /> dependency property.
        /// </summary>
        public const string PropertyName_IsTaskRunning = "IsTaskRunning";

        private static readonly DependencyPropertyKey IsTaskRunningPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_IsTaskRunning, typeof(bool), typeof(MainWindowViewModel),
                new PropertyMetadata(false,
                    (d, e) => (d as MainWindowViewModel).OnIsTaskRunningPropertyChanged(e.OldValue as bool?, (bool)(e.NewValue)),
                    (d, baseValue) => CoerceIsTaskRunningValue(baseValue as bool?)
            )
        );

        /// <summary>
        /// Identifies the <see cref="IsTaskRunning" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTaskRunningProperty = IsTaskRunningPropertyKey.DependencyProperty;

        private void OnIsTaskRunningPropertyChanged(bool? oldValue, bool newValue)
        {
            if (newValue)
            {
                if (!(oldValue.HasValue && oldValue.Value))
                    TaskStatusMessage = DefaultValue_TaskStatusMessage;
                RunPendingJobCommand.IsEnabled = false;
                RunAllPendingJobsCommand.IsEnabled = false;
                StopJobCommand.IsEnabled = true;
            }
            else
            {
                RunAllPendingJobsCommand.IsEnabled = PendingJobs.Count > 0;
                RunPendingJobCommand.IsEnabled = null != SelectedPendingJob;
                StopJobCommand.IsEnabled = false;
            }
        }

        public static bool CoerceIsTaskRunningValue(bool? value)
        {
            if (value.HasValue)
                return value.Value;
            return false;
        }

        /// <summary>
        /// Is task running
        /// </summary>
        public bool IsTaskRunning
        {
            get
            {
                if (CheckAccess())
                    return (bool)(GetValue(IsTaskRunningProperty));
                return Dispatcher.Invoke(() => (bool)(GetValue(IsTaskRunningProperty)));
            }
            private set
            {
                if (CheckAccess())
                    SetValue(IsTaskRunningPropertyKey, value);
                else
                    Dispatcher.Invoke(() => SetValue(IsTaskRunningPropertyKey, value));
            }
        }

        #endregion

        #region "Task status message" (TaskStatusMessage) Property Members

        /// <summary>
        /// Defines the name for the <see cref="TaskStatusMessage" /> dependency property.
        /// </summary>
        public const string PropertyName_TaskStatusMessage = "TaskStatusMessage";

        /// <summary>
        /// Defines the value for the <see cref="TaskStatusMessage"/> dependency property when the associate file is new.
        /// </summary>
        public const string DefaultValue_TaskStatusMessage = "Initializing";

        private static readonly DependencyPropertyKey TaskStatusMessagePropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_TaskStatusMessage, typeof(string), typeof(MainWindowViewModel),
                new PropertyMetadata(DefaultValue_TaskStatusMessage, null,
                    (d, baseValue) => CoerceTaskStatusMessageValue(baseValue as string)
            )
        );

        /// <summary>
        /// Identifies the <see cref="TaskStatusMessage" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty TaskStatusMessageProperty = TaskStatusMessagePropertyKey.DependencyProperty;

        public static string CoerceTaskStatusMessageValue(string value)
        {
            if (null == value)
                return DefaultValue_TaskStatusMessage;
            return value;
        }

        /// <summary>
        /// Task status message
        /// </summary>
        public string TaskStatusMessage
        {
            get
            {
                if (CheckAccess())
                    return (string)(GetValue(TaskStatusMessageProperty));
                return Dispatcher.Invoke(() => (string)(GetValue(TaskStatusMessageProperty)));
            }
            private set
            {
                if (CheckAccess())
                    SetValue(TaskStatusMessagePropertyKey, value);
                else
                    Dispatcher.Invoke(() => SetValue(TaskStatusMessagePropertyKey, value));
            }
        }

        #endregion

        #region "Pending jobs" (PendingJobs) Property Members

        /// <summary>
        /// Defines the name for the <see cref="PendingJobs" /> dependency property.
        /// </summary>
        public const string PropertyName_PendingJobs = "PendingJobs";

        private static readonly DependencyPropertyKey PendingJobsPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_PendingJobs, typeof(ObservableCollection<PendingJobVM>), typeof(MainWindowViewModel),
                new PropertyMetadata(null,
                    (d, e) => (d as MainWindowViewModel).OnPendingJobsPropertyChanged(e.OldValue as ObservableCollection<PendingJobVM>, e.NewValue as ObservableCollection<PendingJobVM>),
                    (d, baseValue) => CoercePendingJobsValue(baseValue as ObservableCollection<PendingJobVM>)
            )
        );

        /// <summary>
        /// Identifies the <see cref="PendingJobs" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty PendingJobsProperty = PendingJobsPropertyKey.DependencyProperty;

        private void OnPendingJobsPropertyChanged(ObservableCollection<PendingJobVM> oldValue, ObservableCollection<PendingJobVM> newValue)
        {
            if (null != oldValue)
                oldValue.CollectionChanged -= OnPendingJobsCollectionChanged;
            newValue.CollectionChanged += OnPendingJobsCollectionChanged;
            OnPendingJobsCollectionChanged(newValue, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnPendingJobsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PendingJobVM item = SelectedPendingJob;
            if (null == item)
                return;
            SelectedPendingJobIndex = PendingJobs.IndexOf(item);
            if (!IsTaskRunning)
                RunAllPendingJobsCommand.IsEnabled = PendingJobs.Count > 0;
        }

        public static ObservableCollection<PendingJobVM> CoercePendingJobsValue(ObservableCollection<PendingJobVM> value)
        {
            if (null == value)
                return new ObservableCollection<PendingJobVM>();
            return value;
        }

        /// <summary>
        /// Pending jobs
        /// </summary>
        public ObservableCollection<PendingJobVM> PendingJobs
        {
            get
            {
                if (CheckAccess())
                    return (ObservableCollection<PendingJobVM>)(GetValue(PendingJobsProperty));
                return Dispatcher.Invoke(() => (ObservableCollection<PendingJobVM>)(GetValue(PendingJobsProperty)));
            }
            private set
            {
                if (CheckAccess())
                    SetValue(PendingJobsPropertyKey, value);
                else
                    Dispatcher.Invoke(() => SetValue(PendingJobsPropertyKey, value));
            }
        }

        #endregion

        #region "Selected pending job index" (SelectedPendingJobIndex) Property Members

        /// <summary>
        /// Defines the name for the <see cref="SelectedPendingJobIndex" /> dependency property.
        /// </summary>
        public const string PropertyName_SelectedPendingJobIndex = "SelectedPendingJobIndex";

        /// <summary>
        /// Identifies the <see cref="SelectedPendingJobIndex" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedPendingJobIndexProperty = DependencyProperty.Register(PropertyName_SelectedPendingJobIndex, typeof(int), typeof(MainWindowViewModel),
                new PropertyMetadata(-1,
                    (d, e) => (d as MainWindowViewModel).OnSelectedPendingJobIndexPropertyChanged(e.OldValue as int?, (int)(e.NewValue)),
                    (d, baseValue) => (d as MainWindowViewModel).CoerceSelectedPendingJobIndexValue(baseValue as int?)
            )
        );

        private void OnSelectedPendingJobIndexPropertyChanged(int? oldValue, int newValue)
        {
            if (oldValue.HasValue && oldValue.Value == newValue)
                return;
            if (newValue < 0)
                SelectedPendingJob = null;
            else
                SelectedPendingJob = PendingJobs[newValue];
        }

        public int CoerceSelectedPendingJobIndexValue(int? value)
        {
            if (value.HasValue && value.Value >= 0 && value < PendingJobs.Count)
                return value.Value;
            return -1;
        }

        /// <summary>
        /// Selected pending job index
        /// </summary>
        public int SelectedPendingJobIndex
        {
            get
            {
                if (CheckAccess())
                    return (int)(GetValue(SelectedPendingJobIndexProperty));
                return Dispatcher.Invoke(() => (int)(GetValue(SelectedPendingJobIndexProperty)));
            }
            set
            {
                if (CheckAccess())
                    SetValue(SelectedPendingJobIndexProperty, value);
                else
                    Dispatcher.Invoke(() => SetValue(SelectedPendingJobIndexProperty, value));
            }
        }

        #endregion

        #region "Selected pending job" (SelectedPendingJob) Property Members

        /// <summary>
        /// Defines the name for the <see cref="SelectedPendingJob" /> dependency property.
        /// </summary>
        public const string PropertyName_SelectedPendingJob = "SelectedPendingJob";

        /// <summary>
        /// Identifies the <see cref="SelectedPendingJob" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedPendingJobProperty = DependencyProperty.Register(PropertyName_SelectedPendingJob, typeof(PendingJobVM), typeof(MainWindowViewModel),
                new PropertyMetadata(null,
                    (d, e) => (d as MainWindowViewModel).OnSelectedPendingJobPropertyChanged(e.OldValue as PendingJobVM, e.NewValue as PendingJobVM),
                    (d, baseValue) => (d as MainWindowViewModel).CoerceSelectedPendingJobValue(baseValue as PendingJobVM)
            )
        );

        private void OnSelectedPendingJobPropertyChanged(PendingJobVM oldValue, PendingJobVM newValue)
        {
            if (null == newValue)
            {
                SelectedPendingJobIndex = -1;
                RunPendingJobCommand.IsEnabled = false;
            }
            else if (null == oldValue || !Object.ReferenceEquals(oldValue, newValue))
            {
                SelectedPendingJobIndex = PendingJobs.IndexOf(newValue);
                if (SelectedPendingJobIndex < 0)
                    SelectedPendingJob = null;
                else if (!IsTaskRunning)
                    RunPendingJobCommand.IsEnabled = true;
            }
        }

        public PendingJobVM CoerceSelectedPendingJobValue(PendingJobVM value)
        {
            if (null != value && !PendingJobs.Contains(value))
                return null;
            return value;
        }

        /// <summary>
        /// Selected pending job
        /// </summary>
        public PendingJobVM SelectedPendingJob
        {
            get
            {
                if (CheckAccess())
                    return (PendingJobVM)(GetValue(SelectedPendingJobProperty));
                return Dispatcher.Invoke(() => (PendingJobVM)(GetValue(SelectedPendingJobProperty)));
            }
            set
            {
                if (CheckAccess())
                    SetValue(SelectedPendingJobProperty, value);
                else
                    Dispatcher.Invoke(() => SetValue(SelectedPendingJobProperty, value));
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            RunPendingJobCommand = new RelayCommand(OnRunPendingJob, false, true);
            RunAllPendingJobsCommand = new RelayCommand(OnRunAllPendingJobs, false, true);
            StopJobCommand = new RelayCommand(OnStopJob, false, true);
            SettingsCommand = new RelayCommand(OnSettings, false, true);
            ExitCommand = new RelayCommand(OnExit, false, true);
            HelpCommand = new RelayCommand(OnHelp, false, true);
            AboutCommand = new RelayCommand(OnAbout, false, true);
        }

        private void OnJobTaskDone(Task task)
        {
            if (task.IsCompletedSuccessfully)
                TaskStatusMessage = "";
            else if (task.IsFaulted)
                TaskStatusMessage = (string.IsNullOrWhiteSpace(task.Exception.Message)) ? task.Exception.GetType().Name : task.Exception.Message;
            else
                TaskStatusMessage = "Cancelled";
            IsTaskRunning = false;
        }

        private void OnRunPendingJob(object parameter)
        {
            PendingJobVM pendingJob = parameter as PendingJobVM;
            if (null == pendingJob)
                return;
            IsTaskRunning = true;
            if (null != _tokenSource)
            {
                if (!_tokenSource.IsCancellationRequested)
                    _tokenSource.Cancel(true);
                _tokenSource.Dispose();
            }
            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                if (_tokenSource.Token.IsCancellationRequested)
                    return;
                Dispatcher.Invoke(() => PendingJobs.Remove(pendingJob));
                RunPendingJob(pendingJob, _tokenSource.Token);
                if (_tokenSource.Token.IsCancellationRequested)
                {
                    Dispatcher.Invoke(() => PendingJobs.Insert(0, pendingJob));
                    return;
                }
            }, _tokenSource.Token).ContinueWith(OnJobTaskDone);
        }

        private void RunPendingJob(PendingJobVM pendingJob, CancellationToken token)
        {
            // TODO: Implement RunPendingJob
        }

        private void OnRunAllPendingJobs()
        {
            IsTaskRunning = true;
            if (null != _tokenSource)
            {
                if (!_tokenSource.IsCancellationRequested)
                    _tokenSource.Cancel(true);
                _tokenSource.Dispose();
            }
            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => RunAllPendingJobs(_tokenSource.Token), _tokenSource.Token).ContinueWith(OnJobTaskDone);
        }

        private void RunAllPendingJobs(CancellationToken token)
        {
            Func<PendingJobVM> getNextJob = () => Dispatcher.Invoke(() =>
            {
                if (token.IsCancellationRequested || PendingJobs.Count == 0)
                    return null;
                PendingJobVM j = PendingJobs[0];
                PendingJobs.RemoveAt(0);
                return j;
            });

            for (PendingJobVM j = getNextJob(); null != j; j = getNextJob())
            {
                RunPendingJob(j, token);
                if (token.IsCancellationRequested)
                {
                    Dispatcher.Invoke(() => PendingJobs.Insert(0, j));
                    break;
                }
            }
        }

        private void OnStopJob()
        {
            CancellationTokenSource tokenSource = _tokenSource;
            if (null != tokenSource && !tokenSource.IsCancellationRequested)
                tokenSource.Cancel(true);
        }

        private void OnSettings()
        {
            // TODO: Implement OnSettings
        }

        private void OnExit()
        {
            CancellationTokenSource tokenSource = _tokenSource;
            if (null != tokenSource && !tokenSource.IsCancellationRequested)
                tokenSource.Cancel(true);
            App.Current.MainWindow.Close();
        }

        private void OnHelp()
        {
            // TODO: Implement OnHelp
        }

        private void OnAbout()
        {
            // TODO: Implement OnAbout
        }
    }
}
