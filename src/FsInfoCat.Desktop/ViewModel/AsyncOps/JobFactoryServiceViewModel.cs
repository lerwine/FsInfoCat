using FsInfoCat.AsyncOps;
using FsInfoCat.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class JobFactoryServiceViewModel : DependencyObject
    {
        private readonly ILogger<JobFactoryServiceViewModel> _logger;
        private readonly EventObserver _eventObserver;
        private readonly ActiveStateObserver _activeStateObserver;

        #region Items Property Members

        private readonly ObservableCollection<BackgroundJobVM> _backingItems = new();

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Items), typeof(ReadOnlyObservableCollection<BackgroundJobVM>), typeof(JobFactoryServiceViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<BackgroundJobVM> Items => (ReadOnlyObservableCollection<BackgroundJobVM>)GetValue(ItemsProperty);

        #endregion
        #region IsBusy Property Members

        private static readonly DependencyPropertyKey IsBusyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsBusy), typeof(bool),
            typeof(JobFactoryServiceViewModel), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsBusy"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = IsBusyPropertyKey.DependencyProperty;

        public bool IsBusy { get => (bool)GetValue(IsBusyProperty); private set => SetValue(IsBusyPropertyKey, value); }

        #endregion

        private void OnOperationStarted([DisallowNull] ICancellableOperation progressInfo, MessageCode? code, [DisallowNull] IObservable<IBackgroundProgressEvent> observable) =>
            _backingItems.Add(BackgroundJobVM.Create(progressInfo, code, observable, item => Dispatcher.Invoke(() => _backingItems.Remove(item)), out BackgroundJobVM.ProgressObserver observer));

        public JobFactoryServiceViewModel()
        {
            _logger = App.GetLogger(this);
            _logger.LogDebug($"{nameof(JobFactoryServiceViewModel)} constructor invoked");
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<BackgroundJobVM>(_backingItems));
            IBackgroundProgressService backgroundService = Hosting.GetRequiredService<IBackgroundProgressService>();
            _eventObserver = new(this, backgroundService);
            _activeStateObserver = new(this, backgroundService);
            _logger.LogDebug($"{nameof(JobFactoryServiceViewModel)} Service instantiated");
        }

        [Obsolete("Use IBackgroundProgressService, instead")]
        private void AddJob(BackgroundJobVM item, IAsyncJob job) => Dispatcher.Invoke(() =>
        {
            lock (_backingItems)
            {
                _backingItems.Add(item);
                _ = job.Task.ContinueWith(task => RemoveJob(item));
            }
            IsBusy = _backingItems.Count > 0;
            return item;
        });

        [Obsolete("Use IBackgroundProgressService, instead")]
        private void RemoveJob(BackgroundJobVM item) => Dispatcher.Invoke(() =>
        {
            lock (_backingItems)
                _ = _backingItems.Remove(item);
            IsBusy = _backingItems.Count > 0;
        });

        internal void CancelAll()
        {
            // TODO: Implement CancelAll()
            throw new NotImplementedException("CancelAll not implemented");
        }
    }
}
