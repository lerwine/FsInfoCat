using FsInfoCat.Local;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlConfigurationVM : DispatcherObject, INotifyPropertyChanged
    {
        private readonly CrawlConfiguration _crawlConfiguration;
        private TimeSpan? _ttl;
        private TimeSpan? _rescheduleInterval;

        public event EventHandler Edit;
        public event EventHandler Activate;
        public event EventHandler Deactivate;
        public event EventHandler Delete;
        public event PropertyChangedEventHandler PropertyChanged;

        public string DisplayName => _crawlConfiguration.DisplayName;

        public CrawlStatus StatusValue => _crawlConfiguration.StatusValue;

        public ushort MaxRecursionDepth => _crawlConfiguration.MaxRecursionDepth;

        public ulong? MaxTotalItems => _crawlConfiguration.MaxTotalItems;

        public TimeSpan? TTL => _ttl;

        public DateTime? LastCrawlStart => _crawlConfiguration.LastCrawlStart;

        public DateTime? LastCrawlEnd => _crawlConfiguration.LastCrawlEnd;

        public DateTime? NextScheduledStart => _crawlConfiguration.NextScheduledStart;

        public TimeSpan? RescheduleInterval => _rescheduleInterval;

        public bool RescheduleFromJobEnd => _crawlConfiguration.RescheduleFromJobEnd;

        public bool RescheduleAfterFail => _crawlConfiguration.RescheduleAfterFail;

        public string FullName { get; private set; }

        public string Notes => _crawlConfiguration.Notes;

        public DateTime? LastSynchronizedOn => _crawlConfiguration.LastSynchronizedOn;

        public Guid? UpstreamId => _crawlConfiguration.UpstreamId;

        public DateTime CreatedOn => _crawlConfiguration.CreatedOn;

        public DateTime ModifiedOn => _crawlConfiguration.ModifiedOn;

        public Commands.RelayCommand EditCommand { get; }

        public Commands.RelayCommand ActivateCommand { get; }

        public Commands.RelayCommand DeactivateCommand { get; }

        public Commands.RelayCommand DeleteCommand { get; }

        internal CrawlConfigurationVM([DisallowNull] CrawlConfiguration model, string fullName, Guid rootId)
        {
            FullName = fullName;
            Subdirectory root = (_crawlConfiguration = model).Root;
            EditCommand = new Commands.RelayCommand(() => Dispatcher.Invoke(() => Edit?.Invoke(this, EventArgs.Empty)));
            (ActivateCommand = new Commands.RelayCommand(() => Dispatcher.Invoke(() =>
            {
                try
                {
                    if (_crawlConfiguration.StatusValue == CrawlStatus.Disabled)
                        _crawlConfiguration.StatusValue = CrawlStatus.NotRunning;
                }
                finally { Activate?.Invoke(this, EventArgs.Empty); }
            }))).IsEnabled = false;
            DeactivateCommand = new Commands.RelayCommand(() => Dispatcher.Invoke(() =>
            {
                try { _crawlConfiguration.StatusValue = CrawlStatus.Disabled; }
                finally { Deactivate?.Invoke(this, EventArgs.Empty); }
            }));
            DeleteCommand = new Commands.RelayCommand(() => Dispatcher.Invoke(() => Delete?.Invoke(this, EventArgs.Empty)));

            model.PropertyChanged += Model_PropertyChanged;
            if (root is not null && root.Id != rootId)
                Subdirectory.LookupFullNameAsync(root).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                        Dispatcher.Invoke(() => OnLookupFullNameError(task.Exception));
                    else if (!task.IsCanceled)
                        Dispatcher.Invoke(() => OnLookupFullNameComplete(task.Result ?? ""));
                });
            OnTtlChanged();
            OnRescheduleIntervalChanged();
        }

        private void OnLookupFullNameComplete(string result)
        {
            if (FullName.Equals(result))
                return;
            FullName = result;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
        }

        private void OnLookupFullNameError(AggregateException exception)
        {
            MessageBox.Show(Application.Current.MainWindow, string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message,
                "Full Name Lookup Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CrawlConfiguration.StatusValue):
                    DeactivateCommand.IsEnabled = !(ActivateCommand.IsEnabled = _crawlConfiguration.StatusValue == CrawlStatus.Disabled);
                    break;
                case nameof(CrawlConfiguration.TTL):
                    OnTtlChanged();
                    break;
                case nameof(CrawlConfiguration.RescheduleInterval):
                    OnRescheduleIntervalChanged();
                    break;
                case nameof(CrawlConfiguration.Root):
                    Subdirectory root = _crawlConfiguration.Root;
                    if (root is null)
                        OnLookupFullNameComplete("");
                    else
                        Subdirectory.LookupFullNameAsync(root).ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                                Dispatcher.Invoke(() => OnLookupFullNameError(task.Exception));
                            else if (!task.IsCanceled)
                                Dispatcher.Invoke(() => OnLookupFullNameComplete(task.Result ?? ""));
                        });
                    return;
            }
            PropertyChanged?.Invoke(this, e);
        }

        private void OnTtlChanged()
        {
            long? seconds = _crawlConfiguration.TTL;
            _ttl = (seconds.HasValue) ? TimeSpan.FromSeconds(seconds.Value) : null;
        }

        private void OnRescheduleIntervalChanged()
        {
            long? seconds = _crawlConfiguration.RescheduleInterval;
            _rescheduleInterval = (seconds.HasValue) ? TimeSpan.FromSeconds(seconds.Value) : null;
        }
    }
}
